using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MujocoUnity;
using UnityEngine;
using MLAgents;

public class Controller004WalkerAgent : MujocoAgent {

    public float TargetVelocityX;
    public bool ShouldJump;
    public float CurrentVelocityX;
    public int StepsUntilNextTarget;

    Controller004InputAgent controllerAgent;

    public override void AgentReset()
    {
        base.AgentReset();

        if (controllerAgent == null)
            controllerAgent = GetComponent<Controller004InputAgent>();
        else
            controllerAgent.LowerEpisodeEnd(this);

        // set to true this to show monitor while training
        Monitor.SetActive(true);

        StepRewardFunction = StepRewardController101;
        // StepRewardFunction = StepRewardJump;
        TerminateFunction = TerminateOnNonFootHitTerrain;
        ObservationsFunction = ObservationsDefault;
        // OnTerminateRewardValue = -100f;

        // TerminateFunction = TerminateOnNonFootHitTerrain;
        // ObservationsFunction = ObservationsDefault;
        // OnEpisodeCompleteGetRewardFunction = GetRewardOnEpisodeComplete;


        BodyParts["pelvis"] = GetComponentsInChildren<Rigidbody>().FirstOrDefault(x=>x.name=="torso");
        BodyParts["right_foot"] = GetComponentsInChildren<Rigidbody>().FirstOrDefault(x=>x.name=="right_foot");
        BodyParts["left_foot"] = GetComponentsInChildren<Rigidbody>().FirstOrDefault(x=>x.name=="left_foot");
        base.SetupBodyParts();
        //ManageTargetStep(true);

    }


    public override void AgentOnDone()
    {
    }
    void ObservationsDefault()
    {
        TargetVelocityX = controllerAgent.AxisX;
        ShouldJump = controllerAgent.Jump;
        if (ShowMonitor) {
        }
        var pelvis = BodyParts["pelvis"];
        AddVectorObs(pelvis.velocity);
        AddVectorObs(pelvis.transform.forward); // gyroscope 
        AddVectorObs(pelvis.transform.up);
        
        AddVectorObs(SensorIsInTouch);
        JointRotations.ForEach(x=>AddVectorObs(x));
        AddVectorObs(JointVelocity);
        AddVectorObs(BodyParts["right_foot"].transform.position.y);
        AddVectorObs(BodyParts["left_foot"].transform.position.y);

        AddVectorObs(TargetVelocityX);
        AddVectorObs(CurrentVelocityX);
        AddVectorObs(ShouldJump);
    }

    float GetRewardOnEpisodeComplete()
    {
        return FocalPoint.transform.position.x;
    }

    float GetRewardJump()
    {
        var rightFoot = BodyParts["right_foot"];
        var leftFoot = BodyParts["left_foot"];
        var footHeight = Mathf.Min(rightFoot.transform.position.y, leftFoot.transform.position.y);
        var jumpReward = 0f;
        if (SensorIsInTouch[0] == 0){
            jumpReward += 1f;
            jumpReward += footHeight;
        }
        return jumpReward;
    }

    float StepRewardController101()
    {
        // float heightPenality = GetHeightPenality(0.5f);
        float uprightBonus = GetForwardBonus("pelvis");
        CurrentVelocityX = GetAverageVelocity("pelvis");
        float velocityReward = 1f - (Mathf.Abs(TargetVelocityX - CurrentVelocityX) * 1.2f);
        velocityReward = Mathf.Clamp(velocityReward, -1f, 1f);
        float effort = GetEffort();
        // var effortPenality = 1e-2f * (float)effort;
        var effortPenality = 3e-1f * (float)effort;
        //var jointsAtLimitPenality = GetJointsAtLimitPenality() * 4;
        var jumpBonus = ShouldJump ? GetRewardJump() : 0f;

        var reward = velocityReward
            +uprightBonus
            +jumpBonus
            // -heightPenality
            -effortPenality;
            //;//-jointsAtLimitPenality;
        if (ShowMonitor) {
            //var hist = new []{reward, velocityReward, jumpBonus, uprightBonus, -effortPenality, -jointsAtLimitPenality}.ToList();
            var hist = new []{reward, velocityReward, jumpBonus, uprightBonus, -effortPenality}.ToList();
            Monitor.Log("rewardHist", hist.ToArray());
        }
        controllerAgent.LowerStepReward(CurrentVelocityX);

        //ManageTargetStep();
        return reward;
    }
}
