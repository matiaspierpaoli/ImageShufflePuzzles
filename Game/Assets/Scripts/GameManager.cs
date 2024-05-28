using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public DifficultySettings difficultySettings;
    public SpriteSlicer spriteSlicer;

    private Transform spriteDisplay;

    private void Start()
    {
        SetRandomSprite();
        spriteSlicer.CreateGamePieces();
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
            int randomIndex = Random.Range(0, selectedArray.Length);
            spriteDisplay = selectedArray[randomIndex];
            spriteSlicer.piecePrefab = spriteDisplay;
        }
        else
        {
            Debug.LogWarning("No sprites available for the selected difficulty");
        }
    }
}