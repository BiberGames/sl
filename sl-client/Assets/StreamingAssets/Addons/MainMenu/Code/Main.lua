function Start()
    CLGameObject.CreateEmpty("LuaAudioSource")
    CLGameObject.AddComponent("LuaAudioSource", "Audio.AudioSource")
    RandomBackgroundMusic()
    CreateBackgroundElemants()
    NewImage()
    CLConsole.Log("Start function loaded.")
end

function RandomBackgroundMusic()
    -- is in streamingassets/sounds/music/*.mp3
    MusicTable = CLIO.GetFilesInFolder("Addons/MainMenu/Sounds", "*.mp3")
    -- generates a random 
    MusicRand = math.random(1, #MusicTable)
    
    -- plays the random sound in unity
    CLAudio.PlayAudioFile("LuaAudioSource", MusicTable[MusicRand])
end

function CreateBackgroundElemants()
    -- creates elements needed
    CLGameObject.CreateEmpty("Background")
    CLGameObject.CreateEmpty("BackgroundImage")
    CLGameObject.CreateEmpty("Flash")

    -- parents the elements to the canvas and under the "container"
    CLTransform.SetParent("Background", "MainCanvas")
    CLTransform.SetParent("BackgroundImage", "Background")
    CLTransform.SetParent("Flash", "Background")
    
    -- adds the components
    CLGameObject.AddComponent("Background", "UI.Image")
    CLUI.SetAnchors("Background", 0, 0, 1, 1)

    CLGameObject.AddComponent("BackgroundImage", "UI.Image")
    CLUI.SetAnchors("BackgroundImage", 0, 0, 1, 1)

    CLGameObject.AddComponent("Flash", "UI.Image")
    CLUI.SetAnchors("Flash", 0, 0, 1, 1)

    --sets the index in the hierarchy
    CLTransform.SetSiblingIndex("Background", 0)

    -- resets the scale of the background
    CLTransform.Scale("Background", {1, 1, 1}, 0)
    CLTransform.Position("Background", {0, 0, 0}, 0)

    CLTransform.Scale("Flash", {1, 1, 1}, 0)
    CLTransform.Position("Flash", {0, 0, 0}, 0)

    -- generating some stuff
    CLUI.SetSpriteAlpha("Flash", 0, 2)
end

function NewImage()
    BackgroundTable = CLIO.GetFilesInFolder("Addons/MainMenu/Textures", "*.png")
    BackgroundRand = math.random(1, #BackgroundTable)
    CLUI.SetImage("BackgroundImage", BackgroundTable[BackgroundRand])
end

function UpdateBackground()
	CLTransform.Scale("BackgroundImage", {0.02, 0.02, 0.02}, 1)
    CLTransform.Rotation("BackgroundImage", {0, 0, -0.33}, 1)

    local BackgroundImageScale = CLTransform.GetScale("BackgroundImage")

    if BackgroundImageScale[1] > 1.35 then
        CLUI.SetSpriteAlpha("Flash", 1, 0.0001)
        -- nice...
        NewImage()
        CLTransform.Rotation("BackgroundImage", {0, 0, 0}, 0)
        CLTransform.Scale("BackgroundImage", {1, 1, 1}, 0)
        CLUI.SetSpriteAlpha("Flash", 0, 2)
    end
end

function Loop()
	UpdateBackground()
end