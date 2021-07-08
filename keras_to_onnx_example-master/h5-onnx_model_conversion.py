#!/usr/bin/env python
# coding: utf-8

# Importing the pre-trained model from Keras Applications library
from tensorflow.keras.models import load_model
import sys

#if (len(sys.argv) <= 1):
    #print("too few arguments! Need filename of .h5 model")
    #exit()

model = load_model('horse_human.h5')

# Importing ONNX conversion tools and converting 
# Instructions: https://medium.com/analytics-vidhya/how-to-convert-your-keras-model-to-onnx-8d8b092c4e4f
# Tools (install first): https://github.com/onnx/keras-onnx

import onnx
import keras2onnx

onnx_model = keras2onnx.convert_keras(model, model.name)
onnx.save_model(onnx_model,"hose_human.onnx")  # saved in current working directory as .onnx
#onnx.save_model(onnx_model, sys.argv[1][:-3] + ".onnx")  # saved in current working directory as .onnx

# Testing if converted properly by importing back
# NOTE: need to install onnxruntime package
#print("Reimporting to check whether conversion succesful (need onnxruntime package installed)...")

#try:
    #import onnxruntime
#except ImportError as e:
    #print("no onnxruntime module found! This is used just to check whether exported correctly, and is not required")
    #exit()
    
#new_model = onnxruntime.InferenceSession(sys.argv[1][:-3] + ".onnx")
#print('Successfully exported!')

