using UnityEngine;
using Discord;
using System;

public class DiscordManager : MonoBehaviour
{
    public Discord.Discord _Discord;

    void Awake()
    {
        if (!Application.isEditor)
        {
            try
            {
                _Discord = new Discord.Discord(1005901649698619493, (System.UInt64)Discord.CreateFlags.Default);
                var ActivityManager = _Discord.GetActivityManager();
                var Activity = new Discord.Activity {
                    Details = "Wasting my time.",
                    State = "alone.",
                    Assets = {
                        LargeImage = "logo_4x",
                    }
                };
                ActivityManager.UpdateActivity(Activity, (res) => {
                    if(res == Discord.Result.Ok)
                    {
                        GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#7289da>Discord > Connected to Discord!<color=#FFFFFF>");
                    }
                    else
                    {
                        GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#7289da>Discord > Connection to Discord failed :(<color=#FFFFFF>");
                    }
                });
            }
            catch (Exception e)
            {
                GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#7289da>Discord > Connection to Discord failed :(<color=#FFFFFF>");
            }
        }
        else
        {
            GameObject.Find("Console").GetComponent<Console>().AddLine("\n<color=#7289da>Discord > Running in editor not connecting.<color=#FFFFFF>");
        }
    }

    void Update()
    {
        if (!Application.isEditor)
        {
            try
            {
                _Discord.RunCallbacks();
            }
            catch (Exception e)
            {
                Debug.Log("");
            }
        }
    }
}
