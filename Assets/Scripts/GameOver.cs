using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] Scoring scoreBoard;
    [SerializeField] TextMeshProUGUI congratsText;
    [SerializeField] TextMeshProUGUI scoreValuesText;

    public void DisplayGameOverScreen() {
        this.gameObject.SetActive(true);
        scoreValuesText.text = scoreBoard.GetCurrentScore().ToString() + "\n" + scoreBoard.GetMaxCombo().ToString();
        int level = scoreBoard.GetCurrentLevel();
        congratsText.text = "Game Over!\nYou reached Level " + level;
    }

    public void OnQuitButtonClick() {
        Application.Quit();
    }

}
