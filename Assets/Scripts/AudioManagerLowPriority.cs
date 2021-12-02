using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerLowPriority : MonoBehaviour
{
    [Range(0f,1f)]
    [SerializeField] float delay = 0.02f;
    private bool canPlay = true;

    public void PlayAudio(AudioClip clip) {
        if (canPlay) {
            GetComponent<AudioSource>().PlayOneShot(clip);
            if (delay>0) {
                canPlay = false;
                StartCoroutine(Reset());
            }
        }
    }

    private IEnumerator Reset() {
        yield return new WaitForSeconds(delay);
        canPlay = true;
    }
}
