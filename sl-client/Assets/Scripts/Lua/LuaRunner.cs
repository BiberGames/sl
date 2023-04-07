///////////////////////////////////////////////////////////
//                                                       //
//  This is the Lua api form sourcelike (my old game)    //
//                                                       //
///////////////////////////////////////////////////////////

using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using MoonSharp.Interpreter;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

[RequireComponent(typeof(LuaAssetLoader))]
public class LuaRunner : MonoBehaviour
{
    private Script LuaScript;
    public bool IsHalted;
    public string RawImportedLuaCode;
    public static string AddonName;

    public Sprite[] SpriteCash;

    public static void RegisterCustomValueTypes()
    {
        // Vector 2

        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Vector2),
            dynVal => {
                Table table = dynVal.Table;
                float x = (float)((double)table[1]);
                float y = (float)((double)table[2]);
                return new Vector2(x, y);
            }
        );
        Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion<Vector2>(
            (script, vector) => {
                DynValue x = DynValue.NewNumber((double)vector.x);
                DynValue y = DynValue.NewNumber((double)vector.y);
                DynValue dynVal = DynValue.NewTable(script, new DynValue[] { x, y });
                return dynVal;
            }
        );

        // Vector3

        Script.GlobalOptions.CustomConverters.SetScriptToClrCustomConversion(DataType.Table, typeof(Vector3),
            dynVal => {
                Table table = dynVal.Table;
                float x = (float)((double)table[1]);
                float y = (float)((double)table[2]);
                float z = (float)((double)table[3]);
                return new Vector3(x, y, z);
            }
        );
        Script.GlobalOptions.CustomConverters.SetClrToScriptCustomConversion<Vector3>(
            (script, vector) => {
                DynValue x = DynValue.NewNumber((double)vector.x);
                DynValue y = DynValue.NewNumber((double)vector.y);
                DynValue z = DynValue.NewNumber((double)vector.z);
                DynValue dynVal = DynValue.NewTable(script, new DynValue[] { x, y, z });
                return dynVal;
            }
        );

    }

    #region CLConsole
    [MoonSharpUserData]
    class CLConsole : MonoBehaviour
    {
        public void Log(string Input)
        {
            GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FFFF00>Lua > " + Input + "<color=#FFFFFF>"); 
        }

        public void Clear()
        {
            GameObject.Find("Console").GetComponent<Console>().Clear();
        }

        public void CanUseConsole(bool _CanOpenConsole)
        {
             GameObject.Find("Console").GetComponent<Console>().CanUseConsole(_CanOpenConsole);
        }
    }
    #endregion

    #region CLTransform
    [MoonSharpUserData]
    class CLTransform : MonoBehaviour
    {
        public void Position(string GameObjectName, float X, float Y, float Z, int Mode)
        {
            if(Mode == 0)
            {
                GameObject.Find(GameObjectName).transform.localPosition = new Vector3(X, Y, Z);
            }
            else if(Mode == 1)
            {
                GameObject.Find(GameObjectName).transform.localPosition += new Vector3(X, Y, Z);
            }
        }

        public List<float> GetPosition(string GameObjectName)
        {
            float[] PositionCordinates = new float[3];
            PositionCordinates[0] = GameObject.Find(GameObjectName).transform.localPosition.x;
            PositionCordinates[1] = GameObject.Find(GameObjectName).transform.localPosition.y;
            PositionCordinates[2] = GameObject.Find(GameObjectName).transform.localPosition.z;

            return new List<float>(PositionCordinates);
        }

        public void Rotation(string GameObjectName, float X, float Y, float Z, int Mode)
        {
            if(Mode == 0)
            {
                GameObject.Find(GameObjectName).transform.eulerAngles = new Vector3(X, Y, Z);
            }
            else if(Mode == 1)
            {
                GameObject.Find(GameObjectName).transform.eulerAngles += new Vector3(X, Y, Z) * Time.deltaTime;
            }
        }

        public List<float> GetRotation(string GameObjectName)
        {
            float[] RotationCordinates = new float[3];
            RotationCordinates[0] = GameObject.Find(GameObjectName).transform.eulerAngles.x;
            RotationCordinates[1] = GameObject.Find(GameObjectName).transform.eulerAngles.y;
            RotationCordinates[2] = GameObject.Find(GameObjectName).transform.eulerAngles.z;

            return new List<float>(RotationCordinates);
        }

        public void Scale(string GameObjectName, float X, float Y, float Z, int Mode)
        {
            if(Mode == 0)
            {
                GameObject.Find(GameObjectName).transform.localScale = new Vector3(X, Y, Z);
            }
            else if(Mode == 1)
            {
                GameObject.Find(GameObjectName).transform.localScale += new Vector3(X, Y, Z) * Time.deltaTime;
            }
        }

        public List<float> GetScale(string GameObjectName)
        {
            float[] ScaleCordinates = new float[3];
            ScaleCordinates[0] = GameObject.Find(GameObjectName).transform.localScale.x;
            ScaleCordinates[1] = GameObject.Find(GameObjectName).transform.localScale.y;
            ScaleCordinates[2] = GameObject.Find(GameObjectName).transform.localScale.z;

            return new List<float>(ScaleCordinates);
        }

        public void MoveForward(string GameObjectName, float MoveValue)
        {
            GameObject.Find(GameObjectName).transform.position += GameObject.Find(GameObjectName).transform.forward * Time.deltaTime * MoveValue;
        } 

        public void MoveSide(string GameObjectName, float MoveValue)
        {
            GameObject.Find(GameObjectName).transform.position += GameObject.Find(GameObjectName).transform.right * Time.deltaTime * MoveValue;
        }

        public void MoveUp(string GameObjectName, float MoveValue)
        {
            GameObject.Find(GameObjectName).transform.position += GameObject.Find(GameObjectName).transform.up * Time.deltaTime * MoveValue;
        }

        public void SetParent(string GameObjectName, string PatentGameObjectName)
        {
            GameObject.Find(GameObjectName).transform.SetParent(GameObject.Find(PatentGameObjectName).transform);
        }

        public void LookAt(string GameObjectName, string ToLookAtGameObjectName)
        {
            GameObject.Find(GameObjectName).transform.LookAt(GameObject.Find(ToLookAtGameObjectName).transform);
        }

        public void SetAsFirstSibling(string GameObjectName)
        {
            GameObject.Find(GameObjectName).transform.SetAsFirstSibling();
        }

        public void SetAsLastSibling(string GameObjectName)
        {
            GameObject.Find(GameObjectName).transform.SetAsLastSibling();
        }

        public void SetSiblingIndex(string GameObjectName, int SiblingIndex)
        {
            GameObject.Find(GameObjectName).transform.SetSiblingIndex(SiblingIndex);
        }

        public int GetSiblingIndex(string GameObjectName)
        {
            return GameObject.Find(GameObjectName).transform.GetSiblingIndex();
        }

        public void SetTag(string GameObjectName, string TagName)
        {
            GameObject.Find(GameObjectName).transform.tag = TagName;
        }
    }
    #endregion

    #region CLInput
    [MoonSharpUserData]
    class CLInput : MonoBehaviour
    {
        public float GetAxis(string AxisName)
        {
            return Input.GetAxis(AxisName);
        }

        public float GetMousePos2DX()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            return worldPosition.x;
        }

        public float GetMousePos2DY()
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

            return worldPosition.y;
        }
    }
    #endregion

    #region CLCursorLockMode
    [MoonSharpUserData]
    class CLCursorLockMode : MonoBehaviour
    {
        public void LockMode(int Mode)
        {
            if(Mode == 0)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else if(Mode == 1)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if(Mode == 2)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
    #endregion

    #region CLTime
    [MoonSharpUserData]
    class CLTime : MonoBehaviour
    {
        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public float GetTime()
        {
            return Time.time;
        }
    }
    #endregion

    #region CLCamera
    [MoonSharpUserData]
    class CLCamera : MonoBehaviour
    {
        public string GetMainCamera()
        {
            return Camera.main.gameObject.name;
        }

        public void SetFov(string CameraName, float Fov)
        {
            GameObject.Find(CameraName).GetComponent<Camera>().fieldOfView = Fov;
        }

        public void SetSize(string CameraName, float Fov)
        {
            GameObject.Find(CameraName).GetComponent<Camera>().orthographicSize = Fov;
        }

        public void IsOrthographic(string CameraName, bool Mode)
        {
            GameObject.Find(CameraName).GetComponent<Camera>().orthographic = Mode;
        }

        public void SetCameraClipping(string CameraName, float NearPlane, float FarPlane)
        {
            GameObject.Find(CameraName).GetComponent<Camera>().nearClipPlane = NearPlane;
            GameObject.Find(CameraName).GetComponent<Camera>().farClipPlane = FarPlane;
        }
    }
    #endregion

    #region CLGameObject
    [MoonSharpUserData]
    class CLGameObject : MonoBehaviour
    {
        public void CreateEmpty(string EmptyName)
        {
            GameObject LuaCreatedGameObject = new GameObject(EmptyName);
        }

        public void AddComponent(string GameObjectName, string ComponentName)
        {
            if(ComponentName == "Mesh.MeshFilter")
            {
                GameObject.Find(GameObjectName).AddComponent<MeshFilter>();
            }
            else if(ComponentName == "Mash.MeshRenderer")
            {
                GameObject.Find(GameObjectName).AddComponent<MeshRenderer>();
            }
            else if(ComponentName == "Physics.Rigidbody")
            {
                GameObject.Find(GameObjectName).AddComponent<Rigidbody>();
            }
            else if(ComponentName == "Physics.CharacterController")
            {
                GameObject.Find(GameObjectName).AddComponent<CharacterController>();
            }
            else if(ComponentName == "Physics.BoxCollider")
            {
                GameObject.Find(GameObjectName).AddComponent<BoxCollider>();
            }
            else if(ComponentName == "Physics.SphereCollider")
            {
                GameObject.Find(GameObjectName).AddComponent<SphereCollider>();
            }
            else if(ComponentName == "Physics.CapsuleCollider")
            {
                GameObject.Find(GameObjectName).AddComponent<CapsuleCollider>();
            }
            else if(ComponentName == "Physics.MeshCollider")
            {
                GameObject.Find(GameObjectName).AddComponent<MeshCollider>();
            }
            else if(ComponentName == "Physics.Rigidbody2D")
            {
                GameObject.Find(GameObjectName).AddComponent<Rigidbody2D>();
            } 
            else if(ComponentName == "Physics.BoxCollider2D")
            {
                GameObject.Find(GameObjectName).AddComponent<BoxCollider2D>();
            }
            else if(ComponentName == "Physics.CircleCollider2D")
            {
                GameObject.Find(GameObjectName).AddComponent<CircleCollider2D>();
            }
            else if(ComponentName == "Audio.AudioListener")
            {
                GameObject.Find(GameObjectName).AddComponent<AudioListener>();
            }
            else if(ComponentName == "Audio.AudioSource")
            {
                GameObject.Find(GameObjectName).AddComponent<AudioSource>();
            }
            else if(ComponentName == "UI.Text")
            {
                GameObject.Find(GameObjectName).AddComponent<TextMeshProUGUI>();
            }
            else if(ComponentName == "UI.Image")
            {
                GameObject.Find(GameObjectName).AddComponent<RawImage>();
            }
            else if(ComponentName == "Renderer.Camera")
            {
                GameObject.Find(GameObjectName).AddComponent<Camera>();
            }
            else if(ComponentName == "2D.Sprite")
            {
                GameObject.Find(GameObjectName).AddComponent<SpriteRenderer>();
            }
            else
            {
                GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FF0000> Lua > " + "Component '" + ComponentName + "' not found..." + "<color=#FFFFFF>"); 
            }
        }

        public void SetActive(string GameObjectName, bool State)
        {
            GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FFFF00> Lua > " + "Setting '" + GameObjectName + "' to " + State + "<color=#FFFFFF>"); 
            GameObject.Find(GameObjectName).SetActive(State);
        }

        public void Destroy(string GameObjectName)
        {
            Destroy(GameObjectName);
        }
    }
    #endregion

    #region CLUI
    [MoonSharpUserData]
    class CLUI : MonoBehaviour
    {
        public void SetImage(string ImageViewer, string ImagePath)
        {
            string fullpath = "file://" + Application.streamingAssetsPath + "/" + ImagePath;

            GameObject.Find("LuaRunner").GetComponent<LuaAssetLoader>().LoadImage(ImageViewer, fullpath);
        }
        public void SetSize(string ImageViewer, int x1, int y1, int x2, int y2)
        {
            RectTransform _RectTransform = GameObject.Find(ImageViewer).GetComponent<RectTransform>();
            _RectTransform.anchorMin = new Vector2(x1, y1);
            _RectTransform.anchorMax = new Vector2(x2, y2);
            //_RectTransform.sizeDelta = new Vector2(x, y);
        }
        public void SetSpriteAlpha(string ImageViewer, float AlphaIntensety, float FadeDuration)
        {
            GameObject.Find(ImageViewer).GetComponent<RawImage>().CrossFadeAlpha(AlphaIntensety, FadeDuration, false);
        }
        public int GetScreenHeight()
        {
            return Screen.height;
        }
        public int GetScreenWidth()
        {
            return Screen.width;
        }
        public void SetAnchors(string GameObjectName, float x1, float y1, float x2, float y2)
        {
            GameObject.Find(GameObjectName).GetComponent<RectTransform>().anchorMin = new Vector2(x1, x1);
            GameObject.Find(GameObjectName).GetComponent<RectTransform>().anchorMax  = new Vector2(x2, x2);
            GameObject.Find(GameObjectName).GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            GameObject.Find(GameObjectName).GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        }
        public void CreateText(string GameObjectName, int Order, string Canvas, string Massage, int FontStyle, string Wrapping, string Overflow)
        {
            GameObject Text = new GameObject(GameObjectName);
            Text.transform.SetParent(GameObject.Find(Canvas).transform);
            Text.transform.SetSiblingIndex(Order);
            Text.AddComponent<MeshRenderer>();
            Text.AddComponent<TextMeshProUGUI>();
            Text.GetComponent<TextMeshProUGUI>().text = Massage;
            
        }
        public void SetText(string GameObjectName, string Massage)
        {
            GameObject.Find(GameObjectName).GetComponent<TextMeshProUGUI>().text = Massage;
        }
    }
    #endregion
    
    #region CLIO
    [MoonSharpUserData]
    class CLIO : MonoBehaviour
    {
        public List<string> GetFilesInFolder(string DirectoryPath,string FileType)
        { 
            DirectoryInfo FileDirectoryInfo = new DirectoryInfo(@Application.streamingAssetsPath + "/" + DirectoryPath);
            FileInfo[] files = FileDirectoryInfo.GetFiles(FileType);
            string[] AllFiles = new string[files.Length];
            for(int i = 0; i < files.Length; i++)
            {
                AllFiles[i] = DirectoryPath + "/" + files[i].Name;
            }
            return new List<string>(AllFiles);;
        }

        public string GetStreamingAssetsPath()
        {
            return Application.streamingAssetsPath;
        }

        public string LoadFileRaw(string FilePath)
        {
            return System.IO.File.ReadAllText(@Application.streamingAssetsPath + "/" + FilePath);
        }
    } 
    #endregion
    
    #region CLAudio
    [MoonSharpUserData]
    class CLAudio : MonoBehaviour
    {
        public void PlayAudioFile(string AudioSourceName, string AudioFilePath)
        {
            string fullpath = "file://" + Application.streamingAssetsPath + "/" + AudioFilePath;

            GameObject.Find("LuaRunner").GetComponent<LuaAssetLoader>().LoadAudio(AudioSourceName, fullpath);
        }

        public void SetVolume(string AudioSourceName, float Volume)
        {
            Volume = Mathf.Clamp(Volume, 0.0f, 1.0f);
            GameObject.Find(AudioSourceName).GetComponent<AudioSource>().volume = Volume;
        }
    }
    #endregion

    #region CLMap
    [MoonSharpUserData]
    class CLMap : MonoBehaviour
    {
        public void EditSun(float R, float G, float B, float Intensety)
        {
            //GameObject.Find("Sun").GetComponent<Light>().type = LightType.Directional;
            Debug.Log("R: " + R + " G: " + G + " B: " + B + " Intensety:" + Intensety);
            GameObject.Find("Sun").GetComponent<Light>().color = new Color(R, G, B, 1f);
            GameObject.Find("Sun").GetComponent<Light>().intensity = Intensety;
        }

        public void CreatePointLight(string GameObjectName, float Range, float R, float G, float B, float Intensety)
        {
            Destroy(GameObject.Find(GameObjectName).GetComponent<MeshCollider>());
            Destroy(GameObject.Find(GameObjectName).GetComponent<MeshFilter>());
            Destroy(GameObject.Find(GameObjectName).GetComponent<MeshRenderer>());

            GameObject.Find(GameObjectName).AddComponent<Light>();
            GameObject.Find(GameObjectName).GetComponent<Light>().type = LightType.Point;
            GameObject.Find(GameObjectName).GetComponent<Light>().color = new Color(R, G, B, 1f);
            GameObject.Find(GameObjectName).GetComponent<Light>().intensity = Intensety;
        }

        public void CreateSpotLight(string GameObjectName, float Range, float InnerSpotAngle, float OuterSpotAngle, float R, float G, float B, float Intensety, float ShadowStrength)
        {
            Destroy(GameObject.Find(GameObjectName).GetComponent<MeshCollider>());
            Destroy(GameObject.Find(GameObjectName).GetComponent<MeshFilter>());
            Destroy(GameObject.Find(GameObjectName).GetComponent<MeshRenderer>());

            GameObject.Find(GameObjectName).AddComponent<Light>();
            GameObject.Find(GameObjectName).GetComponent<Light>().type = LightType.Spot;
            GameObject.Find(GameObjectName).GetComponent<Light>().innerSpotAngle = InnerSpotAngle;
            GameObject.Find(GameObjectName).GetComponent<Light>().spotAngle = OuterSpotAngle;
            GameObject.Find(GameObjectName).GetComponent<Light>().color = new Color(R, G, B, 1f);
            GameObject.Find(GameObjectName).GetComponent<Light>().intensity = Intensety;
            GameObject.Find(GameObjectName).GetComponent<Light>().shadowStrength = ShadowStrength;
        }
    }
    #endregion

    #region CL2D
    [MoonSharpUserData]
    class CL2D : MonoBehaviour
    {
        public void SetSprite(string SpriteViewer, int SpriteID)
        {
            //string fullpath = "file://" + Application.streamingAssetsPath + "/" + SpritePath;
            GameObject.Find("LuaRunner").GetComponent<LuaAssetLoader>().SetSprite(SpriteViewer, SpriteID);
        }

        public void SetColliderOffset(string GameObjectName, float X, float Y)
        {
            if (GameObject.Find(GameObjectName).GetComponent<BoxCollider2D>())
            {
                GameObject.Find(GameObjectName).GetComponent<BoxCollider2D>().offset = new Vector2(X, Y);
            }
            else if(GameObject.Find(GameObjectName).GetComponent<CapsuleCollider2D>())
            {
                GameObject.Find(GameObjectName).GetComponent<CapsuleCollider2D>().offset = new Vector2(X, Y);
            }
        }

        public void SetBoxColliderSize(string GameObjectName, float Width, float Height)
        {
            GameObject.Find(GameObjectName).GetComponent<BoxCollider2D>().size = new Vector2(Width, Height);
        }

        public void SetCapsuleColliderSize(string GameObjectName, float Width, float Height)
        {
            GameObject.Find(GameObjectName).GetComponent<CapsuleCollider2D>().size = new Vector2(Width, Height);
        }

        public void SetCircleColliderSize(string GameObjectName, float Radius)
        {
            GameObject.Find(GameObjectName).GetComponent<CircleCollider2D>().radius = Radius;
        }

        public void SetRigidbodyConstrains(string GameObjectName, bool X, bool Y, bool Z)
        {
            if(X)
            {
                GameObject.Find(GameObjectName).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            if(Y)
            {
                GameObject.Find(GameObjectName).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            if(Z)
            {
                GameObject.Find(GameObjectName).GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

        public void AddForce(string GameObjectName, float ForceX, float ForceY)
        {
            GameObject.Find(GameObjectName).GetComponent<Rigidbody2D>().AddForce(new Vector2(ForceX, ForceY));
        }

        public bool PlayerGrounded()
        {
            return false;// GameObject.Find("2DPlayer").GetComponent<CharacterController2D>().m_Grounded;
        }

        public void LoadSprite(string SpritePath, int SpriteID, float pixelsPerUnit, bool Filtered)
        {
            GameObject.Find("LuaRunner").GetComponent<LuaAssetLoader>().LoadSprite("file://" + Application.streamingAssetsPath + "/" + SpritePath, SpriteID, pixelsPerUnit, Filtered);
        }

        public bool SpriteLoaded(int SpriteID)
        {
            bool Loaded = false;
            if(GameObject.Find("LuaRunner").GetComponent<LuaRunner>().SpriteCash[SpriteID] == null)
            {
                Loaded = false;
            }
            else
            {
                Loaded = true;
            }

            return Loaded;
        }

        public void SetTrigger(string GameObjectName, bool State)
        {
            GameObject TempGameObject = GameObject.Find(GameObjectName);
            TempGameObject.GetComponent<BoxCollider2D>().isTrigger = State;
            TempGameObject.AddComponent<Trigger2D>();
        }

        public string GetColisionName(string GameObjectName)
        {
            return GameObject.Find(GameObjectName).GetComponent<Trigger2D>().GetValue();
        }
    }
    #endregion

    #region CL3D
    [MoonSharpUserData]
    class CL3D : MonoBehaviour
    {
        public void ImportModel(string Path, string Name, string Layer)
        {
            GameObject.Find("LuaRunner").GetComponent<LuaAssetLoader>().LoadModel(Path, AddonName);
        }

        /*void OnFinishAsync(GameObject result, AnimationClip[] animations)
        {
            //Debug.Log("Finished importing " + result.name);
            LoadedMap = result;
            AddColision();
            LoadingScreenUI.SetActive(false);
        }*/
    }
    #endregion

    #region Utils
    [MoonSharpUserData]
    class Utils : MonoBehaviour
    {
        public List<string> SplitString(string StringToSplit, char SplitChar)
        {
            string[] _Raw = StringToSplit.Split(SplitChar);

            return new List<string>(_Raw);
        }

        public void DelayedLauncher(string FuncName, float WaitTime)
        {
            GameObject.Find("LuaRunner").GetComponent<LuaAssetLoader>().DelayedLauncher(FuncName, WaitTime);
        }

        public string ReturnAddonName()
        {
            return PlayerPrefs.GetString("AddonToLoad");
        }

        public string TestV2(Vector2 test)
        {
            return $"Test vector2: {test.x}, {test.y}";
        }
    }
    #endregion

    void LuaStatusMasages()
    {
        string LuaFile = @Application.streamingAssetsPath + "/Code/" + SceneManager.GetActiveScene().name + ".lua";

        GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FFFF00>Lua > Loaded file: " + LuaFile + "<color=#FFFFFF>");
    }

    void InitLua()
    {
        if(IsHalted == false)
        {
            UserData.RegisterAssembly();
            RegisterCustomValueTypes();
            LuaScript = new Script();

            LuaScript.Globals["CLConsole"] = new CLConsole();
            LuaScript.Globals["CLTransform"] = new CLTransform();
            LuaScript.Globals["CLInput"] = new CLInput();
            LuaScript.Globals["CLCursorLockMode"] = new CLCursorLockMode();
            LuaScript.Globals["CLTime"] = new CLTime();
            LuaScript.Globals["CLCamera"] = new CLCamera();
            LuaScript.Globals["CLGameObject"] = new CLGameObject();
            LuaScript.Globals["CLUI"] = new CLUI();
            LuaScript.Globals["CLIO"] = new CLIO();
            LuaScript.Globals["CLAudio"] = new CLAudio();
            LuaScript.Globals["CLMap"] = new CLMap();
            LuaScript.Globals["CL2D"] = new CL2D();
            LuaScript.Globals["CL3D"] = new CL3D();
            LuaScript.Globals["Utils"] = new Utils();
            
            // Runs lua...
            
            LuaScript.DoString(RawImportedLuaCode);
            DynValue StartRes = LuaScript.Call(LuaScript.Globals["Start"]);
        }
    }
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern bool SetWindowText(System.IntPtr hwnd, System.String lpString);
    [DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
    public static extern System.IntPtr GetActiveWindow();

    public void EditWindowTitle(string NewTitle)
    {
        SetWindowText(GetActiveWindow(), NewTitle);
    }
#endif

    public void CallFuncFromConsole(string FuncName)
    {
        LuaScript.Call(LuaScript.Globals[FuncName]);
    }

    public void RunLuaFromConsole(string Script)
    {
        LuaScript.DoString(Script);
    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            AddonName = "MainMenu";
        }
        else
        {
            AddonName = PlayerPrefs.GetString("AddonToLoad");
        }
        RawImportedLuaCode = System.IO.File.ReadAllText(@Application.streamingAssetsPath + "/Addons/" + AddonName + "/Code/Main.lua");

        if(IsHalted == false)
        {
            try
            {
                InitLua();
            }
            catch(ScriptRuntimeException ex)
            {  
                GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FF0000>Lua ERROR > " + ex + "<color=#FFFFFF>");
            }
        }
 #if UNITY_STANDALONE_WIN
        EditWindowTitle("Sourcelike |" + GameObject.Find("Console").GetComponent<Console>()._Engine_VersionInfo.VersionString + "| -- " + AddonName);
#endif
    }

    void Update()
    {
        if(IsHalted == false)
        {
            try
            {
                DynValue StartRes = LuaScript.Call(LuaScript.Globals["Loop"]);
            }
            catch(ScriptRuntimeException ex)
            {  
                GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#FF0000>Lua ERROR > " + ex + "<color=#FFFFFF>");
            }
        }
    }
}