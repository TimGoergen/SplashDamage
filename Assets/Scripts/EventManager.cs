using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // update score when grid square is cleared
    public delegate void OnSquareCleared();
    public static event OnSquareCleared onSquareCleared;

    // remove drop from drop bucket when user clicks on grid
    public delegate void OnDropSpent();
    public static event OnDropSpent onDropSpent;

    // keep track of active flying drops
    public delegate void OnDropCreated();
    public static event OnDropCreated onDropCreated;

    // know when flying drop is destroyed (by colliding with blob or flying off screen)
    public delegate void OnDropDestroyed();
    public static event OnDropDestroyed onDropDestroyed;

    // keep track of when blob is created
    public delegate void OnBlobCreated();
    public static event OnBlobCreated onBlobCreated;

    // know when blob is destroyed (by colliding with drop or user clicks on full blob)
    public delegate void OnBlobDestroyed();
    public static event OnBlobDestroyed onBlobDestroyed;

    // add new drop to bucket when combo meter reaches targets
    public delegate void OnComboEarnsBucketDrop();
    public static event OnComboEarnsBucketDrop onComboEarnsBucketDrop;

    public delegate void OnNewLevel();
    public static event OnNewLevel onNewLevel;

    public delegate void OnGameActive();
    public static event OnGameActive onGameActive;

    public delegate void OnGameInactive();
    public static event OnGameInactive onGameInactive;

    public static void RaiseOnSquareCleared() {
        if (onSquareCleared != null) {
            onSquareCleared();
        }
    }

    public static void RaiseOnDropSpent() {
        if (onDropSpent != null) {
            onDropSpent();
        }
    }

    public static void RaiseOnDropCreated() {
        if (onDropCreated != null) {
            onDropCreated();
        }
    }
    
    public static void RaiseOnDropDestroyed() {
        if (onDropDestroyed != null) {
            onDropDestroyed();
        }
    }
    
    public static void RaiseOnBlobCreated() {
        if (onBlobCreated != null) {
            onBlobCreated();
        }
    }
    
    public static void RaiseOnBlobDestroyed() {
        if (onBlobDestroyed != null) {
            onBlobDestroyed();
        }
    }
    
    public static void RaiseOnComboEarnsBucketDrop() {
        if (onComboEarnsBucketDrop != null) {
            onComboEarnsBucketDrop();
        }
    }
    
    public static void RaiseOnNewLevel() {
        if (onNewLevel != null) {
            onNewLevel();
        }
    }
    
    public static void RaiseOnGameActive() {
        if (onGameActive != null) {
            onGameActive();
        }
    }
    
    public static void RaiseOnGameInactive() {
        if (onGameInactive != null) {
            onGameInactive();
        }
    }
    
}
