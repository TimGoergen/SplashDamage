using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] TextMeshProUGUI levelDisplay;
    private int scoreIncrement;
    private int score = 0;
    private int comboCount = 0;
    private int maxCombo = 0;
    private int level;
    private bool isGameActive = false;
    GameManager.Difficulty gameDifficulty;

    public void Initialize(GameManager.Difficulty difficulty) {
        score = 0;
        comboCount = 0;
        maxCombo = 0;
        level = 1;
        UpdateScoreDisplay();
        UpdateLevelDisplay();
        gameDifficulty = difficulty;
        SetScoreIncrement();
    }

    private void SetScoreIncrement() {
        if (gameDifficulty == GameManager.Difficulty.easy) {
            scoreIncrement = 7;
        }
        else if (gameDifficulty == GameManager.Difficulty.easy) {
            scoreIncrement = 10;
        }
        else { // game difficulty hard
            scoreIncrement = 15;
        }
    }

    private void OnEnable() {
        EventManager.onSquareCleared += SetScore;
        EventManager.onDropSpent += DropSpent;
        EventManager.onNewLevel += SetNewLevel;
        EventManager.onGameActive += SetGameActive;
        EventManager.onGameInactive += SetGameInactive;
    }

    private void SetGameActive() {
        isGameActive = true;
    }

    private void SetGameInactive() {
        isGameActive = false;
    }

    private void SetNewLevel() {
        if (isGameActive) {
            level++;
            UpdateLevelDisplay();
        }
    }

    private void SetScore() {
        if (isGameActive) {
            comboCount++;

            if (comboCount > maxCombo) { maxCombo = comboCount; }

            // determine if combo meter has reached targets that earn new drops in bucket
            CheckComboTarget();

            // determine what score was earned based on combe meter
            if (comboCount <= 5) {
                score += scoreIncrement;
            }
            else if (comboCount <=10) {
                score += (scoreIncrement * 2);
            }
            else {
                score += (scoreIncrement * 3);
            }
            UpdateScoreDisplay();
        }
    }

    private void CheckComboTarget() {
        List<int> comboTargetList = new List<int> {5,11,18,26,35,45};
        if (comboTargetList.Contains(comboCount)) {
            EventManager.RaiseOnComboEarnsBucketDrop();
        }
    }

    private void DropSpent() {
        if (isGameActive) {
            comboCount = 0;
            UpdateScoreDisplay();
        }
    }

    private void UpdateScoreDisplay() {
        scoreDisplay.text = "Score: " + score.ToString() + "\nCombo: " + comboCount.ToString();
    }

    private void UpdateLevelDisplay() {
        levelDisplay.text = "Level: " + level.ToString();
    }

    public int GetCurrentScore() {
        return score;
    }

    public int GetCurrentLevel() {
        return level;
    }

    public int GetMaxCombo() {
        return maxCombo;
    }
}
