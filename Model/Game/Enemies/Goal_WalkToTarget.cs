using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToolDevProjekt.Model.Game;

namespace ToolDevProjekt.Model
{
    class Goal_WalkToTarget :Goal<Agent>
    {
        private Entity target;
        private Vector2 direction;

        public override void Activate()
        {
            this.myStatus = States.active;
            this.direction = Vector2.Substract(this.target.Position, this.owner.Position);
        }

        public override Goal<Agent>.States Process()
        {
            if (this.myStatus == States.inactive)
            {
                this.Activate();
            }
            if (this.owner.Rect.Intersect( this.target.Rect))
            {
                this.myStatus = States.completed;
                return this.myStatus;
            }
            if (this.target == null)
            {
                myStatus = States.failed;
                return myStatus;
            }

            this.direction = Vector2.Substract(this.target.Position, this.owner.Position);
            Vector2.Normalize(direction);
            this.owner.Position = Vector2.Add(this.owner.Position, this.direction);
            this.owner.Rect = new Rectangle(this.owner.Rect.Width, this.owner.Rect.Height, this.owner.Position);

            return this.myStatus;
        }

        public override void Terminate()
        {
        }

        public Goal_WalkToTarget(Agent owner, Types type, Entity target)
        {
            this.owner = owner;
            this.myType = type;
            this.myStatus = States.inactive;
            this.target = target;
        }
    }
}
