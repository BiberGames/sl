using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainmenu : MonoBehaviour
{
    public RectTransform Menu;
    public Material BlurUI;
    public float MoveValue = 400;
    public float OpenDuration;

    public void Start()
    {
        BlurUI.SetFloat("_Radius", 0.0f);
    }

    public void OpenMenu()
    {
        //Menu.SetActive(true);
        LeanTween.moveX(Menu, MoveValue, OpenDuration).setEase(LeanTweenType.easeOutBounce);
        BlurUI.SetFloat("_Radius", 5.0f);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenDCC()
    {
        Application.OpenURL("https://discord.gg/emWThStv32");
    }
}
