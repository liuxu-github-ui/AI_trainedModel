using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;


public enum Mode
{
    Detect,
    Classify,
}
public class modified_liuxu : MonoBehaviour
{
    public Mode mode;
    private bool isWorking = false;
    private static Texture2D boxOutlineTexture;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (this.mode == Mode.Detect)
    //    {
    //        TFDetect();
    //    }
    //}

    //private void TFDetect()
    //{
    //    if (this.isWorking)
    //    {
    //        return;
    //    }

    //    this.isWorking = true;
    //    StartCoroutine(ProcessImage(Detector.IMAGE_SIZE, result =>
    //    {
    //        StartCoroutine(this.detector.Detect(result, boxes =>
    //        {
    //            this.boxOutlines = boxes;
    //            Resources.UnloadUnusedAssets();
    //            this.isWorking = false;
    //        }));
    //    }));
    //}

    //private IEnumerator ProcessImage(int inputSize, System.Action<Color32[]> callback)
    //{
    //    yield return StartCoroutine(TextureTools.CropSquare(backCamera,
    //        TextureTools.RectOptions.Center, snap =>
    //        {
    //            var scaled = Scale(snap, inputSize);
    //            var rotated = Rotate(scaled.GetPixels32(), scaled.width, scaled.height);
    //            callback(rotated);
    //        }));
    //}

    

}
