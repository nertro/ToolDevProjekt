namespace ToolDevProjekt.Model.Game
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using ToolDevProjekt.Control;
    class Player : Entity
    {
        public PlayerType Type{ get; private set;}

        public Vector2 Move(App.Directions newDirection, Map map)
        {
            Rectangle newRect = this.Rect;

            if (newDirection == App.Directions.Up)
            {
                newRect.position = new Vector2(this.Position.X, this.Position.Y - this.Type.Speed);
            }
            else if (newDirection == App.Directions.Down)
            {
                newRect.position = new Vector2(this.Position.X, this.Position.Y + this.Type.Speed);
            }
            else if (newDirection == App.Directions.Left)
            {
                newRect.position = new Vector2(this.Position.X - this.Type.Speed, this.Position.Y);
            }
            else if (newDirection == App.Directions.Right)
            {
                newRect.position = new Vector2(this.Position.X + this.Type.Speed, this.Position.Y);
            }

            if (posIsValid(newRect, map))
            {
                this.Position = newRect.position;
                this.Rect.position = newRect.position;
            }

            return this.Position;
        }

        public bool posIsValid(Rectangle newRect, Map map)
        {
            foreach (var tile in map.Tiles)
            {
                if (newRect.Intersect(tile.Rect))
                {
                    if (!tile.Type.Walkable)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Player(BitmapImage sprite, Vector2 position, PlayerType type)
        {
            this.Sprite = sprite;
            this.Type = type;
            this.Position = position;
            this.Rect = new Rectangle(32, 32, this.Position);
        }

    }
}
