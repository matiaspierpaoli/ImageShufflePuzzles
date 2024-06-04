using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI victoryText;
    //[SerializeField] private Transform victoryText;

    private void OnEnable()
    {
        gameManager.GameFinishedEvent += OnGameFinished;
    }

    private void OnDisable()
    {
        gameManager.GameFinishedEvent -= OnGameFinished;
    }

    private void OnGameFinished()
    {
        victoryText.gameObject.SetActive(true);
    }
}
