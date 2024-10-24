using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordManager : MonoBehaviour
{
    public string state = "Selling Ielsek";
    Discord.Discord discord;
    void Start()
    {
        discord = new Discord.Discord(1299052806711414815, (ulong)Discord.CreateFlags.NoRequireDiscord);
        ChangeActivity();
    }

    private void OnDisable() {
        discord.Dispose();
    }

    public void ChangeActivity() {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity {
            State = "",
            Details = state,
            Timestamps = {
                Start = 0
            }
        };

        activityManager.UpdateActivity(activity, (res) => {
            Debug.Log("Activity updated!");
        });
    }

    private void Update() {
        discord.RunCallbacks();
    }
}
