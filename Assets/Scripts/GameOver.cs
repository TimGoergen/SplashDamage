using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Scoring scoreBoard;
    [SerializeField] GameManager gameManager;

    [SerializeField] TextMeshProUGUI congratsText;
    [SerializeField] TextMeshProUGUI scoreValuesText;
    [SerializeField] AudioClip sfxGameOver;
    private AudioManagerHighPriority gameOverAudio;

    [SerializeField] GameObject easyToggleButton;
    [SerializeField] GameObject normalToggleButton;
    [SerializeField] GameObject hardToggleButton;

    [Header("Button Sprites")]
    [SerializeField] Sprite selectedDifficultySprite;
    [SerializeField] Sprite unselectedDifficultySprite;
    
    private GameManager.Difficulty selectedDifficulty;

    private void Start() {
        gameOverAudio = GameObject.Find("AudioManagerHighPriority").GetComponent<AudioManagerHighPriority>();

    }

    public void DisplayGameOverScreen(GameManager.Difficulty difficulty) {
        this.gameObject.SetActive(true);
        selectedDifficulty = difficulty;
        SetDifficultyToggle();
        gameOverAudio.PlayAudio(sfxGameOver);
        scoreValuesText.text = scoreBoard.GetCurrentScore().ToString() + "\n" + scoreBoard.GetMaxCombo().ToString();
        int level = scoreBoard.GetCurrentLevel();
        congratsText.text = "Game Over!\nYou reached Level " + level;
    }

    private void SetDifficultyToggle() {
        Image buttonImage;

        buttonImage = easyToggleButton.GetComponent<Image>();
        if (selectedDifficulty == GameManager.Difficulty.easy) {
            buttonImage.sprite = selectedDifficultySprite;
        }
        else {
            buttonImage.sprite = unselectedDifficultySprite;
        }

        buttonImage = normalToggleButton.GetComponent<Image>();
        if (selectedDifficulty == GameManager.Difficulty.normal) {
            buttonImage.sprite = selectedDifficultySprite;
        }
        else {
            buttonImage.sprite = unselectedDifficultySprite;
        }

        buttonImage = hardToggleButton.GetComponent<Image>();
        if (selectedDifficulty == GameManager.Difficulty.hard) {
            buttonImage.sprite = selectedDifficultySprite;
        }
        else {
            buttonImage.sprite = unselectedDifficultySprite;
        }
    }

    public void OnQuitButtonClick() {
        Application.Quit();
    }

    public void OnPlayAgainButtonClick() {
        gameManager.StartGame(selectedDifficulty);
        this.gameObject.SetActive(false);
    }

    public void OnEasyToggleClick() {
        selectedDifficulty = GameManager.Difficulty.easy;

        Image buttonImage;
        buttonImage = easyToggleButton.GetComponent<Image>();
        buttonImage.sprite = selectedDifficultySprite;

        buttonImage = normalToggleButton.GetComponent<Image>();
        buttonImage.sprite = unselectedDifficultySprite;

        buttonImage = hardToggleButton.GetComponent<Image>();
        buttonImage.sprite = unselectedDifficultySprite;        
    }

    public void OnNormalToggleClick() {
        selectedDifficulty = GameManager.Difficulty.normal;

        Image buttonImage;
        buttonImage = easyToggleButton.GetComponent<Image>();
        buttonImage.sprite = unselectedDifficultySprite;

        buttonImage = normalToggleButton.GetComponent<Image>();
        buttonImage.sprite = selectedDifficultySprite;

        buttonImage = hardToggleButton.GetComponent<Image>();
        buttonImage.sprite = unselectedDifficultySprite;        
    }

    public void OnHardToggleClick() {
        selectedDifficulty = GameManager.Difficulty.hard;

        Image buttonImage;
        buttonImage = easyToggleButton.GetComponent<Image>();
        buttonImage.sprite = unselectedDifficultySprite;

        buttonImage = normalToggleButton.GetComponent<Image>();
        buttonImage.sprite = unselectedDifficultySprite;

        buttonImage = hardToggleButton.GetComponent<Image>();
        buttonImage.sprite = selectedDifficultySprite;        
    }


}
