using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model
{
    class EnemyType
    {
        public string Name { get; set; }
        public int Aggression { get; private set; }

        public EnemyType(string name, int aggr)
        {
            this.Name = name;
            this.Aggression = aggr;
        }
    }
}
