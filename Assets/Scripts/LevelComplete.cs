using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] Scoring scoreBoard;
    [SerializeField] GameManager gameManager;
    [SerializeField] TextMeshProUGUI congratsText;
    [SerializeField] TextMeshProUGUI scoreValuesText;

    public void DisplayLevelCompleteScreen() {
        this.gameObject.SetActive(true);
        scoreValuesText.text = scoreBoard.GetCurrentScore().ToString() + "\n" + scoreBoard.GetMaxCombo().ToString();
        int level = scoreBoard.GetCurrentLevel();
        congratsText.text = "Congratulations!\nLevel " + level + " Complete";
    }

    public void OnStartLevelButtonClick() {
        gameManager.LoadNewLevel();
        this.gameObject.SetActive(false);
    }

    public void OnQuitButtonClick() {
        Application.Quit();
    }

}
