using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDirection : MonoBehaviour
{
    public enum Direction {
        north,
        south,
        east,
        west
    }

    public static float GetZRotationByDirection(Direction direction) {
        float z = 0f;
        
        if (direction == Direction.north) {
            z = 0f;
        }
        else if (direction == Direction.south) {
            z = 180f;
        }
        else if (direction == Direction.east) {
            z = 270f;
        }
        else if (direction == Direction.west) {
            z = 90f;
        }

        return z;
    }

    public static Vector3 GetMovementVectorByDirection(Direction direction, float movementSpeed) {
        Vector3 movementVector;

        if (direction == Direction.north) {
            movementVector = new Vector3(0, movementSpeed, 0);
        }
        else if (direction == Direction.south) {
            movementVector = new Vector3(0, -1 * movementSpeed, 0);
        }
        else if (direction == Direction.east) {
            movementVector = new Vector3(movementSpeed, 0, 0);
        }
        else if (direction == Direction.west) {
            movementVector = new Vector3(-1 * movementSpeed, 0, 0);
        }
        else {
            movementVector = new Vector3(0, 0, 0);
        }

        return movementVector;
    }
}
