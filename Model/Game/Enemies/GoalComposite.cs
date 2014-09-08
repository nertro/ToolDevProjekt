using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model
{
    abstract class GoalComposite :Goal<Agent>
    {
        protected Stack<Goal<Agent>> subgoalStack;

        public override void Activate()
        {
            throw new NotImplementedException();
        }

        public override Goal<Agent>.States Process()
        {
            throw new NotImplementedException();
        }

        public override void Terminate()
        {
            throw new NotImplementedException();
        }

        //only for composite goals
        public void AddSubgoals(Goal<Agent> goal)
        {
            this.subgoalStack.Push(goal);
        }

        public Goal<Agent>.States ProcessSubgoals()
        {
            Goal<Agent>.States subgoalStatus = States.notSet;

            //all colmpleted or failed goals have to be removed from stack
            if (this.subgoalStack.Count > 0 && (this.subgoalStack.Peek().IsCompleted() || this.subgoalStack.Peek().HasFailed()))
            {
                this.subgoalStack.Peek().Terminate();
                this.subgoalStack.Pop();
            }

            if (this.subgoalStack.Count > 0)
            {
                subgoalStatus = this.subgoalStack.Peek().Process();
            }

            if (subgoalStatus == States.completed && this.subgoalStack.Count > 1)
            {
                return States.active;
            }

            return subgoalStatus;
        }

        public void RemoveAllSubgoals()
        {
            if (subgoalStack == null)
            {
                return;
            }
            for (int i = 0; i < this.subgoalStack.Count; i++)
            {
                this.subgoalStack.Peek().Terminate();
                this.subgoalStack.Pop();
            }

            this.subgoalStack.Clear();
        }
    }
}
