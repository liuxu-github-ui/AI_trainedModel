# keras_to_onnx_example
Quick script for converting .h5 NN models from Keras/TensorFlow to ONNX format. Includes Dockerfile + shell script for environment setup

### Requirements:
* Keras (TensorFlow or Caffe backend) 
  * Provided *build_image.sh* and *Dockerfile* to build latest TensorFlow 2.x (GPU + Jupyter Notebook) + Keras Docker image with Pillow (image processing) and all required ONNX libraries
  * This is the most straight forward way to setting up the environment
* *keras2onnx*, *onnx* packages 
  * installation instructions found in the python file, and here: https://github.com/onnx/keras-onnx/tree/master/keras2onnx
* *onnxruntime* 
  * install using *pip*; only used to double check whether model exported correctly; not vital

#### Note: 
* __Recommend to use Docker for setting up the enviroment; just run *build_image.sh* script (Linux) to create image then use with `docker run`__

