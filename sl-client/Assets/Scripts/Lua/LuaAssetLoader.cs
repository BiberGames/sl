using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Siccity.GLTFUtility;
using UnityEngine.Networking;
using System.Collections.Generic;

public class LuaAssetLoader : MonoBehaviour
{
    public void DelayedLauncher(string FuncName, float WaitTime)
    {
        StartCoroutine(DelayedLauncherA(FuncName, WaitTime));
    }
    public void LoadImage(string ImageViewer, string ImagePath)
    {
        RawImage _RawImage = GameObject.Find(ImageViewer).GetComponent<RawImage>();
        StartCoroutine(GetTexture(_RawImage, ImagePath));
    }

    public void LoadSprite(string SpritePath, int SpriteID, float pixelsPerUnit, bool Filtered)
    {
        StartCoroutine(GetSprite(SpritePath, SpriteID, pixelsPerUnit, Filtered));
    }

    public void SetSprite(string TSpriteViewer, int TSpriteID)
    {
        Debug.Log("Setting sprite...");
        if(GameObject.Find("LuaRunner").GetComponent<LuaRunner>().SpriteCash[TSpriteID] == null)
        {
            Debug.Log("Nothing found / loaded...");
        }
        else
        {
            GameObject.Find(TSpriteViewer).GetComponent<SpriteRenderer>().sprite = GameObject.Find("LuaRunner").GetComponent<LuaRunner>().SpriteCash[TSpriteID];
        }
    }

    public void LoadAudio(string AudioSourceName, string AudioPath)
    {
        AudioSource _AudioSource = GameObject.Find(AudioSourceName).GetComponent<AudioSource>();
        StartCoroutine(LoadAudio(_AudioSource, AudioPath));
    }

    private IEnumerator GetTexture(RawImage input1, string Path)
    {
        GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FFFF00>Lua > Loading texture: " + Path + "<color=#FFFFFF>");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Path);
        yield return www.SendWebRequest();

        Texture myTexture = DownloadHandlerTexture.GetContent(www);
        input1.texture = myTexture;
    }

    private IEnumerator GetSprite(string Path, int SpriteID, float pixelsPerUnit, bool Filtered)
    {
        GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FFFF00>Lua > Loading texture: " + Path + "<color=#FFFFFF>");

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Path);
        yield return www.SendWebRequest();

        Texture myTexture = DownloadHandlerTexture.GetContent(www);
        Sprite mySprite = Sprite.Create((Texture2D)myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        if(Filtered)
        {
            myTexture.filterMode = FilterMode.Point;
        }
        GameObject.Find("LuaRunner").GetComponent<LuaRunner>().SpriteCash[SpriteID] = mySprite;
        Debug.Log("'Loaded' sprite!");
        if(www.isDone)
        {
            Debug.Log("Done.");
        }
    }

    public void LoadModel(string Path, string AddonName)
    {
        string filepath = @Application.streamingAssetsPath + "/Addons/" + AddonName + Path + ".gltf";
        Importer.ImportGLTFAsync(filepath, new ImportSettings(), null);
    }

    private IEnumerator LoadAudio(AudioSource AudioPlayer, string AudioPath)
    {
        GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FFFF00>Lua > Loading audio: " + AudioPath + "<color=#FFFFFF>");
        WWW url = new WWW(AudioPath);
        yield return url;

        AudioPlayer.clip = url.GetAudioClip(false, true);
        AudioPlayer.Play();
    }

    private IEnumerator DelayedLauncherA(string FuncName, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        GameObject.Find("LuaRunner").GetComponent<LuaRunner>().CallFuncFromConsole(FuncName);
    }
}
