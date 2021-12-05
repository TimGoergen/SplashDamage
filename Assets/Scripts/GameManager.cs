using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum Difficulty {
        easy,
        normal,
        hard
    }

    [Header("Assets")]
    [SerializeField] Shader newShaderSurface;
    [SerializeField] GameObject highlightPanel;
    [SerializeField] GameObject borderLinePrefab;
    [SerializeField] GameObject scoreBubblePrefab;
    [SerializeField] Scoring scoreBoard;
    [SerializeField] TextMeshProUGUI dropCountDisplay;
    [SerializeField] PulseGameObject dropCountDisplayUI;
    [SerializeField] AudioClip sfxBonusDrop;
    public Blob blobPrefab;

    [Header("Screens")]
    [SerializeField] StartScreen startScreen;
    [SerializeField] LevelComplete levelCompleteScreen;
    [SerializeField] GameOver gameOverScreen;
    [SerializeField] PauseScreen pauseScreen;

    GameBoard gameBoard;

    float panelDisplayLength = 0.00001f;
    float panelDisplayTime = 0f;

    private List<GameObject> dropBucket;
    private int dropsInBucket;

    private int activeFlyingDropCount = 0;
    private int activeBlobCount = 0;
    private Vector3 defaultNewBucketDropLocation = new Vector3(68f,45f,0);
    private int gridSquareSize = 12;
    private System.Random rand = new System.Random();

    private AudioManagerHighPriority bonusDropAudio;
    
    [HideInInspector]
    public Difficulty gameDifficulty;



    private void OnEnable() {
        EventManager.onDropCreated += IncrementActiveFlyingDropCount;
        EventManager.onBlobCreated += IncrementActiveBlobCount;

        EventManager.onDropDestroyed += DecrementActiveFlyingDropCount;
        EventManager.onBlobDestroyed += DecrementActiveBlobCount;

        EventManager.onComboEarnsBucketDrop += AddDropToBucket;
    }

    private void CheckGridCleared() {
        if (activeFlyingDropCount == 0 && dropsInBucket == 0 && activeBlobCount > 0) {
            Debug.Log("Drops In Bucket: " + dropsInBucket.ToString() 
                    + ", Flying Drops Count: " + activeFlyingDropCount.ToString() 
                    + ", Blobs Remaining: " + activeBlobCount.ToString());
            this.gameObject.layer = 2;
            EventManager.RaiseOnGameInactive();
            gameOverScreen.DisplayGameOverScreen(gameDifficulty);
        }
        else if (activeFlyingDropCount == 0 && activeBlobCount == 0) {
            Debug.Log("Drops In Bucket: " + dropsInBucket.ToString() 
                    + ", Flying Drops Count: " + activeFlyingDropCount.ToString() 
                    + ", Blobs Remaining: " + activeBlobCount.ToString());
            this.gameObject.layer = 2;
            EventManager.RaiseOnGameInactive();
            levelCompleteScreen.DisplayLevelCompleteScreen();
        }
    }

    public void LoadNewLevel() {
        this.gameObject.layer = 0;
        ClearGameBoard();
        EventManager.RaiseOnGameActive();
        EventManager.RaiseOnNewLevel();
        int currentLevel = scoreBoard.GetCurrentLevel();
        int boardSize = GetGameBoardSize(currentLevel);
        LoadGameBoard(boardSize);
        AddDropToBucket();
    }

    private void DecrementActiveFlyingDropCount() {
        activeFlyingDropCount--;
        CheckGridCleared();
    }
    
    private void DecrementActiveBlobCount() {
        activeBlobCount--;
        CheckGridCleared();
    }
    
    private void IncrementActiveFlyingDropCount() {
        activeFlyingDropCount++;
        CheckGridCleared();
    }
    
    private void IncrementActiveBlobCount() {
        activeBlobCount++;
        CheckGridCleared();
    }

    private void Start() {
        startScreen.gameObject.SetActive(true);
        pauseScreen.gameObject.SetActive(false);
        gameOverScreen.gameObject.SetActive(false);
        levelCompleteScreen.gameObject.SetActive(false);

        Screen.fullScreen = !Screen.fullScreen;
    }
    
    // Start is called before the first frame update
    public void StartGame(Difficulty selectedDifficulty)
    {
        gameDifficulty = selectedDifficulty;
        this.gameObject.layer = 0;
        ClearGameBoard();

        int boardSize = GetGameBoardSize(1);

        LoadGameBoard(boardSize);
        InitializeDropBucket();
        EventManager.RaiseOnGameActive();
        activeBlobCount = 0;
        scoreBoard.Initialize(gameDifficulty);
        UpdateDropCountDisplay();
        bonusDropAudio = GameObject.Find("AudioManagerHighPriority").GetComponent<AudioManagerHighPriority>();
    }

    private void UpdateDropCountDisplay() {
        dropCountDisplay.text = "Drops: " + dropsInBucket.ToString();
        dropCountDisplayUI.Pulse();
    }

    private void ClearGameBoard()
    {
        if (gameBoard != null) {
            gameBoard.ClearExistingScreenObjects();
        }
    }

    private int GetGameBoardSize(int currentLevel)
    {
        int size = 0;
        if (currentLevel <= 3) {
            size = currentLevel + 4;
        }
        else {
            int randInt = rand.Next(1,100);
            if (randInt < 15) {
                size = 4;
            }
            else if (randInt < 35) {
                size = 5;
            }
            else if (randInt < 55) {
                size = 7;
            }
            else {
                size = 6;
            }
        }
        return size;
    }

    private void InitializeDropBucket() {
        // destroy any existing drop bucket game objects
        if (dropBucket != null) {
            for (int i=0; i<dropBucket.Count; i++) {
                Destroy(dropBucket[i].gameObject);
                dropBucket[i] = null;
            }
        }

        dropsInBucket = GetStartingDropCount();
        FillDropBucket(dropsInBucket);
    }

    private int GetStartingDropCount() {
        int dropCount;

        if (gameDifficulty == Difficulty.easy) {
            dropCount = 15;
        }
        else if (gameDifficulty == Difficulty.normal) {
            dropCount = 10;
        }
        else {
            dropCount = 7;
        }
        return dropCount;
    }

    private void LoadGameBoard(int boardSize)
    {
        float gridWidth = (gridSquareSize * boardSize / 2);

        Vector3 topLeft = new Vector3(-gridWidth, gridWidth);
        Vector3 bottomRight = new Vector3(gridWidth, -gridWidth);
        gameBoard = new GameBoard(boardSize, blobPrefab, borderLinePrefab, topLeft, bottomRight, gameDifficulty);

        float boardHeight = gameBoard.GetSquareHeight();
        float boardWidth = gameBoard.GetSquareWidth();

        Vector3 panelSize = new Vector3(boardWidth, boardHeight, 1);
        highlightPanel.transform.localScale = panelSize;
    }

    private void AddDropToBucket() {
        dropsInBucket++;
        bonusDropAudio.PlayAudio(sfxBonusDrop);
        GameObject scoreBubble = CreateScoreBubble(defaultNewBucketDropLocation);
        dropBucket.Add(scoreBubble);
        UpdateDropCountDisplay();
    }

    private void FillDropBucket(int startingDrops)
    {
        System.Random rand = new System.Random();

        dropBucket = new List<GameObject>();
        for (int i=0; i<startingDrops; i++) {
            float adjustedX = defaultNewBucketDropLocation.x + ((float)rand.NextDouble() * 6f - 3f);
            float adjustedY = defaultNewBucketDropLocation.y + (i*2f);

            Vector3 location = new Vector3(adjustedX, adjustedY, 0);
            GameObject scoreBubble = CreateScoreBubble(location);
            dropBucket.Add(scoreBubble);
        }
    }

    private void UseDropFromBucket(int dropRemoved) {
        if (dropRemoved >= 0) {
            Destroy(dropBucket[dropRemoved-1].gameObject);
            dropBucket.RemoveAt(dropRemoved-1);
            dropsInBucket--;
            CheckGridCleared();
            UpdateDropCountDisplay();
        }
        else {
            Debug.Log("You've got no drops left!");
        }
    }

    // Update is called once per frame
    void Update() {
        if (panelDisplayTime <= 0) {
            highlightPanel.SetActive(false);
        }
        else {
            panelDisplayTime -= (panelDisplayLength * 5 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Escape)) {
            Time.timeScale = 0;
            this.gameObject.layer = 2;
            pauseScreen.DisplayPauseScreen(gameDifficulty);
        }
    }

    public void UnpauseGame() {
        this.gameObject.layer = 0;
        Time.timeScale = 1;
    }

    public GameObject GetBorderLinePrefab() {
        return borderLinePrefab;
    }

    private void OnMouseDown()
    {
        Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GridIndex gridIndex = gameBoard.GetGridIndexByPosition(cameraPosition);

        //Debug.Log("Camera Position is: " + cameraPosition.ToString());
        Debug.Log("Grid Index is: " + gridIndex.ToString());
        if (gridIndex.isOnBoard && dropsInBucket > 0) {
            Vector3 gridSquareCenter = gameBoard.GetGridSquareCenter(gridIndex);
            ShowHighlightPanel(gridSquareCenter);

            gameBoard.ClickGrid(gridIndex);

            UseDropFromBucket(dropsInBucket);
            EventManager.RaiseOnDropSpent();
        }
        //Debug.Log("Mouse Position: " + Input.mousePosition.ToString() + "\nCamera Position: " + Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void ShowHighlightPanel(Vector3 panelPosition) {
        // Debug.Log("Display Highlight Panel at: " + panelPosition.ToString());
        highlightPanel.transform.position = panelPosition;
        highlightPanel.SetActive(true);
        panelDisplayTime = panelDisplayLength;
    }

    private void CreateBlob(int startingDropCount, Vector3 currentMousePosition)
    {
        Transform clonedBlobs = GameObject.Find("ClonedBlobs").transform;
        Blob blob = Object.Instantiate(blobPrefab, Vector3.zero, Quaternion.identity, clonedBlobs).GetComponent<Blob>();
        Vector3 newPosition = new Vector3(currentMousePosition.x, currentMousePosition.y, 0);
        blob.Initialize(startingDropCount, Camera.main.ScreenToWorldPoint(newPosition));
    }

    private GameObject CreateScoreBubble(Vector3 location)
    {
        Transform clonedScoreBubbles = GameObject.Find("ClonedScoreBubbles").transform;
        GameObject scoreBubble = Object.Instantiate(scoreBubblePrefab, location, Quaternion.identity, clonedScoreBubbles);
        return scoreBubble;
    }

}
