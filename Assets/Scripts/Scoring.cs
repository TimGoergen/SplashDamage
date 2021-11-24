using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoring : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreDisplay;
    [SerializeField] int scoreIncrement = 10;
    private int score = 0;
    private int comboCount = 0;
    private int maxCombo = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreDisplay();
    }

    private void OnEnable() {
        EventManager.onSquareCleared += SetScore;
        EventManager.onDropSpent += DropSpent;
    }

    private void SetScore() {
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

    private void CheckComboTarget() {
        List<int> targetList = new List<int> {5,11,18,26,35,45};
        if (targetList.Contains(comboCount)) {
            EventManager.RaiseOnComboEarnsBucketDrop();
        }
    }

    private void DropSpent() {
        comboCount = 0;
    }

    private void UpdateScoreDisplay() {
        scoreDisplay.text = "Score: " + score.ToString() + "\nCombo: " + comboCount.ToString();
    }

}
