
namespace ToolDevProjekt.View
{
    using System;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using System.Windows.Controls;
    using System.Collections.Generic;
    using System.Windows.Input;
    using System.Diagnostics;

    using ToolDevProjekt.Control;
    using ToolDevProjekt.Model;

    public partial class MainWindow
    {
        private App controller;
        private bool brushDown = false;
        private Image[,] mapCanvasIMGs;
        private Image playerIMG;

        public MainWindow()
        {
            this.controller = (App)Application.Current;

            InitializeComponent();
        }

        private void CanExecuteNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecuteNew();
        }

        private void CanExecuteOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecuteOpen();
        }

        private void CanExecuteSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecuteSave();
        }

        private void CanExecuteClose(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecuteClose();
        }

        private void ExecutedNew(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecuteNew();
        }

        private void ExecutedOpen(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecuteOpen();
        }

        private void ExecutedSave(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecuteSave();
        }

        private void ExecutedClose(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecuteClose();
        }

        public void DrawMap(Map map)
        {
            MapCanvas.Children.Clear();
            MapCanvas.Width = this.controller.TileWidth * map.Width;
            MapCanvas.Height = this.controller.TileHeight * map.Height;

            mapCanvasIMGs = new Image[map.Width, map.Height];
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Image img = new Image
                    {
                        Width = this.controller.TileWidth,
                        Height = this.controller.TileHeight,
                        Source = this.controller.TileIMG(map.Tiles[x, y].Type),
                        Tag = new Vector2(map.Tiles[x,y].Position.X, map.Tiles[x,y].Position.Y)
                    };

                    img.MouseLeftButtonDown += this.BrushDown;
                    img.MouseLeftButtonUp += this.BrushUp;
                    img.MouseMove += this.OnDraw;
                    this.MapCanvas.Children.Add(img);
                    mapCanvasIMGs[x, y] = img;
                    Canvas.SetLeft(img, x * this.controller.TileWidth);
                    Canvas.SetTop(img, y * this.controller.TileHeight);
                    Canvas.SetZIndex(img, 1);
                }
            }
        }

        public void UpdateMap(Vector2 position, BitmapImage brushIMG)
        {
            if (this.controller.TileBrushSelected)
            {
                mapCanvasIMGs[position.X, position.Y].Source = brushIMG;
            }
            else
            {
                if (playerIMG == null)
                {
                    playerIMG = new Image
                    {
                        Width = 32,
                        Height = 32,
                        Source = brushIMG
                    };
                    this.MapCanvas.Children.Add(playerIMG);
                }
                Canvas.SetLeft(playerIMG,position.X * this.controller.TileWidth);
                Canvas.SetTop(playerIMG, position.Y * this.controller.TileHeight);
                Canvas.SetZIndex(playerIMG, 10);
            }
        }

        private void Brush_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton brush = (RadioButton)sender;
            this.controller.OnBrushSelected(brush.Name);
        }

        private void BrushDown(object sender, MouseButtonEventArgs e)
        {
            this.brushDown = true;
            Image img = (Image)sender;
            Vector2 position = (Vector2)img.Tag;
        }

        private void BrushUp(object sender, MouseButtonEventArgs e)
        {
            this.brushDown = false;
        }

        private void OnDraw(object sender, MouseEventArgs e)
        {
            if (!brushDown)
            {
                return;
            }

            Image img = (Image)sender;
            Vector2 position = (Vector2)img.Tag;
            this.controller.OnDraw(position);
        }

        private void ExecutePlay(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CanExecutePlay(object sender, CanExecuteRoutedEventArgs e)
        {

        }
    }
}
