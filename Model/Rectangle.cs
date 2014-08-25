
namespace ToolDevProjekt.Model
{
    using System;

    public class Rectangle
    {
        private int width;
        private int height;

        public Vector2 position;
        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }

        public bool Intersect(Rectangle other)
        {
            if (this.position.X >= other.position.X 
                && this.position.X < other.position.X + other.Width
                && this.position.Y >= other.position.Y
                && this.position.Y < other.position.Y + other.Height)
            {
                return true;
            }
            else if (this.position.X + this.width >= other.position.X 
                && this.position.X + this.width < other.position.X + other.Width
                && this.position.Y >= other.position.Y
                && this.position.Y < other.position.Y + other.Height)
            {
                return true;
            }
            else if (this.position.X >= other.position.X
              && this.position.X < other.position.X + other.Width
              && this.position.Y + this.height >= other.position.Y
              && this.position.Y + this.height < other.position.Y + other.Height)
            {
                return true;
            }
            else if (this.position.X + this.Width >= other.position.X
              && this.position.X + this.Width < other.position.X + other.Width
              && this.position.Y + this.height >= other.position.Y
              && this.position.Y + this.height < other.position.Y + other.Height)
            {
                return true;
            }

            return false;
        }

        public Rectangle(int width, int height, Vector2 position)
        {
            this.width = width;
            this.height = height;
            this.position = position;
        }
    }
}
