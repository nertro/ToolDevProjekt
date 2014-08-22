
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
            if (other.position.X > this.position.X 
                && other.position.X < this.position.X + this.width
                && other.position.Y > this.position.Y
                && other.position.Y < this.position.Y + this.height)
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
