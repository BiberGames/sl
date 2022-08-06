using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadAddon : MonoBehaviour
{
    public GameObject AddonSelectorTemplate;
    public Transform Container;
    private string[] _MapAtributes;
    public string[] AddonNameBuffer;
    public string[] AddonFolderNameBuffer;
    private GameObject PresedButton;
    int BID = 0;

    void OnEnable()
    {
        CreateAddonList();
        AddonSelectorTemplate.SetActive(false);
    }

    private void CreateSelector(int Index)
    {
        GameObject CurrentFilePreview = Instantiate(AddonSelectorTemplate, new Vector3(0,0,0), Quaternion.identity, Container);
        CurrentFilePreview.GetComponent<ButtonID>().ID = BID++;

        CurrentFilePreview.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = _MapAtributes[0];
        RawImage _RawImageCurrentFilePreview = CurrentFilePreview.transform.GetChild(0).gameObject.GetComponent<RawImage>();

        StartCoroutine(LoadTexture(_RawImageCurrentFilePreview, @Application.streamingAssetsPath + "/Addons/" + AddonFolderNameBuffer[Index] + "/Thumbnail.png"));
    }

    public void StartAddon()
    {
        int PressedButtonID = PresedButton.GetComponent<ButtonID>().ID;
        Debug.Log(PressedButtonID);
        
        PlayerPrefs.SetString("AddonToLoad", AddonFolderNameBuffer[PressedButtonID]);
        SceneManager.LoadScene("LuaScene");
    }

    // Update is called once per frame
    void Update()
    {
        PresedButton = EventSystem.current.currentSelectedGameObject;
    }

    private void CreateAddonList()
    {
        DirectoryInfo AddonDir = new DirectoryInfo(@Application.streamingAssetsPath + "/Addons/");
        DirectoryInfo[] AddonDirList = AddonDir.GetDirectories();

        AddonNameBuffer = new string[AddonDirList.Length];
        AddonFolderNameBuffer = new string[AddonDirList.Length];

        for(int i = 0; i < AddonDirList.Length; i++)
        {
            string MapAtributes = File.ReadAllText(@Application.streamingAssetsPath + "/Addons/" + AddonDirList[i].Name + "/addon.txt");
            _MapAtributes = MapAtributes.Split('\n');

            AddonNameBuffer[i] = _MapAtributes[0];
            AddonFolderNameBuffer[i] = AddonDirList[i].Name;

            CreateSelector(i);
        }
    }

    private IEnumerator LoadTexture(RawImage input1, string Path)
    {
        string fullpath = "file:///" + Path;
        using (WWW www = new WWW(fullpath))
        {
            yield return www;
            input1.texture = www.texture;
        }
    }
}
