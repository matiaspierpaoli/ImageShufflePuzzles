using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    [SerializeField] DifficultySettings difficultySettings;
    [SerializeField] DifficultySettings.Difficulty difficulty;

    public void SetDifficulty()
    {
        difficultySettings.currentDifficulty = difficulty;
    }
}
