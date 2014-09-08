using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolDevProjekt.Model
{
    class Goal_DestroyOther :GoalComposite
    {
        public override void Activate()
        { 
            this.RemoveAllSubgoals();
            this.myStatus = States.active;
            this.subgoalStack = new Stack<Goal<Agent>>();
            this.AddSubgoals(new Goal_Attack(this.owner, Types.atomic));
            this.AddSubgoals(new Goal_WalkToTarget(this.owner, Types.atomic, this.owner.Player));
        }

        public override Goal<Agent>.States Process()
        {
            if (this.myStatus == States.inactive)
            {
                this.Activate();
            }

            if (!this.owner.Rect.Intersect(this.owner.Player.Rect))
            {
                if (subgoalStack.Peek().GetType() != typeof(Goal_WalkToTarget))
                {
                    this.AddSubgoals(new Goal_WalkToTarget(this.owner, Types.atomic, this.owner.Player));
                }
            }
            
            this.myStatus = ProcessSubgoals();

            return this.myStatus;
        }

        public override void Terminate()
        {
            this.myStatus = States.inactive;
        }

        public Goal_DestroyOther(Agent owner, Types type)
        {
            this.owner = owner;
            this.myType = type;
            this.myStatus = States.inactive;
        }
    }
}
