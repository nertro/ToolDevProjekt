namespace ToolDevProjekt.Model.Game
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Timers;
    using System.Windows.Media.Imaging;

    using ToolDevProjekt.Control;

    class GameManager
    {
        private App controller;
        private Timer timer;
        private Player player;
        private Map map;

        public GameManager(Map map, Vector2 playerPos, Image playerSprite)
        {
            this.controller = (App)Application.Current;
            this.timer = new Timer(1000);
            this.controller.MainWindow.KeyDown += new KeyEventHandler(OnKeyDown);
            this.player = new Player(playerSprite, playerPos);
            this.map = map;
            //this.timer.Elapsed += new ElapsedEventHandler(OnTimer);
        }

        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W || e.Key == Key.Up)
            {
                this.player.Move(Player.Directions.Up, this.map);
            }
        }

        //public void RunGame()
        //{
        //    if (this.controller.PlayGame)
        //    {
        //        this.timer.Start();
        //    }
        //    else
        //    {
        //        this.timer.Stop();
        //    }
        //}

        //public void OnTimer(object sender, ElapsedEventArgs e)
        //{ 
        //}
    }
}
