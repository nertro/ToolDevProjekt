using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolDevProjekt.Model
{
    abstract class Evaluator
    {
        public abstract float CalculateDesirability(Agent agent);
        public abstract void SetGoal(Agent agent);
    }
}
