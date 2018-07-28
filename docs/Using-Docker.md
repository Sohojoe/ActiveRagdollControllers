# Using Docker For ML-Agents

We currently offer a solution for Windows and Mac users who would like to do training or inference using Docker. This option may be appealing to those who would like to avoid installing Python and TensorFlow themselves. The current setup forces both TensorFlow and Unity to _only_ rely on the CPU for computations. Consequently, our Docker simulation does not use a GPU and uses [`Xvfb`](https://en.wikipedia.org/wiki/Xvfb) to do visual rendering. `Xvfb` is a utility that enables `ML-Agents` (or any other application) to do rendering virtually i.e. it does not assume that the machine running `ML-Agents` has a GPU or a display attached to it. This means that rich environments which involve agents using camera-based visual observations might be slower.


## Requirements
- Unity _Linux Build Support_ Component
- [Docker](https://www.docker.com)

## Setup

- [Download](https://unity3d.com/get-unity/download) the Unity Installer and
add the _Linux Build Support_ Component

- [Download](https://www.docker.com/community-edition#/download) and
install Docker if you don't have it setup on your machine.

- Since Docker runs a container in an environment that is isolated from the host machine, a mounted directory in your host machine is used to share data, e.g. the Unity executable, curriculum files and TensorFlow graph. For convenience, we created an empty `unity-volume` directory at the root of the repository for this purpose, but feel free to use any other directory. The remainder of this guide assumes that the `unity-volume` directory is the one used.

## Usage

Using Docker for ML-Agents involves three steps: building the Unity environment with specific flags, building a Docker container and, finally, running the container. If you are not familiar with building a Unity environment for ML-Agents, please read through our [Getting Started with the 3D Balance Ball Example](Getting-Started-with-Balance-Ball.md) guide first.

### Build the Environment (Optional)
_If you want to used the Editor to perform training, you can skip this step._

Since Docker typically runs a container sharing a (linux) kernel with the host machine, the 
Unity environment **has** to be built for the **linux platform**. When building a Unity environment, please select the following options from the the Build Settings window:
- Set the _Target Platform_ to `Linux`
- Set the _Architecture_ to `x86_64`
- If the environment does not contain visual observations, you can select the `headless` option here.

Then click `Build`, pick an environment name (e.g. `3DBall`) and set the output directory to `unity-volume`. After building, ensure that the file `<environment-name>.x86_64` and subdirectory `<environment-name>_Data/` are created under `unity-volume`.

![Build Settings For Docker](images/docker_build_settings.png)

### Build the Docker Container

First, make sure the Docker engine is running on your machine. Then build the Docker container by calling the following command at the top-level of the repository:

```
docker build -t <image-name> .
``` 
Replace `<image-name>` with a name for the Docker image, e.g. `balance.ball.v0.1`.

**Note** if you modify hyperparameters in `trainer_config.yaml` you will have to build a new Docker Container before running.

### Run the Docker Container

Run the Docker container by calling the following command at the top-level of the repository:

```
docker run --name <container-name> \
           --mount type=bind,source="$(pwd)"/unity-volume,target=/unity-volume \
           -p 5005:5005 \
           <image-name>:latest <environment-name> \
           --docker-target-name=unity-volume \
           --train \
           --run-id=<run-id>
```

Notes on argument values:
- `<container-name>` is used to identify the container (in case you want to interrupt and terminate it). This is optional and Docker will generate a random name if this is not set. _Note that this must be unique for every run of a Docker image._
- `<image-name>` references the image name used when building the container.
- `<environemnt-name>` __(Optional)__: If you are training with a linux executable, this is the name of the executable. If you are training in the Editor, do not pass a `<environemnt-name>` argument and press the :arrow_forward: button in Unity when the message _"Start training by pressing the Play button in the Unity Editor"_ is displayed on the screen.
- `source`: Reference to the path in your host OS where you will store the Unity executable. 
- `target`: Tells Docker to mount the `source` path as a disk with this name. 
- `docker-target-name`: Tells the ML-Agents Python package what the name of the disk where it can read the Unity executable and store the graph. **This should therefore be identical to `target`.**
- `train`, `run-id`: ML-Agents arguments passed to `learn.py`. `train` trains the algorithm, `run-id` is used to tag each experiment with a unique identifier. 

To train with a `3DBall` environment executable, the command would be:

```
docker run --name 3DBallContainer.first.trial \
           --mount type=bind,source="$(pwd)"/unity-volume,target=/unity-volume \
           -p 5005:5005 \
           balance.ball.v0.1:latest 3DBall \
           --docker-target-name=unity-volume \
           --train \
           --run-id=3dball_first_trial
```

For more detail on Docker mounts, check out [these](https://docs.docker.com/storage/bind-mounts/) docs from Docker.


### Stopping Container and Saving State

If you are satisfied with the training progress, you can stop the Docker container while saving state by either using `Ctrl+C` or `⌘+C` (Mac) or by using the following command:

```
docker kill --signal=SIGINT <container-name>
```

`<container-name>` is the name of the container specified in the earlier `docker run` command. If you didn't specify one, you can find the randomly generated identifier by running `docker container ls`.
