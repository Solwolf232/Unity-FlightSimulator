using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Player Data SO" , menuName = "Player Data ScriptAble Object")]
public class PlayerDataScript : ScriptableObject
{
    [Header("Player Data")]
     public string Player_Name = "";
     public int Player_Age;
     public string Time_Spent;
     public LevelDiff Level_Diffculty;


    public PlayerDataScript(string player_Name, int player_Age, string time_Spent, LevelDiff level_Diffculty) // Player Data Builder
    {
        Player_Name = player_Name;
        Player_Age = player_Age;
        Time_Spent = time_Spent;
        Level_Diffculty = level_Diffculty;
    }
}
