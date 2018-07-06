using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class Controller002InputDecision :  MonoBehaviour, Decision {
    
    public float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        var targetVelocityX = vectorObs[0];
        if (memory.Count == 0)
            memory.Add(0f);

        // // 101
        // memory[0]--;
        // if (memory[0] <= 0){
        //     memory[0] = 400;
        //     if (targetVelocityX == 0f)
        //         targetVelocityX = (UnityEngine.Random.value >= .5f) ? 1f : -1f;
        //     else
        //         targetVelocityX = targetVelocityX == 1f ? -1f : 1f;
        // }

        // 102
        memory[0]--;
        if (memory[0] <= 0){
            var rnd = UnityEngine.Random.value;
            if (targetVelocityX == 0f)
                targetVelocityX = (rnd >= .9f) ? targetVelocityX : (rnd >= .45f) ? 1f : -1f;
            else if (targetVelocityX > 0f)
                targetVelocityX = (rnd >= .4f) ? targetVelocityX : (rnd >= .2f) ? -1f : 0f;
            else
                targetVelocityX = (rnd >= .4f) ? targetVelocityX : (rnd >= .2f) ? 1f : 0f;
            memory[0] = 40 + (int) (UnityEngine.Random.value * 200);
        }

        return new float[1] { targetVelocityX };
    }
    public List<float> MakeMemory(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        // memory.Add(0);
        return memory; 
    }	
}
