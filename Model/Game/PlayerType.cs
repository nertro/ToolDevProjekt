using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model.Game
{
    class PlayerType
    {
        public string Name { get; private set; }
        public int Health { get; set; }
        public int Speed { get; set; }

        public PlayerType(string name, int health, int speed)
        {
            this.Name = name;
            this.Health = health;
            this.Speed = speed;
        }
    }
}
