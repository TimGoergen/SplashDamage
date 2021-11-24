using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game")]
    [Range(2,8)]
    [SerializeField] int gameBoardSize;
    [Range(2,20)]
    [SerializeField] int startingDrops = 10;

    [Header("Assets")]
    [SerializeField] Shader newShaderSurface;
    [SerializeField] GameObject highlightPanel;
    [SerializeField] GameObject borderLinePrefab;
    [SerializeField] GameObject scoreBubblePrefab;
    public Blob blobPrefab;

    GameBoard gameBoard;

    float panelDisplayLength = 0.00001f;
    float panelDisplayTime = 0f;

    private List<GameObject> dropBucket;
    private int dropsInBucket;

    private int activeFlyingDropCount = 0;
    private int activeBlobCount = 0;
    private Vector3 defaultNewBucketDropLocation = new Vector3(68f,45f,0);


    private void OnEnable() {
        EventManager.onDropCreated += IncrementActiveFlyingDropCount;
        EventManager.onBlobCreated += IncrementActiveBlobCount;

        EventManager.onDropDestroyed += DecrementActiveFlyingDropCount;
        EventManager.onBlobDestroyed += DecrementActiveBlobCount;

        EventManager.onComboEarnsBucketDrop += AddDropToBucket;
    }

    private void CheckGridCleared() {
        Debug.Log("Drops In Bucket: " + dropsInBucket.ToString() 
                + ", Flying Drops Count: " + activeFlyingDropCount.ToString() 
                + ", Blobs Remaining: " + activeBlobCount.ToString());
        if (activeFlyingDropCount == 0 && dropsInBucket == 0) {
            Debug.Log("YOU RAN OUT OF DROPS");
        }
        else if (activeFlyingDropCount == 0 && activeBlobCount == 0) {
            Debug.Log("YOU WIN!");
        }
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
    
    // Start is called before the first frame update
    void Start() {
        int gridSquareSize = 12;
        float gridWidth = (gridSquareSize * gameBoardSize / 2);

        Vector3 topLeft = new Vector3(-gridWidth, gridWidth);
        Vector3 bottomRight = new Vector3(gridWidth, -gridWidth);
        gameBoard = new GameBoard(gameBoardSize, blobPrefab, borderLinePrefab, topLeft, bottomRight);

        float boardHeight = gameBoard.GetSquareHeight();
        float boardWidth = gameBoard.GetSquareWidth();
        
        Vector3 panelSize = new Vector3(boardWidth, boardHeight, 1);
        highlightPanel.transform.localScale = panelSize;

        dropsInBucket = startingDrops;
        FillDropBucket(startingDrops);
    }

    private void AddDropToBucket() {
        dropsInBucket++;
        GameObject scoreBubble = CreateScoreBubble(defaultNewBucketDropLocation);
        dropBucket.Add(scoreBubble);
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
            Destroy(dropBucket[dropRemoved].gameObject);
            dropBucket.RemoveAt(dropRemoved);
            CheckGridCleared();
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

            dropsInBucket--;
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
