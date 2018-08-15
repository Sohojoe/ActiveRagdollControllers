# ActiveRagdollControllers
Research into controllers for 2d and 3d Active Ragdolls (using MujocoUnity+ml_agents)

----

#### Contributors
* Joe Booth ([SohoJoe](https://github.com/Sohojoe))

----
#### Download builds (Mac, Windows): [see Releases](https://github.com/Sohojoe/ActiveRagdollControllers/releases)
--- 
### Controller004
![Controller004](images/Controller004.13-10m.gif)
* **Type:** Discrete 2D
* **Build (MacOS)** TODO [v0.004](https://github.com/Sohojoe/ActiveRagdollControllers/releases/tag/v0.003) **Playable**
* **Actions:** No-op, Forward, Backwards, Jump, Jump+Forward, Jump+Backwards
* **Controls:** Left arrow, Right arrow, Spacebar
* **Mujoco Model:** DeepMindHopper
* **Hypostheis**: Use discreate + random trainer to create human controller.
* **Outcome:** 
  * **SUCCESS** - contriol feels responsive
  * ... Has emerging functionality - i.e. tap left for small step, swap direction in air 


### Controller003
![Controller003](images/Controller003.gif)
* **Type:** Continuous 2D
* **Build (MacOS)** [v0.003](https://github.com/Sohojoe/ActiveRagdollControllers/releases/tag/v0.003)
* **Actions:** Forward / Backwards
* **Mujoco Model:** DeepMindHopper
* **Hypostheis**: Use an adversarial hierarchical trained agent as the controller which gets the inverse reward of the locomation agent on a slower time step. The idea is that it will push the locomoation agent to focus on its weakest areas. 
* **Outcome:** 
  * **FAIL** - training is too heavily influenced by the number of steps the controller agent takes between decisions; 
  * ... it maybe better to train a seperate agent on hyper-parms (i.e. meta learning) 
  * ... having read more about these approaches (MAML, RL2, etc) it would be better to move to a Discreate conrtroller as ml-agents LSTM does not work well with Continuous actions.


### Controller002
![Controller002](images/Controller002.gif)
* **Type:** Continuous 2D
* **Build (MacOS, Windows)** [v0.002](https://github.com/Sohojoe/ActiveRagdollControllers/releases/tag/v0.002)
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
![Controller001](images/Controller001.gif)
* **Type:** Discrete 2D
* **Build (MacOS)** [v0.001](https://github.com/Sohojoe/ActiveRagdollControllers/releases/tag/v0.001)
* **Actions:** Forward / Backwards
* **Mujoco Model:** DeepMindHopper
* **Hypostheis**: It should be simple to train a backwards / forwards by giving the agent a +1 / 1 velocity target which feeds the reward function.
* **Outcome:** It worked - but was harder than expected to find good hyper-parms. Next, wrap this in a proper ml-agent controller and allow user input.
