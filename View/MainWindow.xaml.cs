﻿
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
        private List<Image> enemyIMGs;

        public Image PlayerIMG { get { return this.playerIMG; } }
        public List<Image> EnemyIMGs { get { return this.enemyIMGs; } }

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
            if (!this.controller.PlayGame)
            {
                this.playerIMG = null;
                this.enemyIMGs = new List<Image>(); ;
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
                            Source = this.controller.TileIMG(map.Tiles[x, y].Type.Name),
                            Tag = new Vector2(map.Tiles[x, y].Position.X, map.Tiles[x, y].Position.Y)
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
        }

        public void UpdateMap(Vector2 position, BitmapImage brushIMG, string name)
        {
            if (this.controller.TileBrushSelected &! this.controller.PlayGame &! this.controller.EndingGame)
            {
                mapCanvasIMGs[position.X / this.controller.TileWidth, position.Y / this.controller.TileHeight].Source = brushIMG;
            }
            else if(this.controller.EndingGame || this.controller.PlayerBrushSelected || this.controller.PlayGame)
            {
                if (playerIMG == null)
                {
                    playerIMG = new Image
                    {
                        Width = 32,
                        Height = 32,
                        Source = brushIMG,
                        Tag = new Vector2(position.X, position.Y)
                    };
                    playerIMG.MouseLeftButtonDown += this.BrushDown;
                    playerIMG.MouseLeftButtonUp += this.BrushUp;
                    playerIMG.MouseMove += this.OnDraw;
                    this.MapCanvas.Children.Add(playerIMG);
                }
                if (playerIMG.Source != brushIMG)
                {
                    playerIMG.Source = brushIMG;
                }
                playerIMG.Tag = new Vector2(position.X, position.Y);
                Canvas.SetLeft(playerIMG,position.X);
                Canvas.SetTop(playerIMG, position.Y);
                Canvas.SetZIndex(playerIMG, 10);
            }
            else if (this.controller.EnemyBackup || this.controller.EnemyBrushSelected &! this.controller.PlayGame)
            {
                    Image enemyIMG = new Image
                    {
                        Name = name,
                        Width = 32,
                        Height = 32,
                        Source = brushIMG,
                        Tag = new Vector2(position.X, position.Y)
                    };
                    enemyIMG.MouseLeftButtonDown += this.BrushDown;
                    enemyIMG.MouseLeftButtonUp += this.BrushUp;
                    enemyIMG.MouseMove += this.OnDraw;
                    this.MapCanvas.Children.Add(enemyIMG);
                    this.enemyIMGs.Add(enemyIMG);
                    enemyIMG.Tag = new Vector2(position.X, position.Y);
                    Canvas.SetLeft(enemyIMG, position.X);
                    Canvas.SetTop(enemyIMG, position.Y);
                    Canvas.SetZIndex(enemyIMG, 10);
            }
        }

        public void Brush_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton brush = (RadioButton)sender;
            this.controller.OnBrushSelected(brush.Name);
        }

        private void Brush_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButton brush = (RadioButton)sender;
            this.controller.OnBrushSelected(brush.Name);
        }

        private void BrushDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.controller.PlayGame)
            {
                this.brushDown = true;

                Image img = (Image)sender;
                Vector2 position = (Vector2)img.Tag;
                this.controller.OnDraw(position);
            }
        }

        private void BrushUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.controller.PlayGame)
            {
                this.brushDown = false;
            }
        }

        private void OnDraw(object sender, MouseEventArgs e)
        {
            if (!brushDown || this.controller.PlayGame)
            {
                return;
            }

            Image img = (Image)sender;
            Vector2 position = (Vector2)img.Tag;
            this.controller.OnDraw(position);
        }

        #region Game
        private void ExecutePlay(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecutePlay();
        }

        private void CanExecutePlay(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecutePlay();
        }

        private void ExecuteEndGame(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecuteEndGame();
        }

        private void CanExecuteEndGame(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecuteEndGame();
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (this.controller.PlayGame)
            {
                if (e.Key == Key.W || e.Key == Key.Up)
                {
                    this.controller.OnPlayGame(App.Directions.Up, (Vector2)this.playerIMG.Tag);
                }
                else if (e.Key == Key.S || e.Key == Key.Down)
                {
                    this.controller.OnPlayGame(App.Directions.Down, (Vector2)this.playerIMG.Tag);
                }
                else if (e.Key == Key.A || e.Key == Key.Left)
                {
                    this.controller.OnPlayGame(App.Directions.Left, (Vector2)this.playerIMG.Tag);
                }
                else if (e.Key == Key.D || e.Key == Key.Right)
                {
                    this.controller.OnPlayGame(App.Directions.Right, (Vector2)this.playerIMG.Tag);
                }
            }
        }

        private void ExecutedPause(object sender, ExecutedRoutedEventArgs e)
        {
            this.controller.ExecutePause();
        }

        private void CanExecutePause(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = this.controller.CanExecutePause();
        }

        #endregion
    }
}
