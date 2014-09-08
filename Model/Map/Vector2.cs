using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model
{
    public class Vector2
    {
        private readonly int x;
        private readonly int y;

        public int X 
        {
            get { return this.x;}
        }

        public int Y
        {
            get { return this.y; }
        }

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 Substract(Vector2 first,Vector2 second)
        {
            return new Vector2(first.X - second.X, first.Y - second.Y);
        }

        public static Vector2 Add(Vector2 first, Vector2 second)
        {
            return new Vector2(first.X + second.X, first.Y + second.Y);
        }

        public static Vector2 Normalize(Vector2 vec)
        {
            double length = Math.Sqrt(Math.Sqrt(vec.x)+Math.Sqrt(vec.y));
            return new Vector2(vec.x / (int)length, vec.y / (int)length);
        }
    }
}
