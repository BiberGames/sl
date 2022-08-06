using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Console : MonoBehaviour
{
    public TextMeshProUGUI Output;
    public Engine_VersionInfo _Engine_VersionInfo;
    public TMP_InputField UserInput;
    public Sprite ErrorSprite;

    public GameObject ConsoleWindow;

    public Vector3 OldWindowPos;
    public Vector3 NewWindowPos;

    //public Fragsurf.Movement.SurfCharacter PlayerSpeedControler;
    //public PlayerAiming CameraController;

    public string[] maps;
    bool IsConsoleOpen = false;

    public float Duration = 0.3f;

    public void AddLine(string Line)
    {
        Output.text += Line;
    }

    public void Clear()
    {
        Output.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            var SavePath = @Application.streamingAssetsPath + "/Screenshots/";
            var CurrentDate = DateTime.Now.ToString("dd-MM-yyyy");
            var CurrentTime = DateTime.Now.ToString("HH_mm_ss");
            ScreenCapture.CaptureScreenshot(SavePath + "Screenshot-" + CurrentDate + "_" + CurrentTime + ".png");
            Output.text += "\n<i>[Saved screenshot! to: " + SavePath + "Screenshot-" + CurrentDate + "_" + CurrentTime + ".png" + "]</i>";
        }

        if(Input.GetKeyDown(KeyCode.F4))
        {
            IsConsoleOpen = !IsConsoleOpen;

            if(IsConsoleOpen)
            {
                LeanTween.moveY(ConsoleWindow.gameObject.GetComponent<RectTransform>(), NewWindowPos.y, Duration).setDelay(Duration);
                //GetConsoleInput();
            }
            else
            {
                LeanTween.moveY(ConsoleWindow.gameObject.GetComponent<RectTransform>(), OldWindowPos.y, Duration).setDelay(Duration);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string InputText = UserInput.text;
            string[] TextBuffer = InputText.Split(' ');
            Output.text += "\n<i>[ " + InputText + " ]</i>";
            UserInput.text = "";
            if(TextBuffer[0] == "")
            {
                Output.text += "";
            }

            if(TextBuffer[0] == "help")
            {
                //Output.text += "Speed 0 - aunimitied speed (sonic go brrrrrrrr)\n mouse_sens 0 - unlimited (change mouse sensitivity";
                Output.text += "\n=====Console=Help=============================================";
                Output.text += "\nhelp\t=>\topens this page";
                Output.text += "\nReload\t=>\treloads the map / scene";
                Output.text += "\ncl_draw_viewmodel_side\t=>\tchandes the weapon side 0 = right 1 = left";
                Output.text += "\nlua halt / resume\t=>\tStops / resums lua from running";
            }

            else if(TextBuffer[0] == "maps")
            {
                if(TextBuffer[1] == "*")
                {
                    //GameObject.Find("Notifications").GetComponent<NotificationSystem>().CreateMasage(ErrorSprite, "Nothing found...", "");
                }
            }

            else if(TextBuffer[0] == "LogError")
            {
                //GameObject.Find("Notifications").GetComponent<NotificationSystem>().CreateMasage(ErrorSprite, TextBuffer[1], TextBuffer[2]);
            }

            else if(TextBuffer[0] == "reload")
            {
                //GameObject.Find("Notifications").GetComponent<NotificationSystem>().CreateMasage(ErrorSprite, "Realoding!", SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //GameObject.Find("LuaRunner").GetComponent<LuaRunner>().Lua();
            }

            else if(TextBuffer[0] == "map")
            {
                SceneManager.LoadScene(TextBuffer[1]);
            }

            else if(TextBuffer[0] == "cl_draw_viewmodel_side")
            {
                if(TextBuffer[1] == "0")
                {
                    GameObject.Find("mmod").transform.localScale = new Vector3(1, 1, 1);
                }
                if(TextBuffer[1] == "1")
                {
                    GameObject.Find("mmod").transform.localScale = new Vector3(-1, 1, 1);
                }
                GameObject.Find("mmod").transform.position = new Vector3(0, 0, 0);
            }

            else if(TextBuffer[0] == "exit")
            {
                Application.Quit();
            }

            else if(TextBuffer[0] == "clear" || TextBuffer[0] == "cls")
            {
                Clear();
            }

            else if(TextBuffer[0] == "setpos" || TextBuffer[0] == "tp" || TextBuffer[0] == "teleport")
            {
                GameObject.Find("player").transform.position = new Vector3(float.Parse(TextBuffer[1]), float.Parse(TextBuffer[2]), float.Parse(TextBuffer[3]));
            }

            else if(TextBuffer[0] == "addons")
            {
                if(TextBuffer[1] == "list")
                {
                    AddLine("\nList of installed addons:");
                }
            }

            else if(TextBuffer[0] == "lua")
            {
                if(TextBuffer[1] == "")
                {
                    AddLine("halt | resume | run");
                }
                if(TextBuffer[1] == "halt")
                {
                    GameObject.Find("LuaRunner").GetComponent<LuaRunner>().IsHalted = true;
                    AddLine("\n<color=#FF0000>Lua halted...<color=#FFFFFF>");
                }
                if(TextBuffer[1] == "resume")
                {
                    GameObject.Find("LuaRunner").GetComponent<LuaRunner>().IsHalted = false;
                    AddLine("\n<color=#FFFF00>Lua resumed...<color=#FFFFFF>");
                }
                if(TextBuffer[1] == "run")
                {
                    GameObject.Find("LuaRunner").GetComponent<LuaRunner>().CallFuncFromConsole(TextBuffer[2]);
                }
            }

            else if(TextBuffer[0] == "version" || TextBuffer[0] == "ver")
            {
                AddLine("\n=====Version=Info=============================================");
                AddLine("\n=Version:" + _Engine_VersionInfo.VersionString);
            }

            else
            {
                AddLine("\n<color=#FF0000>Command '" + InputText +  "' was not found...<color=#FFFFFF>");
            }
        }
    }
}
