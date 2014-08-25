
namespace ToolDevProjekt.Control
{
    using System;
    using System.Xml;
    using System.Globalization;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using System.Windows.Input;

    using Microsoft.Win32;

    using ToolDevProjekt.Model;
    using ToolDevProjekt.Model.Game;
    using ToolDevProjekt.View;
    public partial class App
    {
        private MainWindow mainWindow;
        private NewWindow newWindow;
        private Map map;
        private Dictionary<string, TileType> tileTypes;
        private Dictionary<string, BitmapImage> tileIMGs;
        private Dictionary<string, BitmapImage> entitySprites;
        private TileType selectedBrush;
        private string selectedEntity;
        private BitmapImage entitySprite;
        private bool playerSet;

        public int TileWidth;
        public int TileHeight;

        public bool PlayerSet { get { return this.playerSet; } }
        public bool TileBrushSelected{get; private set;}
        public bool PlayGame { get; set; }

        public BitmapImage TileIMG(string tileType)
        {
            return tileIMGs[tileType];
        }

        public bool CanExecuteNew()
        {
            if (PlayGame)
            {
                return false;
            }
            return true;
        }

        public bool CanExecuteOpen()
        {
            if (PlayGame)
            {
                return false;
            }
            return true;
        }

        public bool CanExecuteSave()
        {
            if (PlayGame)
            {
                return false;
            }
            return true;
        }

        public bool CanExecuteClose()
        {
            return true;
        }
        #region openFile
        public void ExecuteOpen()
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                AddExtension = true,
                CheckFileExists = true,
                CheckPathExists = true,
                ValidateNames = true,
                DefaultExt = ".xml",
                Filter = "Map File (.xml)|*xml"
            };

            var checkDialog = openDialog.ShowDialog();
            if (checkDialog != true)
            {
                return;
            }

            using (var stream = openDialog.OpenFile())
            {
                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true,
                    IgnoreComments = true
                };

                using (var reader = XmlReader.Create(stream, settings))
                {
                    reader.Read();

                    reader.ReadToFollowing("Width");
                    reader.ReadStartElement();
                    int mapWidth = reader.ReadContentAsInt();

                    reader.ReadToFollowing("Height");
                    reader.ReadStartElement();
                    int mapHeight = reader.ReadContentAsInt();

                    Map newMap = new Map(mapWidth, mapHeight);

                    reader.ReadToFollowing("Tiles");

                    for (int i = 0; i < mapWidth * mapHeight; i++)
                    {
                        reader.ReadToFollowing("XPos");
                        reader.ReadStartElement();
                        int xPos = reader.ReadContentAsInt();

                        reader.ReadToFollowing("YPos");
                        reader.ReadStartElement();
                        int yPos = reader.ReadContentAsInt();

                        reader.ReadToFollowing("Type");
                        reader.ReadStartElement();
                        string tileType = reader.ReadContentAsString();

                        newMap.Tiles[xPos, yPos] = new MapTile(xPos, yPos, this.TileWidth, this.TileHeight, tileTypes[tileType]);
                    }

                    this.map = newMap;
                    this.mainWindow.DrawMap(map);
                }
            }
        }
        #endregion
        public void ExecuteNew()
        {
            this.OpenNewWindow();
        }
        #region saveFile
        public void ExecuteSave()
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                AddExtension = true,
                FileName = "Map",
                CheckPathExists = true,
                DefaultExt = ".xml",
                ValidateNames = true
            };

            var checkDialog = saveDialog.ShowDialog();
            if (checkDialog != true)
            {
                return;
            }

            using (var stream = saveDialog.OpenFile())
            {
                XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

                using (var writer = XmlWriter.Create(stream, settings))
                {
                    writer.WriteStartElement("Map");

                    writer.WriteElementString("Width", this.map.Width.ToString());
                    writer.WriteElementString("Height", this.map.Height.ToString());

                    writer.WriteStartElement("Tiles");

                    foreach (var tile in map.Tiles)
                    {
                        writer.WriteStartElement("Tile");

                        writer.WriteElementString("XPos", tile.Position.X.ToString());
                        writer.WriteElementString("YPos", tile.Position.Y.ToString());
                        writer.WriteElementString("Type", tile.Type.Name);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
        }
        #endregion
        public void ExecuteClose()
        {
            this.Shutdown();
        }
        #region newMap
        public void CreateNewMap(int width, int height)
        {
            this.map = new Map(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map.Tiles[x, y] = new MapTile(x * this.TileWidth, y * this.TileHeight, this.TileWidth, this.TileHeight, tileTypes["Water"]);
                }
            }

            this.newWindow.Close();
            this.mainWindow.DrawMap(map);
        }

        private void OpenNewWindow()
        {
            if (this.newWindow == null || !this.newWindow.IsLoaded)
            {
                this.newWindow = new NewWindow();
            }

            this.newWindow.Show();
        }
        #endregion
        public void OnBrushSelected(string brushType)
        {
            foreach (var tileType in tileTypes)
            {
                if (tileType.Key == brushType)
                {
                    this.selectedBrush = tileTypes[brushType];
                    TileBrushSelected = true;
                    return;
                }
            }
            TileBrushSelected = false;
            this.selectedEntity = brushType;
        }

        public void OnDraw(Vector2 position)
        {
            if (TileBrushSelected)
            {
                this.map.Tiles[position.X / this.TileWidth, position.Y / this.TileHeight].Type = tileTypes[this.selectedBrush.Name];
                this.mainWindow.UpdateMap(position, tileIMGs[this.selectedBrush.Name]);
            }
            else
            {
                if (map.Tiles[position.X / this.TileWidth, position.Y / this.TileHeight].Type.Walkable)
                {
                    this.playerSet = true;
                    this.mainWindow.UpdateMap(position, entitySprite);
                }
            }
        }
        #region AppStartActivate
        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            this.tileTypes = new Dictionary<string, TileType>();

            tileTypes.Add("Desert", new TileType("Desert", true));
            tileTypes.Add("Grass", new TileType("Grass", true));
            tileTypes.Add("Water", new TileType("Water", false));

            tileIMGs = new Dictionary<string, BitmapImage>();

            foreach (var tileType in tileTypes)
            {
                string imgURI = "pack://application:,,,/Resources/" + tileType.Key + ".png";

                BitmapImage tileIMG = new BitmapImage();
                tileIMG.BeginInit();
                tileIMG.UriSource = new Uri(imgURI);
                tileIMG.EndInit();

                tileIMGs.Add(tileType.Key, tileIMG);
            }

            this.TileWidth = 32;
            this.TileHeight = 32;

            entitySprites = new Dictionary<string, BitmapImage>();

            entitySprite = new BitmapImage();
            string spriteURI = "pack://application:,,,/Resources/Link.png";
            entitySprite.BeginInit();
            entitySprite.UriSource = new Uri(spriteURI);
            entitySprite.EndInit();

            entitySprites.Add("Link", entitySprite);
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            this.mainWindow = (MainWindow)MainWindow;
            this.PlayGame = false;
            this.playerSet = false;
        }
        #endregion

        #region Game
        private Player player;
        public enum Directions
        {
            Up,
            Down,
            Left,
            Right
        }

        public bool CanExecutePlay()
        {
            if (playerSet)
            {
                return true;
            }

            return false;
        }

        public bool CanExecuteEndGame()
        {
            if (PlayGame)
            {
                return true;
            }
            return false;
        }

        public void ExecutePlay()
        {
            this.PlayGame = true;
            this.mainWindow.KeyDown += new KeyEventHandler(this.mainWindow.OnKeyDown);
            this.player = new Player(this.entitySprite, (Vector2)this.mainWindow.PlayerIMG.Tag);
        }

        public void ExecuteEndGame()
        {
            this.PlayGame = false;
            this.player = null;
            this.mainWindow.KeyDown -= this.mainWindow.OnKeyDown;
        }

        public void OnPlayGame(App.Directions newDirection, Vector2 position)
        {
            this.mainWindow.UpdateMap(this.player.Move(newDirection, this.map), entitySprite);
        }

        #endregion
    }
}
