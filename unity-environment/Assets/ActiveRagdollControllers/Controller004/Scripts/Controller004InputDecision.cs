using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class Controller004InputDecision :  MonoBehaviour, Decision {
    
    [Header("Use this to switch between Heuristic and Player")]
    public bool PlayerInput;

    // EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    [Header("Status")]

    public float X;
    public bool Jump;
    public int Action;

    public float[] Decide(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        if (PlayerInput)
            return DecidePlayer(vectorObs, visualObs, reward, done, memory);
        return DecideHeuristic(vectorObs, visualObs, reward, done, memory);
    }

    float[] DecidePlayer(
        List<float> vectorObs,
        List<Texture2D> visualObs,
        float reward,
        bool done,
        List<float> memory)
    {
        X = Input.GetAxis("Horizontal") * Time.deltaTime;
        Jump = Input.GetButton("Fire1");
        Action = 0;
        if (X > 0f)
            Action = 1;
        else if (X < 0f)
            Action = 2;
        if (Jump)
            Action += 3;

        return new float[1] { Action };
    }

    float[] DecideHeuristic(
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
        Action = (int) memory[1];

        memory[0]--;
        if (memory[0] <= 0){
            var rnd = UnityEngine.Random.value;
            bool repeateAction = false;
            if (Action != 0 && rnd >.6f)
                repeateAction = true;
            if (!repeateAction)
            {
                rnd = UnityEngine.Random.value;
                if (rnd <= .4f)
                    Action = 1; // right
                else if (rnd <= .8f)
                    Action = 2; // left
                else
                    Action = 0; // stand
                rnd = UnityEngine.Random.value;
                if (rnd >= .75)
                    Action += 3; // add jump
            }
            // if (Action == 0f)
            //     Action = (rnd >= .9f) ? action : (rnd >= .45f) ? 1f : 2f;
            // else if (Action == 1f)
            //     Action = (rnd >= .4f) ? action : (rnd >= .2f) ? 2f : 0f;
            // else
            //     Action = (rnd >= .4f) ? action : (rnd >= .2f) ? 1f : 0f;
            memory[0] = 40 + (int) (UnityEngine.Random.value * 200);
            memory[1] = (float)Action;
        }

        return new float[1] { Action };
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
