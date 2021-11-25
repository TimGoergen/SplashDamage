using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Behavior")]
    [Range(1f,100f)]
    [SerializeField] float dropSpeed = 20f;
    [SerializeField] float newDropPositionOffset = 3f;

    [Header("Assets")]
    [SerializeField] GameObject drop;

    private DropDirection.Direction direction;

    // Update is called once per frame
    void Update()
    {
        Vector3 movePosition;
        float movementSpeed = dropSpeed * Time.deltaTime;

        if (direction == DropDirection.Direction.north) {
            movePosition = new Vector3(0, movementSpeed, 0);
        }
        else if (direction == DropDirection.Direction.south) {
            movePosition = new Vector3(0, -1 * movementSpeed, 0);
        }
        else if (direction == DropDirection.Direction.east) {
            movePosition = new Vector3(movementSpeed, 0, 0);
        }
        else if (direction == DropDirection.Direction.west) {
            movePosition = new Vector3(-1 * movementSpeed, 0, 0);
        }
        else {
            movePosition = new Vector3(0, 0, 0);
        }

        drop.transform.position += movePosition;
    }

    public void Initialize(DropDirection.Direction dropDirection, Vector3 startingPosition) {
        direction = dropDirection;
        
        Vector3 newPosition = new Vector3(startingPosition.x, startingPosition.y, 0);

        if (direction == DropDirection.Direction.north) {
            newPosition.y += newDropPositionOffset;
        }
        else if (direction == DropDirection.Direction.south) {
            newPosition.y -= newDropPositionOffset;
        }
        else if (direction == DropDirection.Direction.east) {
            newPosition.x += newDropPositionOffset;
        }
        else if (direction == DropDirection.Direction.west) {
            newPosition.x -= newDropPositionOffset;
        }

        drop.transform.position = newPosition;

        float zRotation = DropDirection.GetZRotationByDirection(direction);
        drop.transform.rotation = Quaternion.Euler(0, 0, zRotation);

        EventManager.RaiseOnDropCreated();
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    private void OnDestroy() {
        EventManager.RaiseOnDropDestroyed();
    }
}
