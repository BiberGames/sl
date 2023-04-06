using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class Console : MonoBehaviour
{
    public TextMeshProUGUI Output;
    public Engine_VersionInfo _Engine_VersionInfo;
    public TMP_InputField UserInput;

    public GameObject ConsoleWindow;

    public float ConsoleOpenY;
    public float ConsoleCloseY;

    public float Duration = 0.3f;

    private bool IsConsoleOpen = false;

    private bool CanOpenConsole = true;
    private bool ShowUnityLog = true;

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
     
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if(type == LogType.Exception && ShowUnityLog)
        {
            GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#808080>Unity > " + logString + "<color=#FFFFFF>");
        }
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            string[] AutoExecContent = System.IO.File.ReadAllLines(@Application.streamingAssetsPath + "/autoexec.con");
            for(int i = 0; i < AutoExecContent.Length; i++)
            {
                string[] TextBuffer = AutoExecContent[i].Split(' ');
                RunCmd(TextBuffer);
            }
        }
        else
        {
            AddLine("Not loading autoexec in this scene. (not main menu)");
        }
    }

    public void AddLine(string Line)
    {
        Output.text += Line;
    }

    public void Clear()
    {
        Output.text = "";
    }

    private void TakeScreenshot()
    {
        var SavePath = @Application.streamingAssetsPath + "/Screenshots/";
        var CurrentDate = DateTime.Now.ToString("dd-MM-yyyy");
        var CurrentTime = DateTime.Now.ToString("HH_mm_ss");
        ScreenCapture.CaptureScreenshot(SavePath + "Screenshot-" + CurrentDate + "_" + CurrentTime + ".png");
        Output.text += "\n<i>[Saved screenshot! to: " + SavePath + "Screenshot-" + CurrentDate + "_" + CurrentTime + ".png" + "]</i>";
    }

    private void ToggleConsoleState()
    {
        IsConsoleOpen = !IsConsoleOpen;
        if(IsConsoleOpen)
        {
            LeanTween.moveY(ConsoleWindow.gameObject.GetComponent<RectTransform>(), ConsoleOpenY, Duration).setDelay(Duration);
        }
        else
        {
            LeanTween.moveY(ConsoleWindow.gameObject.GetComponent<RectTransform>(), ConsoleCloseY, Duration).setDelay(Duration);
        }
    }

    public void CanUseConsole(bool _CanOpenConsole)
    {
        CanOpenConsole = _CanOpenConsole;
    }

    private void RunCmd(string[] Command)
    {
        if(Command[0] == "")
        {
            Output.text += "";
        }

        if(Command[0].Substring(0, 1) == "#")
        {
            return;
        }

        if(Command[0] == "help")
        {
            Output.text += "\n=====Console=Help=============================================";
            Output.text += "\nhelp\t=>\topens this page";
            Output.text += "\nReload\t=>\treloads the map / scene";
            Output.text += "\ncl_draw_viewmodel_side\t=>\tchandes the weapon side 0 = right 1 = left";
            Output.text += "\nlua halt / resume\t=>\tStops / resums lua from running";
        }
        else if(Command[0] == "reload")
        {
            AddLine("Reloading current scene!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(Command[0] == "scene")
        {
            SceneManager.LoadScene(Command[1]);
        }
        else if(Command[0] == "exit" || Command[0] == "quit")
        {
            Application.Quit();
        }
        else if(Command[0] == "clear" || Command[0] == "cls")
        {
            Clear();
        }
        else if(Command[0] == "lua")
        {
            if(Command[1] == "")
            {
                AddLine("halt | resume | run");
            }
            if(Command[1] == "halt")
            {
                GameObject.Find("LuaRunner").GetComponent<LuaRunner>().IsHalted = true;
                AddLine("\n<color=#FF0000>Lua halted...<color=#FFFFFF>");
            }
            if(Command[1] == "resume")
            {
                GameObject.Find("LuaRunner").GetComponent<LuaRunner>().IsHalted = false;
                AddLine("\n<color=#FFFF00>Lua resumed...<color=#FFFFFF>");
            }
            if(Command[1] == "run")
            {
                GameObject.Find("LuaRunner").GetComponent<LuaRunner>().RunLuaFromConsole(Command[2]);
            }
        }
        else if(Command[0] == "version" || Command[0] == "ver")
        {
            AddLine("\n=====Version=Info=============================================");
            AddLine("\n=Version:" + _Engine_VersionInfo.VersionString);
        }
        else if(Command[0] == "cl_drawclip")
        {
            Color customColor = new Color(1f, 0f, 0f, float.Parse(Command[1]));
            GameObject.Find("LuaRunner").GetComponent<AddonMapLoader>().Wireframe.SetColor("_WireColor", customColor);
        }
        else if(Command[0] == "ShowUnityLog")
        {
            ShowUnityLog = !ShowUnityLog;
        }
        else
        {
            AddLine("\n<color=#FF0000>Command '" + Command[0] +  "' was not found...<color=#FFFFFF>");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2))
        {
            TakeScreenshot();
        }

        if(Input.GetKeyDown(KeyCode.F4) && CanOpenConsole)
        {
            transform.SetAsLastSibling();
            ToggleConsoleState();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string InputText = UserInput.text;
            string[] TextBuffer = InputText.Split(' ');
            Output.text += "\n<i>[ " + InputText + " ]</i>";
            UserInput.text = "";
            RunCmd(TextBuffer);
        }
    }
}
