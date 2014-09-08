using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolDevProjekt.Model
{
    class Goal_Attack :Goal<Agent>
    {
        public override void Activate()
        {
            this.myStatus = States.active;
        }

        public override Goal<Agent>.States Process()
        {
            if (this.myStatus == States.inactive)
            {
                this.Activate();
            }
            else
            {
                this.myStatus = States.failed;
            }

            this.owner.Player.Health--;

            if (this.owner.Player.Health <= 0)
            {
                this.owner.Controller.ExecuteEndGame();
                this.myStatus = States.completed;
            }

            return this.myStatus;
        }

        public override void Terminate()
        {
        }

        public Goal_Attack(Agent owner, Types type)
        {
            this.owner = owner;
            this.myType = type;
            this.myStatus = States.inactive;
        }
    }
}
