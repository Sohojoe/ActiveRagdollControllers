# ActiveRagdollControllers
Research into controllers for 2d and 3d Active Ragdolls (using MujocoUnity+ml_agents)


### Controller002
* **Type:** Continuous 2D
* **Actions:** Forward / Backwards
* **Input:** Unity Axis input (left/right or a/d or joystick)
* **Mujoco Model:** DeepMindHopper
* **Hypostheis**: Use ml-agent (player + Heuristic) and unity input to create a player contoller .
* **Outcome:** 
  * Works - makes a simple 2d proof of concept of 2d active ragdoll using RL.
  * I needed to use Discrete target velocities for stable training (-1,1,0) however it is continuous under player control. 
  * Training was sensitive to the number of steps between chaning the input (see counter logic below)
* **Notes:**
  * Controller002InputBrain
    * BrainType: Player = input is from player
    * BrainType: Heuristic = input is from Controller002InputBrain.cs
  * Controller002InputDecision.cs - 
    * Waits until counter is 0 then:
    * Takes random action and outputs AxisX as -1, 0, 1 (used as target velocity), then:
    * Set counter random 40-240
  * Controller002InputAgent.cs - 

### Controller001
* **Type:** Discrete 2D
* **Actions:** Forward / Backwards
* **Mujoco Model:** DeepMindHopper
* **Hypostheis**: It should be simple to train a backwards / forwards by giving the agent a +1 / 1 velocity target which feeds the reward function.
* **Outcome:** It worked - but was harder than expected to find good hyper-parms. Next, wrap this in a proper ml-agent controller and allow user input.
