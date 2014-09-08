using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model
{
    abstract class Goal<AgentType>
    {
        protected AgentType owner;
        public enum States { inactive, active, completed, failed, notSet }
        public enum Types { composite, atomic }
        protected States myStatus;
        protected Types myType;

        //must be implemented individually for every goal
        public abstract void Activate();
        public abstract States Process();
        public abstract void Terminate();

        public virtual bool IsActive() { return this.myStatus == States.active; }
        public virtual bool IsInactive() { return this.myStatus == States.inactive; }
        public virtual bool IsCompleted() { return this.myStatus == States.completed; }
        public virtual bool HasFailed() { return this.myStatus == States.failed; }

        public virtual Types GetGoalType() { return this.myType; }

        //only for composite goals, must be implemented here,
        //beacause of the composite design pattern
        public virtual void AddSubgoals(Goal<AgentType> goal)
        {
            throw new Exception("Can not add subgoals o atomic goals.");
        }

        //Constructor wich defines the current Status, Type and Owner
        public Goal(AgentType owner, Types type)
        {
            this.owner = owner;
            this.myType = type;
            this.myStatus = States.inactive;
        }

        public Goal() { }
    }
}
