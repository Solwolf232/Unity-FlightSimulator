using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level Diff" , menuName = "Level Diffuclty Data")]


public class LevelDiffData : ScriptableObject
{
    public LevelDiff currentLevelDiff;


}
public enum LevelDiff
{
    None,
    Easy,
    Medium,
    Hard
}