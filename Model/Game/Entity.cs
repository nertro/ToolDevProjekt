using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ToolDevProjekt.Model.Game
{
    abstract class Entity
    {
        public Rectangle Rect { get; set; }
        public Vector2 Position { get; set; }
        public BitmapImage Sprite { get; set; }
    }
}
