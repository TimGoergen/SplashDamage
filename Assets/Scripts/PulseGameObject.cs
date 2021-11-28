using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseGameObject : MonoBehaviour
{
    [Header("Configure Pulse Movement")]
    [Range(0.01f, 2f)]
    [SerializeField] float pulseMaxMovement = 0.05f;

    [Range(0.001f, 5f)]
    [SerializeField] float pulseMovement = 0.2f;

    [Range(0, 50)]
    [SerializeField]  int maxPulses = 0;

    [SerializeField]  bool pulseOnStart = false;

    private bool isPulsing = false;
    private Vector3 originScale;

    private float totalMovement = 0f;
    private int currentPulseCount = 1;
    private bool isPulseUp = true;
    private float adjustedPulseMovement;
    private float adjustedPulseMaxMovement;
    private int pulseFrame;

    private void Start() {
        originScale = this.transform.localScale;

        if (pulseOnStart) {
            Pulse();
        }
    }

    void Update()
    {
        pulseFrame++;
        adjustedPulseMovement = pulseMovement * Time.deltaTime;

        if (isPulsing) {
            if (isPulseUp && totalMovement < pulseMaxMovement) {
                totalMovement += adjustedPulseMovement;
                transform.localScale += new Vector3 (adjustedPulseMovement, adjustedPulseMovement, adjustedPulseMovement);
            }
            else {
                isPulseUp = false;
                totalMovement -= adjustedPulseMovement;
                transform.localScale -= new Vector3 (adjustedPulseMovement, adjustedPulseMovement, adjustedPulseMovement);
            }

            if (transform.localScale.x <= originScale.x) {
                currentPulseCount++;
                isPulseUp = true;
                totalMovement = 0;
            }
            
            if (currentPulseCount > maxPulses && maxPulses > 0) {
                isPulsing = false;
                transform.localScale = originScale;
                // Debug.Log("Total Jiggle Frames: " + jiggleFrame.ToString());
                // Debug.Log("Final Adjusted Jiggle Movement: " + adjustedJiggleMovement.ToString("0.0000000"));
                // Debug.Log("Final Adjusted Jiggle Max Movement: " + adjustedJiggleMaxMovement.ToString("0.0000000"));
            }
        }
    }

    public void Pulse() {
        if (!isPulsing) {
            isPulsing = true;
            currentPulseCount = 1;
            isPulseUp = true;
            pulseFrame = 0;
        }
    }
}
