using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model
{
    public class MapTile
    {
        public Vector2 Position { get; private set; }
        public string Type { get; set; }
        public Rectangle rect { get; set; }

        public MapTile(int x, int y, int width, int height, string type)
        {
            this.Position = new Vector2(x, y);
            this.Type = type;
            this.rect = new Rectangle(width, height, this.Position);
        }
    }
}
