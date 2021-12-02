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
    [SerializeField] AudioClip sfxBubblePop;
    SpriteRenderer spriteRenderer;
    private ParticleSystem splashEffect;
    private bool isDestroyedByDrop = false;
    private AudioManagerHighPriority blobPopAudio;

    // Start is called before the first frame update
    void Start()
    {
        jiggleBlob = GetComponent<JiggleBlob>();
        splashEffect = GetComponent<ParticleSystem>();
        EventManager.RaiseOnBlobCreated();
        blobPopAudio = GameObject.Find("AudioManagerHighPriority").GetComponent<AudioManagerHighPriority>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Drop") {
            AddDropToBlob();
            splashEffect.Play();
            Destroy(other.gameObject, 0);
            other = null;
            jiggleBlob.Jiggle();
        }
    }
    private void SetBlobSprite() {
        CircleCollider2D blobCollider = GetComponent<CircleCollider2D>();

        if (dropCount == 1) {
            spriteRenderer.sprite = spriteBubbleDropCount1;
            blobCollider.radius = 2.2f;
        }
        else if (dropCount == 2) {
            spriteRenderer.sprite = spriteBubbleDropCount2;
            blobCollider.radius = 2.8f;
        }
        else if (dropCount == 3) {
            spriteRenderer.sprite = spriteBubbleDropCount3;
            blobCollider.radius = 3.3f;
        }
        else if (dropCount == 4) {
            spriteRenderer.sprite = spriteBubbleDropCount4;
            blobCollider.radius = 4f;
        }

    }

    public void ClickBlob() {
        splashEffect.Play();
        AddDropToBlob();
        jiggleBlob.Jiggle();
    }

    private void AddDropToBlob() {
        if (dropCount < 4) {
            dropCount++;
            SetBlobSprite();
        }
        else {
            isDestroyedByDrop = true;
            Destroy(blob.gameObject, 0);
            blob = null;
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
        if (isDestroyedByDrop) {
            blobPopAudio.PlayAudio(sfxBubblePop);

            EventManager.RaiseOnSquareCleared();
            EventManager.RaiseOnBlobDestroyed();
        }
    }


}
