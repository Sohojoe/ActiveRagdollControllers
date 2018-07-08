using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class Controller004InputDecision :  MonoBehaviour, Decision {
    
    public float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        if (memory.Count == 0) {
            memory.Add(0f);
            memory.Add(0f);
        }
        var action = (int) memory[1];

        memory[0]--;
        if (memory[0] <= 0){
            var rnd = UnityEngine.Random.value;
            bool repeateAction = false;
            if (action != 0 && rnd >.6f)
                repeateAction = true;
            if (!repeateAction)
            {
                rnd = UnityEngine.Random.value;
                if (rnd <= .4f)
                    action = 1; // right
                else if (rnd <= .8f)
                    action = 2; // left
                else
                    action = 0; // stand
                rnd = UnityEngine.Random.value;
                if (rnd >= .75)
                    action += 3; // add jump
            }
            // if (action == 0f)
            //     action = (rnd >= .9f) ? action : (rnd >= .45f) ? 1f : 2f;
            // else if (action == 1f)
            //     action = (rnd >= .4f) ? action : (rnd >= .2f) ? 2f : 0f;
            // else
            //     action = (rnd >= .4f) ? action : (rnd >= .2f) ? 1f : 0f;
            memory[0] = 40 + (int) (UnityEngine.Random.value * 200);
            memory[1] = (float)action;
        }

        return new float[1] { action };
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
