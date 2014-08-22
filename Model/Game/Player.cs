namespace ToolDevProjekt.Model.Game
{
    using System;
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    class Player : Entity
    {
        private int health;
        private int speed;
        private Image sprite;
        private Directions currentDirection;
        private Vector2 position;
        private Rectangle rect;

        public enum Directions
        { 
            Up,
            Down,
            Left,
            Right
        }

        public void Move(Directions newDirection, Map map)
        {
            Vector2 newPos = this.position;

            if (newDirection == Directions.Up)
            {
                newPos = new Vector2(this.position.X, this.position.Y - this.speed);
            }
            else if (newDirection == Directions.Down)
            {
                newPos = new Vector2(this.position.X, this.position.Y + this.speed);
            }
            else if (newDirection == Directions.Left)
            {
                newPos = new Vector2(this.position.X - this.speed, this.position.Y);
            }
            else if (newDirection == Directions.Right)
            {
                newPos = new Vector2(this.position.X + this.speed, this.position.Y);
            }

            if (posIsValid(newPos, map))
            {
                this.position = newPos;
            }
        }

        public bool posIsValid(Vector2 position, Map map)
        {
            foreach (var tile in map.Tiles)
            {
                if (this.rect.Intersect(tile.rect))
                {
                    return false;
                }
            }

            return true;
        }

        public Player(Image sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.health = 100;
            this.speed = 1;
            this.currentDirection = Directions.Down;
            this.position = position;
            this.rect = new Rectangle((int)sprite.Width, (int)sprite.Height, this.position);
        }

    }
}
