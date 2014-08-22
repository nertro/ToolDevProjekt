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

        public MapTile(int x, int y, string type)
        {
            this.Position = new Vector2(x, y);
            this.Type = type;
        }
    }
}
