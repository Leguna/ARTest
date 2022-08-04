using System.Collections;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public void TakeAShot()
    {
        StartCoroutine(nameof(TakeScreenshotAndShare));
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();
        var ss = SaveScreenToTemp(out var filePath);
        CaptureScreen(ss, out var filePathGallery);
        ShareCapture(filePathGallery);
    }

    private void ShareCapture(string filePath)
    {
        new NativeShare().AddFile(filePath)
            .SetSubject("Hello").SetText("Aplikasi gabut").SetUrl("https://github.com/leguna/")
            .SetCallback((result, shareTarget) =>
                Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
    }

    private string CaptureScreen(Texture2D ss, out string filepath)
    {
        string imagePath = "";
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "GalleryTest", "Image.png",
            (success, path) =>
            {
                Debug.Log("Media save result: " + success + " " + path);
                imagePath = path;
            });
        Debug.Log("Permission result: " + permission);
        filepath = imagePath;
        return imagePath;
    }

    private static Texture2D SaveScreenToTemp(out string filePath)
    {
        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());
        Destroy(ss);

        return ss;
    }
}