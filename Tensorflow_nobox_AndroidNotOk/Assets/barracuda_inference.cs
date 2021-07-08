using UnityEngine;
using Unity.Barracuda;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class barracuda_inference : MonoBehaviour
{
    // Barracuda variables
    public NNModel modelSource;     // ONNX model (asset)
    public Model model;             // Runtime model wrapper (binary)
    private IWorker worker;         // Barracuda worker for inference
    public Sprite sprite1; // Images to be classified (assets)



    // Camera variables
    private static Texture2D boxOutlineTexture;
    private static GUIStyle labelStyle;

    private float cameraScale = 1f;
    private bool camAvailable;

    private WebCamTexture backCamera;
    public RawImage background;
    public AspectRatioFitter fitter;
    public Text uiText;
  


    // Start is called before the first frame update
    void Start()
    {

        model = ModelLoader.Load(modelSource);      // Load ONNX model as runtime binary model 
       // worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);  // Create Worker 

        // Camera start

        this.backCamera = new WebCamTexture();
        this.background.texture = this.backCamera;
        this.backCamera.Play();
        camAvailable = true;

    }


    // Update is called once per frame
    void Update()
    {
        Classify();

        //camera update

        if (!this.camAvailable)
        {
            return;
        }

        float ratio = (float)backCamera.width / (float)backCamera.height;
        fitter.aspectRatio = ratio;

        float scaleX = cameraScale;
        float scaleY = backCamera.videoVerticallyMirrored ? -cameraScale : cameraScale;
        background.rectTransform.localScale = new Vector3(scaleX, scaleY, 1f);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);

        if (orient != 0)
        {
            this.cameraScale = (float)Screen.width / Screen.height;
        }


    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(150, 150, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(150, 150, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, 150, 150), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
    }
    /**
     * Runs inference on two images
     */


    private void Classify()
    {
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        // convert WebCamTexture to Tensor
        var channels = 3;   // color; use 1 for grayscale, 4 for alpha


        var rawimage_px = TextureToTexture2D(background.texture).GetPixels(
           (int)TextureToTexture2D(background.texture).texelSize.x,
           (int)TextureToTexture2D(background.texture).texelSize.y,
           (int)TextureToTexture2D(background.texture).width,
           (int)TextureToTexture2D(background.texture).height);

        ///////-->change by LX
        Texture2D input = new Texture2D(150, 150);      // Creating Texture2D and setting pixels to dog image catvsdog is 150x150
        //Texture2D input = new Texture2D(300, 300);      // Creating Texture2D and setting pixels to dog image humanvshorse is 300x300
//////--->
        input.SetPixels(rawimage_px);
        input.Apply();

        // ==== Inference on a dog picture (should return 1) ====

        var in_tensor = new Tensor(input, channels);    // Create tensor from Texture2D (3 channel)
       // UnityEngine.Debug.Log("Got preprocessed tensor...");

        worker.Execute(in_tensor);                      // run inference on tensor
       // UnityEngine.Debug.Log("Model executed on dog photo...");

///////----> Change by LX
        // Explore inference detections
        //var out_tensor = worker.PeekOutput("dense_5");  // name of output layers dense_5 for catvsdog; 'predictions' for Xception NN
        var out_tensor = worker.PeekOutput("dense_1");  // name of output layers dense_1 for horsevshuman; 'predictions' for Xception NN
///////----->

        var max_val = Mathf.Max(out_tensor.ToReadOnlyArray());
        var arr = out_tensor.ToReadOnlyArray();
        var index = System.Array.IndexOf(arr, max_val);

        // confidence level of input image is human 
        print("max_val: " + max_val);

        if (max_val >= 0.90f)
        {
            uiText.text = "dog";

        }
        else if (max_val < 0.2f)
        {
            uiText.text = "cat";
            
        }
        else
        {
            uiText.text = "neither dog or cat";
        }

        worker.Dispose();
        in_tensor.Dispose();
        out_tensor.Dispose();

        //UnityEngine.Debug.Log("Output = " + out_tensor[0]);     // should be approx. 1 for dogs
        //UnityEngine.Debug.Log("Max prob = " + max_val);
        //UnityEngine.Debug.Log("Index of max = " + index);

        // ==== Inference on a cat picture (should return 0) ====

        //        input.SetPixels(cat_px);                        // Set Texture2D pixels to cat photo
        //        input.Apply();
        //        in_tensor = new Tensor(input, channels);        // Convert to tensor

        //        worker.Execute(in_tensor);                      // run inference on tensor
        //        UnityEngine.Debug.Log("Model executed on cat photo...");
        //////////------>Change by LX
        //        // Explore inference detections
        //        //out_tensor = worker.PeekOutput("dense_5");      // name of output layers dense5 catvsdog; 'predictions' for Xception NN
        //        out_tensor = worker.PeekOutput("dense_1");      // name of output layers dense1 horsevshuan; 'predictions' for Xception NN
        /////////------>
        //        max_val = Mathf.Max(out_tensor.ToReadOnlyArray());
        //        arr = out_tensor.ToReadOnlyArray();
        //        index = System.Array.IndexOf(arr, max_val);

        //        UnityEngine.Debug.Log("Output = " + out_tensor[0]);   // Should be approx. 0 for cats
        //        UnityEngine.Debug.Log("Max prob = " + max_val);
        //        UnityEngine.Debug.Log("Index of max = " + index);

        // Clean up (don't forget!)
        //worker.Dispose();
        //in_tensor.Dispose();
        //out_tensor.Dispose();     // not necessary if gathered by worker.PeekOutput()
    }
}
