
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
            MapCanvas.Width = 32 * map.Width;
            MapCanvas.Height = 32 * map.Height;

            mapCanvasIMGs = new Image[map.Width, map.Height];
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Image img = new Image
                    {
                        Width = 32,
                        Height = 32,
                        Source = this.controller.TileIMG(map.Tiles[x, y].Type),
                        Tag = new Vector2(map.Tiles[x,y].Position.X, map.Tiles[x,y].Position.Y)
                    };

                    img.MouseLeftButtonDown += this.BrushDown;
                    img.MouseLeftButtonUp += this.BrushUp;
                    img.MouseMove += this.OnDraw;
                    this.MapCanvas.Children.Add(img);
                    mapCanvasIMGs[x, y] = img;
                    Canvas.SetLeft(img, x * 32);
                    Canvas.SetTop(img, y * 32);
                }
            }
        }

        public void UpdateMap(Vector2 position, BitmapImage tileIMG)
        {
            mapCanvasIMGs[position.X, position.Y].Source = tileIMG;
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
            this.controller.OnDraw(position);
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
    }
}
