using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolDevProjekt.Model
{
    class Brain :GoalComposite
    {
        private List<Evaluator> evaluators;

        public override void Activate()
        {
            this.RemoveAllSubgoals();
            this.myStatus = States.active;
            this.subgoalStack = new Stack<Goal<Agent>>();
        }

        public override Goal<Agent>.States Process()
        {
            if (this.myStatus == States.inactive)
            {
                this.Activate();
            }
            if (this.subgoalStack.Count <= 0)
            {
                this.Consider();
            }
            this.myStatus = ProcessSubgoals();
            return this.myStatus;
        }

        public override void Terminate()
        {
            this.myStatus = States.inactive;
        }

        public void AddGoal_DestroyOther()
        {
            this.AddSubgoals(new Goal_DestroyOther(this.owner, Types.composite));
        }

        public void Consider()
        {
            Evaluator mostDesirable = evaluators[0];
            float bestValue = 0;
            foreach (var evaluator in evaluators)
            {
                if (evaluator.CalculateDesirability(this.owner) > bestValue)
                {
                    bestValue = evaluator.CalculateDesirability(this.owner);
                    mostDesirable = evaluator;
                }
            }

            mostDesirable.SetGoal(this.owner);
        }

        public Brain(Agent owner, Types type)
        {
            this.owner = owner;
            this.myType = type;
            this.myStatus = States.inactive;
            evaluators = new List<Evaluator>();
            evaluators.Add(new Evaluator_AttackOther());
        }
    }
}
