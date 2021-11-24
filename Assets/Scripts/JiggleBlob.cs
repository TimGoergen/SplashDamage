using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiggleBlob : MonoBehaviour
{
    [Header("Configure Jiggle Movement")]
    [Range(0.01f, 1f)]
    [SerializeField] float jiggleMaxMovement = 0.056f;

    [Range(0.001f, 5f)]
    [SerializeField] float jiggleMovement = 0.9f;

    [Range(1, 6)]
    [SerializeField]  int maxJiggles = 3;

    [Range(0.001f, 5f)]
    [SerializeField]  float jiggleMaxMovementDecay = 0.04f;

    [Range(0.0001f, 1f)]
    [SerializeField]  float jiggleMovementDecay = 0.2f;

    private bool isJiggling = false;
    private Vector3 originScale;

    private float totalMovement = 0f;
    private int jiggleCount = 1;
    private bool isJiggleDown = true;
    private float adjustedJiggleMovement;
    private float adjustedJiggleMaxMovement;
    private int jiggleFrame;

    private void Start() {
        originScale = new Vector3 (1f, 1f, 1f);
    }

    private void OnMouseEnter() {
        Jiggle();
    }

    void Update()
    {
        jiggleFrame++;
        adjustedJiggleMovement = jiggleMovement * (1 - (jiggleCount * jiggleMovementDecay)) * Time.deltaTime;
        adjustedJiggleMaxMovement = jiggleMaxMovement * (1 - ((jiggleCount - 1) * jiggleMaxMovementDecay));

        if (isJiggling) {
            if (isJiggleDown && totalMovement < adjustedJiggleMaxMovement) {
                totalMovement += adjustedJiggleMovement;
                transform.localScale -= new Vector3 (adjustedJiggleMovement, adjustedJiggleMovement, adjustedJiggleMovement);
            }
            else {
                isJiggleDown = false;
                totalMovement -= adjustedJiggleMovement;
                transform.localScale += new Vector3 (adjustedJiggleMovement, adjustedJiggleMovement, adjustedJiggleMovement);
            }

            if (transform.localScale.x >= 1f && jiggleCount <= maxJiggles) {
                jiggleCount++;
                isJiggleDown = true;
                totalMovement = 0;
            }
            
            if (jiggleCount > maxJiggles) {
                isJiggling = false;
                transform.localScale = originScale;
                // Debug.Log("Total Jiggle Frames: " + jiggleFrame.ToString());
                // Debug.Log("Final Adjusted Jiggle Movement: " + adjustedJiggleMovement.ToString("0.0000000"));
                // Debug.Log("Final Adjusted Jiggle Max Movement: " + adjustedJiggleMaxMovement.ToString("0.0000000"));
            }
        }
    }

    public void Jiggle() {
        if (!isJiggling) {
            isJiggling = true;
            jiggleCount = 1;
            isJiggleDown = true;
            jiggleFrame = 0;
        }
    }
}
