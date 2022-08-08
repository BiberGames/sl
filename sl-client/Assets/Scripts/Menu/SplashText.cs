using UnityEngine;
using System.IO;
using System;
using TMPro;

public class SplashText : MonoBehaviour
{
    int RandomInt = 0;
    public TextMeshPro[] TextObjects;
    public string[] SplashTexts;
    public float rotationfactor = 1f;

    void Start()
    {
        SplashTexts = File.ReadAllLines(@Application.streamingAssetsPath + "/Splash.txt");
        RandomText();
    }

    void Update()
    {
        transform.Rotate(rotationfactor * Time.deltaTime, rotationfactor * Time.deltaTime, rotationfactor * Time.deltaTime, Space.Self);
    }

    void RandomText()
    {
        var CurrentDate = DateTime.Now.ToString("dd-MM");

        RandomInt = UnityEngine.Random.Range(0, SplashTexts.Length);

        for(int i = 0; i < TextObjects.Length; i++)
        {
            if(CurrentDate == "01-12")
            {
                TextObjects[i].text = "Only today!";
            }
            else if(CurrentDate == "29-05")
            {
                TextObjects[i].text = "Happy Birthday @BENJA_303!";
            }
            else if(CurrentDate == "31-09")
            {
                TextObjects[i].text = "happy halloween!";
            }
            else if(CurrentDate == "24-12")
            {
                TextObjects[i].text = "merry xmas!";
            }
            else if(CurrentDate == "31-12")
            {
                TextObjects[i].text = "Happy new year!";
            }
            else
            {
                TextObjects[i].text = SplashTexts[RandomInt];
            }
        }
    }
}
