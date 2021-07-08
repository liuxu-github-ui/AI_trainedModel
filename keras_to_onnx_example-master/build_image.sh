# !/usr/bin/env bash

echo "NOTES:" 
echo "- If getting errors, try running as sudo: sudo bash build_image.sh"
echo "- This image has set TF_FORCE_GPU_ALLOW_GROWTH=true to workaround Keras/TF ConvNet bug with CUDA"
echo "Building tensorflow:latest-gpu-jupyter-onnx image (includes Pillow for image manipulation, h5-to-ONNX conversion tools)..."

docker build --tag tensorflow:latest-gpu-jupyter-onnx .
