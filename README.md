# ActiveRagdollControllers
Research into controllers for 2d and 3d Active Ragdolls (using MujocoUnity+ml_agents)


### Controll001
* **Type:** Discrete 2D
* **Actions:** Forward / Backwards
* **Mujoco Model:** DeepMindHopper
* **Hypostheis**: It should be simple to train a backwards / forwards by giving the agent a +1 / 1 velocity target which feeds the reward function.
* **Outcome:** It worked - but was harder than expected to find good hyper-parms. Next, wrap this in a proper ml-agent controller and allow user input.


