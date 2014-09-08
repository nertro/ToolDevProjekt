
namespace ToolDevProjekt.Model
{
    public class TileType
    {
        public string Name { get; set; }
        public bool Walkable { get; private set; }
        public bool DoesDamage { get; private set; }

        public TileType(string name, bool walkable, bool doesDamage)
        {
            this.Name = name;
            this.Walkable = walkable;
            this.DoesDamage = doesDamage;
        }
    }
}
