
namespace ToolDevProjekt.Control
{
    using System;
    using System.Xml;
    using System.Globalization;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;
    using System.Windows.Input;
    using System.Windows.Controls;
    using System.Windows;

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
        private Dictionary<string, PlayerType> playerTypes;
        private Dictionary<string, BitmapImage> playerSprites;
        private Dictionary<string, EnemyType> enemyTypes;
        private Dictionary<string, BitmapImage> enemySprites;
        private TileType selectedBrush;
        private string selectedPlayer;
        private string selectedEnemy;
        private BitmapImage currentPlayerSprite;
        private Vector2 backupPlayerPos;
        private bool pause;
        private bool playerSet;
        private bool enemySet;

        public int TileWidth;
        public int TileHeight;

        public bool PlayerSet { get { return this.playerSet; } }
        public bool EnemySet { get { return this.enemySet; } }
        public bool TileBrushSelected{get; private set;}
        public bool PlayerBrushSelected { get; private set; }
        public bool EnemyBrushSelected { get; private set; }
        public bool PlayGame { get; set; }
        public bool EndingGame { get; private set; }
        public bool EnemyBackup { get; private set; }

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
                        reader.ReadToFollowing("Type");
                        reader.ReadStartElement();
                        string tileType = reader.ReadContentAsString();

                        reader.ReadToFollowing("XPos");
                        reader.ReadStartElement();
                        int xPos = reader.ReadContentAsInt();

                        reader.ReadToFollowing("YPos");
                        reader.ReadStartElement();
                        int yPos = reader.ReadContentAsInt();

                        newMap.Tiles[xPos, yPos] = new MapTile(xPos * this.TileWidth, yPos * this.TileHeight, this.TileWidth, this.TileHeight, tileTypes[tileType]);
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

                        writer.WriteElementString("Type", tile.Type.Name);
                        writer.WriteElementString("XPos", (tile.Position.X/tile.Rect.Width).ToString());
                        writer.WriteElementString("YPos", (tile.Position.Y/tile.Rect.Width).ToString());

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
                    map.Tiles[x, y] = new MapTile(x * this.TileWidth, y * this.TileHeight, this.TileWidth, this.TileHeight, tileTypes["water"]);
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
            foreach (var enemy in enemyTypes)
            {
                if (enemy.Key == brushType)
                {
                    this.selectedEnemy = brushType;
                    EnemyBrushSelected = true;
                    PlayerBrushSelected = false;
                    TileBrushSelected = false;
                    return;
                }
            }
            foreach (var player in playerTypes)
            {
                if (player.Key == brushType)
                {
                    this.selectedPlayer = brushType;
                    EnemyBrushSelected = false;
                    PlayerBrushSelected = true;
                    TileBrushSelected = false;
                    return;
                }
            }
        }

        public void OnDraw(Vector2 position)
        {
            if (TileBrushSelected)
            {
                this.map.Tiles[position.X / this.TileWidth, position.Y / this.TileHeight].Type = tileTypes[this.selectedBrush.Name];
                this.mainWindow.UpdateMap(position, tileIMGs[this.selectedBrush.Name], this.selectedBrush.Name);
            }
            else if(PlayerBrushSelected)
            {
                if (map.Tiles[position.X / this.TileWidth, position.Y / this.TileHeight].Type.Walkable)
                {
                    this.playerSet = true;
                    this.currentPlayerSprite = this.playerSprites[this.selectedPlayer];
                    this.mainWindow.UpdateMap(position, playerSprites[this.selectedPlayer], selectedPlayer);
                }
            }
            else if (EnemyBrushSelected)
            {
                if (map.Tiles[position.X / this.TileWidth, position.Y / this.TileHeight].Type.Walkable)
                {
                    this.enemySet = true;
                    this.mainWindow.UpdateMap(position, enemySprites[this.selectedEnemy], selectedEnemy);
                }
            }
        }
        #region AppStartActivate

        private void Application_Activated(object sender, EventArgs e)
        {
            this.mainWindow = (MainWindow)MainWindow;

            RadioButton rbtn;
            //Set Player Types and Brushes
            TextBlock menuTextblock = new TextBlock();
            menuTextblock.Text = "Players";
            this.mainWindow.BrushStack.Children.Add(menuTextblock);

            playerTypes = new Dictionary<string, PlayerType>();

            playerTypes.Add("red", new PlayerType("red", 75, 4));
            playerTypes.Add("green", new PlayerType("green", 100, 2));
            playerTypes.Add("baby", new PlayerType("baby", 50, 6));

            playerSprites = new Dictionary<string, BitmapImage>();

            foreach (var type in playerTypes)
            {
                string spriteURI = "pack://application:,,,/Resources/" + type.Key + ".png";

                BitmapImage playerSprite = new BitmapImage();
                playerSprite.BeginInit();
                playerSprite.UriSource = new Uri(spriteURI);
                playerSprite.EndInit();

                playerSprites.Add(type.Key, playerSprite);

                rbtn = new RadioButton();
                rbtn.Margin = new System.Windows.Thickness(Convert.ToDouble(5));
                rbtn.Name = type.Key;
                rbtn.Checked += this.mainWindow.Brush_Checked;
                rbtn.Content = new Image
                {
                    Source = playerSprite,
                    Width = 32,
                    Height = 32
                };
                if (rbtn.Name == "grass")
                {
                    rbtn.IsChecked = true;
                }
                this.mainWindow.BrushStack.Children.Add(rbtn);
            }

            menuTextblock = new TextBlock();
            menuTextblock.Text = "Enemies";
            this.mainWindow.BrushStack.Children.Add(menuTextblock);

            enemyTypes = new Dictionary<string, EnemyType>();

            enemyTypes.Add("pike", new EnemyType("pike", 2));
            enemyTypes.Add("blob", new EnemyType("blob", 1));

            enemySprites = new Dictionary<string, BitmapImage>();

            foreach (var type in enemyTypes)
            {
                string spriteURI = "pack://application:,,,/Resources/" + type.Key + ".png";

                BitmapImage enemySprite = new BitmapImage();
                enemySprite.BeginInit();
                enemySprite.UriSource = new Uri(spriteURI);
                enemySprite.EndInit();

                enemySprites.Add(type.Key, enemySprite);

                rbtn = new RadioButton();
                rbtn.Margin = new System.Windows.Thickness(Convert.ToDouble(5));
                rbtn.Name = type.Key;
                rbtn.Checked += this.mainWindow.Brush_Checked;
                rbtn.Content = new Image
                {
                    Source = enemySprite,
                    Width = 32,
                    Height = 32
                };
                if (rbtn.Name == "grass")
                {
                    rbtn.IsChecked = true;
                }
                this.mainWindow.BrushStack.Children.Add(rbtn);
            }

            menuTextblock = new TextBlock();
            menuTextblock.Text = "Tile Brushes";

            this.mainWindow.BrushStack.Children.Add(menuTextblock);
            this.tileTypes = new Dictionary<string, TileType>();

            tileTypes.Add("grass", new TileType("grass", true, false));
            tileTypes.Add("grassflower", new TileType("grassflower", true, false));
            tileTypes.Add("earth", new TileType("earth", true, false));
            tileTypes.Add("stone", new TileType("stone", false, false));
            tileTypes.Add("sand", new TileType("sand", true, false));
            tileTypes.Add("water", new TileType("water", false, false));
            tileTypes.Add("lava", new TileType("lava", true, true));

            tileIMGs = new Dictionary<string, BitmapImage>();

            foreach (var tileType in tileTypes)
            {
                string imgURI = "pack://application:,,,/Resources/" + tileType.Key + ".png";

                BitmapImage tileIMG = new BitmapImage();
                tileIMG.BeginInit();
                tileIMG.UriSource = new Uri(imgURI);
                tileIMG.EndInit();

                tileIMGs.Add(tileType.Key, tileIMG);

                rbtn = new RadioButton();
                rbtn.Margin = new System.Windows.Thickness(Convert.ToDouble(5));
                rbtn.Name = tileType.Key;
                rbtn.Checked += this.mainWindow.Brush_Checked;
                rbtn.Content = new Image
                {
                    Source = tileIMG,
                    Width = 32,
                    Height = 32
                };
                if (rbtn.Name == "grass")
                {
                    rbtn.IsChecked = true;
                }
                this.mainWindow.BrushStack.Children.Add(rbtn);
            }

            this.TileWidth = 32;
            this.TileHeight = 32;

            this.PlayGame = false;
            this.playerSet = false;
        }
        #endregion

        #region Game
        private Player player;
        private List<Agent> enemies;
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

        public bool CanExecutePause()
        {
            if (this.PlayGame)
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
            if (!pause)
            {
                this.backupPlayerPos = (Vector2)this.mainWindow.PlayerIMG.Tag;
                this.player = new Player(this.playerSprites[this.selectedPlayer], this.backupPlayerPos, this.playerTypes[this.selectedPlayer]);
                for (int i = 0; i < (playerTypes.Count + enemyTypes.Count + 2); i++)
                {
                    this.mainWindow.BrushStack.Children[i].IsEnabled = false;
                }
                enemies = new List<Agent>();
                foreach (var enemyIMG in this.mainWindow.EnemyIMGs)
                {
                    enemies.Add(new Agent(enemySprites[enemyIMG.Name], (Vector2)enemyIMG.Tag, enemyTypes[enemyIMG.Name], enemyIMG.Name));
                }
            }
            this.pause = false;
        }

        public void ExecutePause()
        {
            this.PlayGame = false;
            this.pause = true;
            this.mainWindow.KeyDown -= this.mainWindow.OnKeyDown;
        }

        public void ExecuteEndGame()
        {
            this.player = null;
            this.PlayGame = false;
            this.EndingGame = true;
            this.mainWindow.KeyDown -= this.mainWindow.OnKeyDown;
            this.mainWindow.DrawMap(map);
            this.mainWindow.UpdateMap(this.backupPlayerPos, this.playerSprites[this.selectedPlayer], this.selectedPlayer);
            for (int i = 0; i <= (playerTypes.Count + enemyTypes.Count+2); i++)
            {
                this.mainWindow.BrushStack.Children[i].IsEnabled = true;
            }
            this.EndingGame = false;
            this.EnemyBackup = true;
            foreach (var agent in enemies)
            {
                this.mainWindow.UpdateMap(agent.BackupPos, this.enemySprites[agent.Name], agent.Name);
            }
            this.EnemyBackup = false;
        }

        public void OnPlayGame(App.Directions newDirection, Vector2 position)
        {
            this.mainWindow.UpdateMap(this.player.Move(newDirection, this.map), this.currentPlayerSprite, this.selectedPlayer);
        }


        #endregion
    }
}
