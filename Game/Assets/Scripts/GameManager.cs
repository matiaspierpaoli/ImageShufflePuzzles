using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [FormerlySerializedAs("size")] [SerializeField] private SpriteSlicer spriteSlicer;
    
    // void Start()
    // {
    //     spriteSlicer.CreateGamePieces();
    // }
}