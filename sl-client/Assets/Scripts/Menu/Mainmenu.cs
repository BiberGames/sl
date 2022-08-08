using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainmenu : MonoBehaviour
{
    public GameObject Menu;
    public Vector3 OpenScale;
    public float OpenDuration;

    public void OpenMenu()
    {
        Menu.SetActive(true);
        LeanTween.scale(Menu, OpenScale, OpenDuration);
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
