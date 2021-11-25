using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridIndex {
    public int rowIndex;
    public int columnIndex;
    public bool isOnBoard;
    
    public GridIndex(int x, int y) {
        rowIndex = x;
        columnIndex = y;
        if (x<0 || y<0) {
            isOnBoard = false;
        }
        else {
            isOnBoard = true;
        }
    }

    public override string ToString() {
        string indexString = "[" + rowIndex.ToString() + "," + columnIndex.ToString() + "]";
        return indexString;
    }
}


public class GameBoard
{
    int gridSize;
    public Blob[,] grid;
    public Blob blobPrefab;
    float gridLeft;
    float gridTop;
    float gridHeight;
    float gridWidth;
    float gridSquareWidth;
    float gridSquareHeight;
    float borderSize = 0.75f;
    int borderCornerVertices = 20;
    private GameObject borderLinePrefab;
    private System.Random randDrops;
    private List<GameObject> borderLines;



    public float GetSquareHeight() {
        return gridSquareHeight;
    }

    public float GetSquareWidth() {
        return gridSquareWidth;
    }

    public void ClearExistingScreenObjects() {
        for (int row = 0; row<gridSize; row++) {
            for (int col = 0; col<gridSize; col++) {
                if (grid[row,col] != null) {
                    UnityEngine.Object.Destroy(grid[row,col].gameObject);
                }
            }
        }

        for (int i=0; i<borderLines.Count; i++) {
            if (borderLines[i].gameObject != null) {
                UnityEngine.Object.Destroy(borderLines[i].gameObject);
            }
        }
    }

    public GameBoard(int size, Blob prefab, GameObject borderPrefab, Vector3 topLeft, Vector3 bottomRight) {
        blobPrefab = prefab;
        borderLinePrefab = borderPrefab;
        int dropCount;
        Blob newBlob;

        gridLeft = topLeft.x;
        gridTop = topLeft.y;
        gridHeight = topLeft.y - bottomRight.y;
        gridWidth = bottomRight.x - topLeft.x;
        gridSquareWidth = (gridWidth / size);
        gridSquareHeight = (gridHeight / size);
        gridSize = size;

        Vector3 gridCenterPosition; // I need to calculate the center of each grid square

        grid = new Blob[size,size];
        int blobCount = 0;
        int cellCount = 0;
        randDrops = new System.Random();

        for (int rowIndex=0; rowIndex<size; rowIndex++) {
            for (int colIndex=0; colIndex<size; colIndex++) {
                cellCount++;
                dropCount = GetRandomDropCount();
                if (blobCount < (cellCount / 2.0f) && dropCount == 0) {
                    dropCount++;
                }
                if (dropCount > 0) {
                    GridIndex index = new GridIndex(rowIndex, colIndex); 
                    gridCenterPosition = GetGridSquareCenter(index);
                    newBlob = CreateBlob(dropCount, gridCenterPosition);
                    grid[rowIndex,colIndex] = newBlob;
                    blobCount++;
                }
            }
        }

        DrawGridLines(gridSize, topLeft, bottomRight);
    }

    private int GetRandomDropCount() {
        int randomInt = randDrops.Next(1, 100);
        int adjustedDropCount = 0;

        if (randomInt <= 20) { adjustedDropCount = 0; }
        else if (randomInt <= 35) { adjustedDropCount = 1; }
        else if (randomInt <= 55) { adjustedDropCount = 2; }
        else if (randomInt <= 75) { adjustedDropCount = 3; }
        else { adjustedDropCount = 4; }

        return adjustedDropCount;
    }

    public void ClickGrid(GridIndex index) {
        if (grid[index.rowIndex, index.columnIndex] == null) {
            Vector3 newBlobPosition = GetGridSquareCenter(index);
            Blob newBlob = CreateBlob(1, newBlobPosition);
            grid[index.rowIndex, index.columnIndex] = newBlob;
        }
        else {
            grid[index.rowIndex, index.columnIndex].ClickBlob();
        }
    }

    public Vector3 GetGridSquareCenter(GridIndex gridIndex) {
        float gridCenterX = gridLeft + (gridSquareWidth * gridIndex.columnIndex) + (gridSquareWidth / 2f);
        float gridCenterY = (gridTop - gridHeight) + (gridSquareHeight * gridIndex.rowIndex) + (gridSquareHeight / 2f);
        Vector3 gridCenterPosition = new Vector3(gridCenterX, gridCenterY, 0);
        return gridCenterPosition;
    }

    public GridIndex GetGridIndexByPosition(Vector3 gridPosition) {
        GridIndex gridIndex;
        int gridColumn = -1;
        int gridRow = -1;

        float adjustedGridX = gridPosition.x + (gridWidth / 2);
        float adjustedGridY = gridPosition.y + (gridHeight / 2);

        // identify row if in grid
        if (adjustedGridX >= 0 && adjustedGridX <= gridWidth) {
            gridColumn = (int)Math.Floor(adjustedGridX / gridSquareWidth);
        }

        // identify column if in grid
        if (adjustedGridY >= 0 && adjustedGridY <= gridHeight) {
            gridRow = (int)Math.Floor(adjustedGridY / gridSquareHeight);
        }

        gridIndex = new GridIndex(gridRow, gridColumn);

        return gridIndex;
    }

    private Blob CreateBlob(int startingDropCount, Vector3 startingPosition)
    {
        Transform clonedBlobs = GameObject.Find("ClonedBlobs").transform;
        Blob blob = UnityEngine.Object.Instantiate(blobPrefab, Vector3.zero, Quaternion.identity, clonedBlobs).GetComponent<Blob>();
        blob.Initialize(startingDropCount, startingPosition);
        return blob;
    }

    private GameObject CreateBorderLine()
    {
        Transform clonedLines = GameObject.Find("ClonedLines").transform;
        GameObject myLine = UnityEngine.Object.Instantiate(borderLinePrefab, Vector3.zero, Quaternion.identity, clonedLines);
        return myLine;
    }

    private void DrawLines(Vector3[] positions, int positionCount) {
        GameObject myLine = CreateBorderLine();
        myLine.transform.position = new Vector3(0,0,0);
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startWidth = borderSize;
        lr.endWidth = borderSize;
        lr.numCornerVertices = borderCornerVertices;
        lr.positionCount = positionCount;
        lr.SetPositions(positions);
        borderLines.Add(myLine);
     }

    private void DrawGridLines(int gridSize, Vector3 topLeft, Vector3 bottomRight)
    {
        // draw outside border
        Vector3[] positions = new Vector3[5];
        borderLines = new List<GameObject>();

        positions[0] = topLeft;
        positions[1] = new Vector3(bottomRight.x, topLeft.y);
        positions[2] = bottomRight;
        positions[3] = new Vector3(topLeft.x, bottomRight.y);
        positions[4] = topLeft;
        DrawLines(positions, 5);

        positions = new Vector3[2];
        for (int r=1; r<gridSize; r++) {
            // line between rows
            positions[0] = new Vector3(topLeft.x, topLeft.y - (gridSquareHeight*r));
            positions[1] =  new Vector3(bottomRight.x, topLeft.y - (gridSquareHeight*r));
            DrawLines(positions, 2);
        }

        for (int c=1; c<gridSize; c++) {
            // line between columns
            positions[0] = new Vector3(topLeft.x + (gridSquareWidth*c), topLeft.y);
            positions[1] =  new Vector3(topLeft.x + (gridSquareWidth*c), bottomRight.y);
            DrawLines(positions, 2);
        }
    }

}
