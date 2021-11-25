using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    public void OnQuitButtonClick() {
        Application.Quit();
    }

    public void OnPlayGameButtonClick() {
        gameManager.StartGame();
        this.gameObject.SetActive(false);
    }


}