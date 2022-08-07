using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAddonsMenu : MonoBehaviour
{
    public GameObject Menu;
    public Vector3 OpenScale;
    public float OpenDuration;

    public void OpenMenu()
    {
        Menu.SetActive(true);
        LeanTween.scale(Menu, OpenScale, OpenDuration);
    }
}
