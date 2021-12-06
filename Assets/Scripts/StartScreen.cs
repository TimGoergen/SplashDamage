using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject easyToggleButton;
    [SerializeField] GameObject normalToggleButton;
    [SerializeField] GameObject hardToggleButton;

    [Header("Button Sprites")]
    [SerializeField] Sprite selectedDifficultySprite;
    [SerializeField] Sprite unselectedDifficultySprite;
    
    private GameManager.Difficulty selectedDifficulty = GameManager.Difficulty.normal;

    public void OnQuitButtonClick() {
        Application.Quit();
    }

    public void OnPlayGameButtonClick() {
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
