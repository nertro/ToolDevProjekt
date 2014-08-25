namespace ToolDevProjekt.Model.Game
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using ToolDevProjekt.Control;
    class Player : Entity
    {
        private int health;
        private int speed;
        private BitmapImage sprite;
        private Vector2 position;
        private Rectangle rect;

        public Vector2 Position
        {
            get { return this.position; }
        }

        public Vector2 Move(App.Directions newDirection, Map map)
        {
            Rectangle newRect = this.rect;

            if (newDirection == App.Directions.Up)
            {
                newRect.position = new Vector2(this.position.X, this.position.Y - this.speed);
            }
            else if (newDirection == App.Directions.Down)
            {
                newRect.position = new Vector2(this.position.X, this.position.Y + this.speed);
            }
            else if (newDirection == App.Directions.Left)
            {
                newRect.position = new Vector2(this.position.X - this.speed, this.position.Y);
            }
            else if (newDirection == App.Directions.Right)
            {
                newRect.position = new Vector2(this.position.X + this.speed, this.position.Y);
            }

            if (posIsValid(newRect, map))
            {
                this.position = newRect.position;
                this.rect.position = newRect.position;
            }

            return this.position;
        }

        public bool posIsValid(Rectangle newRect, Map map)
        {
            foreach (var tile in map.Tiles)
            {
                if (newRect.Intersect(tile.rect))
                {
                    if (!tile.Type.Walkable)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Player(BitmapImage sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.health = 100;
            this.speed = 3;
            this.position = position;
            this.rect = new Rectangle(32, 32, this.position);
        }

    }
}
