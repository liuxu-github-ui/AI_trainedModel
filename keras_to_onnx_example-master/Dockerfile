# Using official latest tensorflow image + GPU support and Jupyter
# NOTE: need NVIDIA Container Toolkit for CUDA https://github.com/NVIDIA/nvidia-docker
FROM tensorflow/tensorflow:latest-gpu-jupyter

# Installing Pillow for image manipulation 
RUN pip install pillow

# Installing Keras-ONNX support
# NOTE: for TF 2.x support, need to install from source
# https://github.com/onnx/keras-onnx
RUN pip install onnx onnxruntime
RUN pip install -U git+https://github.com/microsoft/onnxconverter-common
RUN pip install -U git+https://github.com/onnx/keras-onnx

# Same working directory as base TF image
WORKDIR /tf

# Setting env. vars. to workaround a Keras/TensorFlow ConvNet bug
# On RTX series of GPUs
# Error code: Could not create cudnn handle: CUDNN_STATUS_INTERNAL_ERROR
# Found at: https://github.com/tensorflow/tensorflow/issues/24496#issuecomment-629864800
# Goal: Set env. variable TF_FORCE_GPU_ALLOW_GROWTH = true
ARG TF_FORCE_GPU_ALLOW_GROWTH=true
ENV TF_FORCE_GPU_ALLOW_GROWTH=$TF_FORCE_GPU_ALLOW_GROWTH

# Image starts with running jupyter notebook in default directory
CMD ["jupyter", "notebook", "."]
