
namespace ToolDevProjekt.Control
{
    using System;
    using System.Xml;
    using System.Globalization;
    using System.Collections;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    using Microsoft.Win32;

    using ToolDevProjekt.Model;
    using ToolDevProjekt.View;
    public partial class App
    {
        private MainWindow mainWindow;
        private NewWindow newWindow;
        private Map map;
        private Dictionary<string, TileType> tileTypes;
        private Dictionary<string, BitmapImage> tileIMGs;
        private double tileWidth;
        private double tileHeight;
        private TileType selectedBrush;

        public BitmapImage TileIMG(string tileType)
        {
            return tileIMGs[tileType];
        }

        public bool CanExecuteNew()
        {
            return true;
        }

        public bool CanExecuteOpen()
        {
            return true;
        }

        public bool CanExecuteSave()
        {
            return true;
        }

        public bool CanExecuteClose()
        {
            return true;
        }

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

                        newMap.Tiles[xPos, yPos] = new MapTile(xPos, yPos, tileType);
                    }

                    this.map = newMap;
                    this.mainWindow.DrawMap(map);
                }
            }
        }

        public void ExecuteNew()
        {
            this.OpenNewWindow();
        }

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
                        writer.WriteElementString("Type", tile.Type);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();

                    writer.WriteEndElement();
                }
            }
        }

        public void ExecuteClose()
        {
            this.Shutdown();
        }

        public void CreateNewMap(int width, int height)
        {
            this.map = new Map(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map.Tiles[x, y] = new MapTile(x, y, "Water");
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

        public void OnBrushSelected(string tileType)
        {
            this.selectedBrush = tileTypes[tileType];
        }

        public void OnDraw(Vector2 position)
        {
            this.map.Tiles[position.X, position.Y].Type = this.selectedBrush.Name;
            this.mainWindow.UpdateMap(position, tileIMGs[this.selectedBrush.Name]);
        }

        private void Application_Startup(object sender, System.Windows.StartupEventArgs e)
        {
            this.tileTypes = new Dictionary<string, TileType>();

            tileTypes.Add("Desert", new TileType("Desert"));
            tileTypes.Add("Grass", new TileType("Grass"));
            tileTypes.Add("Water", new TileType("Water"));

            tileIMGs = new Dictionary<string, BitmapImage>();

            foreach (var tileType in tileTypes)
            {
                string imgURI = "pack://application:,,,/Resources/" + tileType.Key + ".png";

                BitmapImage tileIMG = new BitmapImage();
                tileIMG.BeginInit();
                tileIMG.UriSource = new Uri(imgURI);
                tileIMG.EndInit();

                tileIMGs.Add(tileType.Key, tileIMG);
                tileWidth = tileIMG.Width;
                tileHeight = tileIMG.Height;
            }
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            this.mainWindow = (MainWindow)MainWindow;
        }
    }
}
