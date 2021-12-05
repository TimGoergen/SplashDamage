
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] Scoring scoreBoard;
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI scoreValuesText;
    [SerializeField] AudioClip sfxGamePaused;

    [SerializeField] GameObject easyToggleButton;
    [SerializeField] GameObject normalToggleButton;
    [SerializeField] GameObject hardToggleButton;

    [Header("Button Sprites")]
    [SerializeField] Sprite selectedDifficultySprite;
    [SerializeField] Sprite unselectedDifficultySprite;

    private AudioManagerHighPriority gamePausedAudio;


    private void Start() {
        gamePausedAudio = GameObject.Find("AudioManagerHighPriority").GetComponent<AudioManagerHighPriority>();
    }

    public void DisplayPauseScreen(GameManager.Difficulty difficulty) {
        this.gameObject.SetActive(true);
        gamePausedAudio.PlayAudio(sfxGamePaused);
        scoreValuesText.text = scoreBoard.GetCurrentScore().ToString() + "\n" + scoreBoard.GetMaxCombo().ToString();
        SetDifficultyToggle(difficulty);
    }

    private void SetDifficultyToggle(GameManager.Difficulty difficulty) {
        Image buttonImage;

        buttonImage = easyToggleButton.GetComponent<Image>();
        if (difficulty == GameManager.Difficulty.easy) {
            buttonImage.sprite = selectedDifficultySprite;
        }
        else {
            buttonImage.sprite = unselectedDifficultySprite;
        }

        buttonImage = normalToggleButton.GetComponent<Image>();
        if (difficulty == GameManager.Difficulty.normal) {
            buttonImage.sprite = selectedDifficultySprite;
        }
        else {
            buttonImage.sprite = unselectedDifficultySprite;
        }

        buttonImage = hardToggleButton.GetComponent<Image>();
        if (difficulty == GameManager.Difficulty.hard) {
            buttonImage.sprite = selectedDifficultySprite;
        }
        else {
            buttonImage.sprite = unselectedDifficultySprite;
        }
    }


    public void OnQuitButtonClick() {
        Application.Quit();
    }

    public void OnResumeButtonClick() {
        Time.timeScale = 1;
        this.gameObject.SetActive(false);
        gameManager.UnpauseGame();
    }

}
