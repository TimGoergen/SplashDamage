using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blob : MonoBehaviour
{
    [Range(1, 4)]
    [SerializeField] int dropCount;
    [SerializeField] GameObject blob;
    [SerializeField] Sprite spriteBubbleDropCount1;
    [SerializeField] Sprite spriteBubbleDropCount2;
    [SerializeField] Sprite spriteBubbleDropCount3;
    [SerializeField] Sprite spriteBubbleDropCount4;
    JiggleBlob jiggleBlob;
    [SerializeField] Drop dropPrefab;
    SpriteRenderer spriteRenderer;
    private ParticleSystem splashEffect;

    // Start is called before the first frame update
    void Start()
    {
        jiggleBlob = GetComponent<JiggleBlob>();
        splashEffect = GetComponent<ParticleSystem>();
        EventManager.RaiseOnBlobCreated();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Drop") {
            AddDropToBlob();
            splashEffect.Play();
            Destroy(other.gameObject, 0);
            jiggleBlob.Jiggle();
        }
    }
    private void SetBlobSprite() {

        if (dropCount == 1) {
            spriteRenderer.sprite = spriteBubbleDropCount1;
        }
        else if (dropCount == 2) {
            spriteRenderer.sprite = spriteBubbleDropCount2;
        }
        else if (dropCount == 3) {
            spriteRenderer.sprite = spriteBubbleDropCount3;
        }
        else if (dropCount == 4) {
            spriteRenderer.sprite = spriteBubbleDropCount4;
        }

    }

    public void ClickBlob() {
        AddDropToBlob();
        splashEffect.Play();
        jiggleBlob.Jiggle();
    }

    private void AddDropToBlob() {
        if (dropCount < 4) {
            dropCount++;
            SetBlobSprite();
        }
        else {
            Destroy(blob.gameObject, 0);
            CreateDropsOnErupt();
        }
    }

    private void CreateDropsOnErupt() {
        CreateDrop(DropDirection.Direction.north, transform.position);
        CreateDrop(DropDirection.Direction.south, transform.position);
        CreateDrop(DropDirection.Direction.east, transform.position);
        CreateDrop(DropDirection.Direction.west, transform.position);
    }

    public void Initialize(int startingDropCount, Vector3 startingPosition) {
        dropCount = startingDropCount;

        Vector3 mousePosition = new Vector3(startingPosition.x, startingPosition.y, 0);
        blob.transform.position = mousePosition;

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;
        
        SetBlobSprite();
    }

    private void CreateDrop(DropDirection.Direction dropDirection, Vector3 startingPosition)
    {
        Transform clonedDrops = GameObject.Find("ClonedDrops").transform;
        Drop drop = Object.Instantiate(dropPrefab, Vector3.zero, Quaternion.identity, clonedDrops).GetComponent<Drop>();
        Vector3 newPosition = new Vector3(startingPosition.x, startingPosition.y, 0);
        //drop.Initialize(dropDirection, Camera.main.ScreenToWorldPoint(newPosition));
        drop.Initialize(dropDirection, newPosition);
    }

    public int GetDropCount() {
        return dropCount;
    }

    private void OnDestroy() {
        EventManager.RaiseOnSquareCleared();
        EventManager.RaiseOnBlobDestroyed();
    }


}
