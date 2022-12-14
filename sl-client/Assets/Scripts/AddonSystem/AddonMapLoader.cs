using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Siccity.GLTFUtility;

public class AddonMapLoader : MonoBehaviour
{
    private bool Is3D = false;
    private bool PlayerSpawned = false;
    public RawImage LoadingScreenImage;
    public GameObject LoadingScreenUI;
    public GameObject Player2D;
    public GameObject Player3D;
    public GameObject LoadedMap;
    string MapName;
    [Header("Debug materials")]
    public Material Wireframe;
    [Header("Map entities")]
    public GameObject env_test;
    public GameObject env_ambient_light;
    public GameObject env_fire;
    public GameObject env_dust;

    void Start()
    {
        MapName = PlayerPrefs.GetString("MapToLoad");
        if (PlayerPrefs.GetInt("AddonMode")==1) Is3D = true; else Is3D = false;
        LoadingScreen();
        SetupScene();

        Color customColor = new Color(1f, 0f, 0f, 0f);
        Wireframe.SetColor("_WireColor", customColor);
    }

    void Update()
    {
        if(Is3D == true && GameObject.Find("info_player_start") && PlayerSpawned == false)
        {
            PlayerSpawned = true;
        }
    }

    void SetupMapEntities(GameObject MapObject)
    {
        string eriorjg = MapObject.name;
        string[] MapObjectName = eriorjg.Split('.');

        if(MapObjectName[0] == "info_player_start")
        {
            Transform Player3DSpawn = MapObject.transform;
            Player3D.transform.position = Player3DSpawn.position;
            Player3D.transform.eulerAngles = new Vector3(0, Player3DSpawn.eulerAngles.y, 0);

            Destroy(Player3DSpawn.gameObject);
            MapObject.SetActive(false);
        }
        else if(MapObjectName[0] == "env_test")
        {
            // a test point entity
            Instantiate(env_test, MapObject.transform.position, MapObject.transform.localRotation);

            MapObject.SetActive(false);
        }
        else if(MapObjectName[0] == "env_ambient_light")
        {
            // only one sun per map so don't need to make a new one...
            // need to add 90?? on the x axis to get the same angle as in blender...
            env_ambient_light.transform.position = MapObject.transform.position;
            env_ambient_light.transform.eulerAngles = new Vector3(MapObject.transform.eulerAngles.x + 90f, MapObject.transform.eulerAngles.y, MapObject.transform.eulerAngles.z);

            MapObject.SetActive(false);
        }
        else if(MapObjectName[0] == "env_cubemap")
        {
            GameObject env_cubemap = new GameObject("env_cubemap");
            env_cubemap.transform.position = MapObject.transform.position;
            env_cubemap.AddComponent<ReflectionProbe>();
            env_cubemap.GetComponent<ReflectionProbe>().mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
            env_cubemap.GetComponent<ReflectionProbe>().refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
            env_cubemap.GetComponent<ReflectionProbe>().RenderProbe();

            MapObject.SetActive(false);
        }
        else if(MapObjectName[0] == "env_clip")
        {
            MapObject.GetComponent<MeshRenderer>().material = Wireframe;
            //MapObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else if(MapObjectName[0] == "env_fire")
        {
            Instantiate(env_fire, MapObject.transform.position, MapObject.transform.localRotation);

            MapObject.SetActive(false);
        }
        else if(MapObjectName[0] == "env_dust")
        {
            Instantiate(env_dust, MapObject.transform.position, MapObject.transform.localRotation);

            MapObject.SetActive(false);
        }
        else
        {
        }
    }

    void AddColision()
    {
        GameObject.Find("Console").GetComponent<Console>().AddLine("\nAdding colliders...");

        Transform transform = LoadedMap.transform;
        foreach (Transform child in transform)
        {
            SetupMapEntities(child.gameObject);
            if(child.gameObject.GetComponent<MeshCollider>() == null)
            {
                child.gameObject.AddComponent<MeshCollider>();
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
            string filepath = @Application.streamingAssetsPath + "/Addons/" + PlayerPrefs.GetString("AddonToLoad") + "/Maps/" + MapName + ".gltf";
            GameObject.Find("Console").GetComponent<Console>().AddLine("\n" + filepath ); 
            Importer.ImportGLTFAsync(filepath, new ImportSettings(), OnFinishAsync);
        }
        else
        {
            Player2D.SetActive(true);
            Player3D.SetActive(false);
        }
    }

    void OnFinishAsync(GameObject result, AnimationClip[] animations)
    {
        //Debug.Log("Finished importing " + result.name);
        LoadedMap = result;
        AddColision();
        LoadingScreenUI.SetActive(false);
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
