using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public DifficultySettings difficultySettings;
    public SpriteSlicer spriteSlicer;
    public SpriteMover spriteMover;

    public event Action GameFinishedEvent;

    private Transform spriteDisplay;

    private void OnEnable()
    {
        spriteMover.MovementCompletedEvent += OnMovementCompleted;
    }

    private void OnDisable()
    {
        spriteMover.MovementCompletedEvent -= OnMovementCompleted;
    }

    private void Start()
    {
        SetRandomSprite();
        spriteSlicer.CreateGamePieces();
        spriteSlicer.DuplicateGameboard();

        spriteMover.Shuffle();
    }

    public void SetRandomSprite()
    {
        Transform[] selectedArray = null;

        switch (difficultySettings.currentDifficulty)
        {
            case DifficultySettings.Difficulty.Easy:
                selectedArray = difficultySettings.easySprites;
                spriteSlicer.gridSize = difficultySettings.easyGridSize;
                break;
            case DifficultySettings.Difficulty.Medium:
                selectedArray = difficultySettings.mediumSprites;
                spriteSlicer.gridSize = difficultySettings.mediumGridSize;
                break;
            case DifficultySettings.Difficulty.Hard:
                selectedArray = difficultySettings.hardSprites;
                spriteSlicer.gridSize = difficultySettings.hardGridSize;
                break;
        }

        if (selectedArray != null && selectedArray.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, selectedArray.Length);
            spriteDisplay = selectedArray[randomIndex];
            spriteSlicer.piecePrefab = spriteDisplay;
        }
        else
        {
            Debug.LogWarning("No sprites available for the selected difficulty");
        }
    }

    private void OnMovementCompleted()
    {
        if (spriteMover.CheckCompletion())
        {
            GameFinishedEvent?.Invoke();
            spriteMover.ToggleMovementAllowance(false);
        }
    }
}