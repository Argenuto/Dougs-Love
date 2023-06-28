/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this Code Monkey project
    I hope you find it useful in your own projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.IO;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Malee;
public enum ScreenshotMode {single,layered }

[System.Serializable]
public class LanguageShare {
    public SystemLanguage language;
    public string text, title;
}
[System.Serializable]
public class CameraList : ReorderableArray<Camera>
{
}
[RequireComponent(typeof(Camera))]
[Serializable]
public class ScreenshotHandler : MonoBehaviour {
    public static ScreenshotHandler instance;
    public bool SaveInTheFolder;
    public ScreenshotMode mode = ScreenshotMode.single;
    private static Camera myCamera;
    [Reorderable][HideInInspector]
    public CameraList cameraLayers;
    private bool takeScreenshotOnNextFrame;
    private string filename;
    public LanguageShare[] shareDiags;
    public string shareTitle;
    public string shareText;
    private bool takeScreenshotOnNextFrameWithExtraCameras;
    
    private void Awake() {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender() {
        if (takeScreenshotOnNextFrame) {
            takeScreenshotOnNextFrame = false;
            switch (mode)
            {
                case ScreenshotMode.single:
                    Debug.Log("modo simple");
                    SingleTakeShot();
                    break;
                case ScreenshotMode.layered:
                    Debug.Log("modo capas");
                    LayeredTakeShot();
                    break;
                default:
                    break;
            }
        }
       
    }

    private void LayeredTakeShot()
    {
        RenderTexture renderTexture = cameraLayers[cameraLayers.Length - 1].targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);
        Color[] colBase = renderResult.GetPixels(), colLayer;
        if (cameraLayers.Length > 1)
            for (int j = cameraLayers.Length - 2; j < 0; j--)
            {
                RenderTexture temp_renderTexture = cameraLayers[j].targetTexture;
                Texture2D temp_renderResult = new Texture2D(temp_renderTexture.width, temp_renderTexture.height, TextureFormat.ARGB32, false);
                Rect temp_rect = new Rect(0, 0, temp_renderTexture.width, temp_renderTexture.height);
                temp_renderResult.ReadPixels(rect, 0, 0);
                colLayer = temp_renderResult.GetPixels();
                for (var i = 0; i < colBase.Length; ++i)
                {
                    float rOut = (colLayer[i].r * colLayer[i].a) + (colBase[i].r * (1 - colLayer[i].a));
                    float gOut = (colLayer[i].g * colLayer[i].a) + (colBase[i].g * (1 - colLayer[i].a));
                    float bOut = (colLayer[i].b * colLayer[i].a) + (colBase[i].b * (1 - colLayer[i].a));
                    float aOut = colLayer[i].a + (colBase[i].a * (1 - colLayer[i].a));

                    colBase[i] = new Color(rOut, gOut, bOut, aOut);
                }
                renderResult.SetPixels(colBase);
                renderResult.Apply();
                RenderTexture.ReleaseTemporary(temp_renderTexture);
                cameraLayers[j].targetTexture = null;
            }
        byte[] byteArray = renderResult.EncodeToPNG();
#if UNITY_EDITOR

        if (SaveInTheFolder)
        {
            filename = Path.Combine(Application.dataPath, "Screenshots", "pic made" + System.DateTime.Now.ToFileTime() + ".png");
            if (!Directory.Exists(Path.Combine(Application.dataPath, "Screenshots")))
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Screenshots"));
            File.WriteAllBytes(filename, byteArray);
        }

#else
            filename = Path.Combine(Application.temporaryCachePath, "shared_img.png");
            File.WriteAllBytes(filename, byteArray);

#endif
        RenderTexture.ReleaseTemporary(renderTexture);
        cameraLayers[cameraLayers.Length - 1].targetTexture = null;
    }

    private void SingleTakeShot()
    {
        RenderTexture renderTexture = myCamera.targetTexture;
        Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
        renderResult.ReadPixels(rect, 0, 0);
        byte[] byteArray = renderResult.EncodeToPNG();
#if UNITY_EDITOR
        if (SaveInTheFolder)
        {
            filename = Path.Combine(Application.dataPath, "Screenshots", "pic made" + System.DateTime.Now.ToFileTime() + ".png");
            if (!Directory.Exists(Path.Combine(Application.dataPath, "Screenshots")))
                Directory.CreateDirectory(Path.Combine(Application.dataPath, "Screenshots"));
            File.WriteAllBytes(filename, byteArray);
        }
#else
            filename = Path.Combine(Application.temporaryCachePath, "shared_img.png");
#endif
        System.IO.File.WriteAllBytes(filename, byteArray);
        RenderTexture.ReleaseTemporary(renderTexture);
        myCamera.targetTexture = null;
    }

    private void TakeScreenshot(int width, int height) {
        switch (mode)
        {
            case ScreenshotMode.single:
                myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
                break;
            case ScreenshotMode.layered:
                cameraLayers[cameraLayers.Length-1].targetTexture = RenderTexture.GetTemporary(width, height, 16);
                break;
            default:
                break;
        }
        takeScreenshotOnNextFrame = true;
    }

    public static void TakeScreenshot_Static(int width, int height) {
        instance.TakeScreenshot(width, height);
    }

    public void TakeSSFromUnity() {
        ScreenCapture.CaptureScreenshot("unnivel");
    }
    public static void TakeSS() {

        instance.TakeScreenshot(myCamera.pixelWidth,myCamera.pixelHeight);
    }
    public void TakeScreenshotNoStatic() {
        Debug.Log("tomado");
        TakeScreenshot(myCamera.pixelWidth, myCamera.pixelHeight);
    }
    public async static void TakeScreenshotDelayed(float secs) {
        Debug.Log("foto tomada");
        await Task.Delay((int)(secs*1000));
        instance.TakeScreenshot(myCamera.pixelWidth, myCamera.pixelHeight);
    }
    public void Share() {

        foreach (LanguageShare ls in shareDiags)
            if (ls.language == Application.systemLanguage)
            {
                shareTitle = ls.title;
                shareText = ls.text;
                break;
            }
        new NativeShare().AddFile(filename).SetTitle(shareTitle).SetText(shareText).Share();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(ScreenshotHandler))]
[CanEditMultipleObjects]
public class SSHandlerEditor : Editor
{
    private ScreenshotHandler my_SSH;

    private void OnEnable()
    {
        my_SSH = target as ScreenshotHandler;
        //camList = new ReorderableList(my_SSH.cameraLayers, typeof(Camera));

    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        if (my_SSH.mode == ScreenshotMode.layered)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("cameraLayers"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
