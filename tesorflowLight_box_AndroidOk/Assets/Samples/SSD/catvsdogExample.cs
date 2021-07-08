using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TensorFlowLite;
using System;

public class catvsdogExample : MonoBehaviour
{
    
    [SerializeField, FilePopup("*.tflite")] string fileName = "catvsdog.tflite";
    [SerializeField] RawImage cameraView = null;
    [SerializeField] Text framePrefab = null;
    [SerializeField, Range(0f, 1f)] float scoreThreshold = 0.5f;
    [SerializeField] TextAsset labelMap = null;

    WebCamTexture webcamTexture;
    CATVSDOG catvsdog;

    Text[] frames;

    public string[] labels;

    void Start()
    {

        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        catvsdog = new CATVSDOG(path);

        // Init camera
        string cameraName = WebCamUtil.FindName();
        webcamTexture = new WebCamTexture(cameraName, 1280, 720, 30);
        cameraView.texture = webcamTexture;
        webcamTexture.Play();
        Debug.Log($"Starting camera: {cameraName}");

        // Init frames
        frames = new Text[2];
        var parent = cameraView.transform;
        for (int i = 0; i < frames.Length; i++)
        {
            frames[i] = Instantiate(framePrefab, Vector3.zero, Quaternion.identity, parent);
        }

        // Labels
        labels = labelMap.text.Split('\n');


    }

    void OnDestroy()
    {
        webcamTexture?.Stop();
        catvsdog?.Dispose();
    }

    void Update()
    {
        catvsdog.Invoke(webcamTexture);

        var results = catvsdog.GetResults();

        var size = cameraView.rectTransform.rect.size;
        for (int i = 0; i < 10; i++)
        {
            SetFrame(frames[i], results[i], size);
        }

        cameraView.material = catvsdog.transformMat;
        // cameraView.texture = ssd.inputTex;
    }

    void SetFrame(Text frame, CATVSDOG.Result result, Vector2 size)
    {
        if (result.score < scoreThreshold)
        {
            frame.gameObject.SetActive(false);
            return;
        }
        else
        {
            frame.gameObject.SetActive(true);
        }

        frame.text = $"{GetLabelName(result.classID)} : {(int)(result.score * 100)}%";
        var rt = frame.transform as RectTransform;
        rt.anchoredPosition = result.rect.position * size - size * 0.5f;
        rt.sizeDelta = result.rect.size * size;
    }

    string GetLabelName(int id)
    {
        if (id < 0 || id >= labels.Length - 1)
        {
            return "?";
        }
        return labels[id + 1];
    }

}
