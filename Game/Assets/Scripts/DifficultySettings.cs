using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "ScriptableObjects/DifficultySettings", order = 1)]
public class DifficultySettings : ScriptableObject
{
    public enum Difficulty { Easy, Medium, Hard}
    public Difficulty currentDifficulty;

    public Transform[] easySprites;
    public Transform[] mediumSprites;
    public Transform[] hardSprites;

    public int easyGridSize;
    public int mediumGridSize;
    public int hardGridSize;
}
