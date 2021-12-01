using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private bool canPlay = true;

    public void PlayAudio(AudioClip clip) {
        if (canPlay) {
            canPlay = false;
            GetComponent<AudioSource>().PlayOneShot(clip);
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset() {
        yield return new WaitForSeconds(0.001f);
        canPlay = true;
    }
}
