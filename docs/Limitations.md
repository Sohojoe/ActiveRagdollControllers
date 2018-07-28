# Limitations 

## Unity SDK
### Headless Mode
If you enable Headless mode, you will not be able to collect visual 
observations from your agents.

### Rendering Speed and Synchronization
Currently the speed of the game physics can only be increased to 100x 
real-time. The Academy also moves in time with FixedUpdate() rather than 
Update(), so game behavior implemented in Update() may be out of sync with the Agent decision making. See [Execution Order of Event Functions](https://docs.unity3d.com/Manual/ExecutionOrder.html) for more information.

## Python API

### Python version
As of version 0.3, we no longer support Python 2. 

### Tensorflow support
Currently Ml-Agents uses TensorFlow 1.4 due to the version of the TensorFlowSharp plugin we are using. 