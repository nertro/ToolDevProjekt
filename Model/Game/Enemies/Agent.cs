using System;
using System.Windows;
using System.Windows.Media.Imaging;
using ToolDevProjekt.Model.Game;
using ToolDevProjekt.Control;

namespace ToolDevProjekt.Model
{
    class Agent : Entity
    {
        public string Name { get; set; }
        public EnemyType Type { get; private set; }
        public int Health { get; set; }
        public Agent Player { get; private set; }
        private Brain myBrain;
        public Brain MyBrain { get { return this.myBrain; } }
        public App Controller { get; private set; }
        public Vector2 BackupPos { get; private set; }

        //for evaluation
        public float GetDistanceToPlayer()
        {
            return 100;
        }

        //Constructor
        public Agent(BitmapImage sprite, Vector2 position, EnemyType type, string name)
        {
            this.Name = name;
            this.Sprite = sprite;
            this.Position = position;
            this.BackupPos = position;
            this.Rect = new Rectangle((int)sprite.Width, (int)sprite.Height, position);

            this.myBrain = new Brain(this, Goal<Agent>.Types.composite);
            this.Health = 100;
            this.myBrain.Activate();
            this.Type = type;
            this.Controller = (App)Application.Current;
        }
    }
}
