using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] Scoring scoreBoard;
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI congratsText;
    [SerializeField] TextMeshProUGUI scoreValuesText;
    [SerializeField] AudioClip sfxGameOver;
    private AudioManagerHighPriority gameOverAudio;

    private void Start() {
        gameOverAudio = GameObject.Find("AudioManagerHighPriority").GetComponent<AudioManagerHighPriority>();

    }

    public void DisplayGameOverScreen() {
        this.gameObject.SetActive(true);
        gameOverAudio.PlayAudio(sfxGameOver);
        scoreValuesText.text = scoreBoard.GetCurrentScore().ToString() + "\n" + scoreBoard.GetMaxCombo().ToString();
        int level = scoreBoard.GetCurrentLevel();
        congratsText.text = "Game Over!\nYou reached Level " + level;
    }

    public void OnQuitButtonClick() {
        Application.Quit();
    }

    public void OnPlayAgainButtonClick() {
        gameManager.StartGame();
        this.gameObject.SetActive(false);
    }

}
