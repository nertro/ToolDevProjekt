namespace ToolDevProjekt.Model
{
    public class Map
    {
        private int width;
        private int height;

        public int Width { get { return this.width; } }
        public int Height { get { return this.height; } }
        public MapTile[,] Tiles;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.Tiles = new MapTile[width, height];
        }
    }
}
