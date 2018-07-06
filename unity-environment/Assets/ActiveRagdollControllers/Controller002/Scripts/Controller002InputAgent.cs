using UnityEngine;
using MLAgents;

public class Controller002InputAgent : Agent {
    public float AxisX;

    public override void AgentReset()
    {
        AxisX = 0f;
    }
    public override void CollectObservations()
    {
        AddVectorObs(AxisX);
    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AxisX = vectorAction[0];
    }
}