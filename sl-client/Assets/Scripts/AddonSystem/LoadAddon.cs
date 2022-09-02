using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct MapAtribute
{
    public string AddonFolderName;
    public string AddonName;
    public string AddonCreator;
    public string Addon3dScene;
    public string AddonType;
    public string AddonMode;
}

public class LoadAddon : MonoBehaviour
{
    public GameObject Button;
    public Transform Contriner;
    public MapAtribute[] MapAtributes;
    public Sprite[] AddonTypeIcons;
    int BID = 0;
    GameObject PresedButton;
    int CurrentButtonID = 0;

    void OnEnable()
    {
        CreateAddonList();
    }

    void CreateAddonList()
    {
        DirectoryInfo AddonDir = new DirectoryInfo(@Application.streamingAssetsPath + "/Addons/");
        DirectoryInfo[] AddonDirList = AddonDir.GetDirectories();

        MapAtributes = new MapAtribute[AddonDirList.Length];

        for(CurrentButtonID = 0; CurrentButtonID < AddonDirList.Length; CurrentButtonID++)
        {
            if (File.Exists(@Application.streamingAssetsPath + "/Addons/" + AddonDirList[CurrentButtonID].Name + "/addon.txt"))
            {
                string[] AddonInfoRaw = File.ReadAllText(@Application.streamingAssetsPath + "/Addons/" + AddonDirList[CurrentButtonID].Name + "/addon.txt").Split('\n');
                MapAtributes[CurrentButtonID].AddonFolderName = AddonDirList[CurrentButtonID].Name;
                MapAtributes[CurrentButtonID].AddonName = AddonInfoRaw[0];
                MapAtributes[CurrentButtonID].AddonCreator = AddonInfoRaw[1];
                MapAtributes[CurrentButtonID].Addon3dScene = AddonInfoRaw[2];
                MapAtributes[CurrentButtonID].AddonType = AddonInfoRaw[3];
                MapAtributes[CurrentButtonID].AddonMode = AddonInfoRaw[4];

                CreateButton(MapAtributes[CurrentButtonID]);

                GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#228B22>Addons > Found addon " + MapAtributes[CurrentButtonID].AddonName + "<color=#FFFFFF>");
            }
        }

        Button.SetActive(false);
    }

    void CreateButton(MapAtribute _MapAtribute)
    {
        GameObject CurrentButton = Instantiate(Button, new Vector3(0,0,0), Quaternion.identity, Contriner);
        CurrentButton.GetComponent<ButtonID>().ID = BID++;

        CurrentButton.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = _MapAtribute.AddonName;

        RawImage _RawImageCurrentFilePreview = CurrentButton.transform.GetChild(0).gameObject.GetComponent<RawImage>();

        StartCoroutine(LoadThumbnail(_RawImageCurrentFilePreview, @Application.streamingAssetsPath + "/Addons/" + MapAtributes[CurrentButtonID].AddonFolderName + "/Thumbnail.png"));

    }

    public void StartAddon()
    {
        int PressedButtonID = PresedButton.GetComponent<ButtonID>().ID;

        if (MapAtributes[PressedButtonID].AddonMode == "3D") PlayerPrefs.SetInt("AddonMode", 1); else PlayerPrefs.SetInt("AddonMode", 0);
        
        PlayerPrefs.SetString("AddonToLoad", MapAtributes[PressedButtonID].AddonFolderName);
        PlayerPrefs.SetString("MapToLoad", MapAtributes[PressedButtonID].Addon3dScene);
        SceneManager.LoadScene("LuaScene");
    }

    void Update()
    {
        PresedButton = EventSystem.current.currentSelectedGameObject;
    }

    private IEnumerator LoadThumbnail(RawImage input1, string Path)
    {
        string fullpath = "file:///" + Path;
        using (WWW www = new WWW(fullpath))
        {
            yield return www;
            input1.texture = www.texture;
        }
    }
}