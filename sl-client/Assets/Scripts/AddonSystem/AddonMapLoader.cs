using Dummiesman;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AddonMapLoader : MonoBehaviour
{
    private bool Is3D = false;
    private bool PlayerSpawned = false;
    public RawImage LoadingScreenImage;
    public GameObject Player2D;
    public GameObject Player3D;
    public GameObject LoadedMap;

    void Start()
    {
        if (PlayerPrefs.GetInt("AddonMode")==1) Is3D = true; else Is3D = false;
        LoadingScreen();
        SetupScene();
    }

    void Uodate()
    {
        if(Is3D && GameObject.Find("info_player_start") &&  !PlayerSpawned)
        {
            AddColision();
            Player3D.transform.position = GameObject.Find("info_player_spawn").transform.position;
            PlayerSpawned = true;
        }
    }

    void AddColision()
    {
        GameObject.Find("Console").GetComponent<Console>().AddLine("\nAdding colliders...");
        if(GameObject.Find("Root") != null)
        {
            Transform transform = LoadedMap.transform;
            foreach (Transform child in transform)
            {
                if(child.gameObject.GetComponent<MeshCollider>() == null)
                {
                    child.gameObject.AddComponent<MeshCollider>();
                }
            }
        }
    }

    void LoadingScreen()
    {
        StartCoroutine(LoadLoadingScreenImage(LoadingScreenImage, @Application.streamingAssetsPath + "/Addons/" + PlayerPrefs.GetString("AddonToLoad") + "/LoadingBackground.png"));
    }

    void SetupScene()
    {
        if(Is3D)
        {
            Player2D.SetActive(false);
            Player3D.SetActive(true);
            Debug.Log(@Application.streamingAssetsPath + "/Addons/" + PlayerPrefs.GetString("AddonToLoad") + "/Maps/"+ PlayerPrefs.GetString("MapToLoad") + ".obj");
            LoadedMap = new OBJLoader().Load(@Application.streamingAssetsPath + "/Addons/" + PlayerPrefs.GetString("AddonToLoad") + "/Maps/"+ PlayerPrefs.GetString("MapToLoad") + ".obj");
        }
        else
        {
            Player2D.SetActive(true);
            Player3D.SetActive(false);
        }
    }

    private IEnumerator LoadLoadingScreenImage(RawImage input1, string Path)
    {
        string fullpath = "file:///" + Path;
        using (WWW www = new WWW(fullpath))
        {
            yield return www;
            input1.texture = www.texture;
        }
    }
}
