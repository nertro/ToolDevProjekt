using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ToolDevProjekt.Model
{
    class Evaluator_AttackOther: Evaluator
    {
        public override float CalculateDesirability(Agent agent)
        {
            float tweak = 1.15f;
            Random rand = new Random();
            float combatReady = rand.Next(0, 1);

            float distance = agent.GetDistanceToPlayer() / 100;
            float desire =  agent.Type.Aggression * (tweak * combatReady/ distance);
            if (desire > 1)
            {
                desire = 1;
            }

            return desire;
        }

        public override void SetGoal(Agent agent)
        {
            agent.MyBrain.AddGoal_DestroyOther();
        }
    }
}
