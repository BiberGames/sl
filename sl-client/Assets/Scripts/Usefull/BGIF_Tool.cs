//using SFB;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class BGIF
{
	public string title;
	public int width;
	public int height;
	public string[] data;
}

public class BGIF_Tool : MonoBehaviour
{
	public Texture2D LoadBGIF(string BGIFPath)
	{

        string rawData = File.ReadAllText(BGIFPath, Encoding.UTF8);

		BGIF currentBGIF = new BGIF();
		currentBGIF = JsonUtility.FromJson<BGIF>(rawData);

		Texture2D currentTexture = new Texture2D(currentBGIF.width, currentBGIF.height, TextureFormat.RGBA32, false);
		currentTexture.filterMode = FilterMode.Point;

		byte[] imageBytes = Convert.FromBase64String(currentBGIF.data[0]);
		currentTexture.LoadImage(imageBytes);

		currentTexture.Apply();

		return currentTexture;
	}
/*
	public void loadPNG()
	{
		var extensions = new [] {
            new ExtensionFilter("Image Files", "png", "jpg", "tga"),
            new ExtensionFilter("All Files", "*" ),
        };
		string[] PNGPath = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
		StartCoroutine(GetTexture(previewWindow, PNGPath[0]));
	}
*/
	public void ConvertToBGIF(string InputFilePath, string OutputFilePath)
	{
		Texture2D TempTexture = new Texture2D(0,0);
		StartCoroutine(GetTexture((Texture)TempTexture, InputFilePath));

		byte[] imageData = TempTexture.EncodeToPNG();
		
		string encodedImage = Convert.ToBase64String(imageData);

		BGIF tempBGIF = new BGIF();
		tempBGIF.title = "BGIFIMAGE";
		tempBGIF.data = new string[1];
		tempBGIF.data[0] = encodedImage;

		File.WriteAllText(OutputFilePath, JsonUtility.ToJson(tempBGIF), Encoding.UTF8);
	}

    IEnumerator GetTexture(Texture texture, string path)
    {
        // Start a download of the given URL
        using (WWW www = new WWW("file:/" + path))
        {
            // Wait for download to complete
            yield return www;

            // assign texture
			Texture2D imported = www.texture;
			imported.filterMode = FilterMode.Point;
            texture = imported;
        }
    }
}
