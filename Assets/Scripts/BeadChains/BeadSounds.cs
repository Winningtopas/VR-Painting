using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BeadSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClips;
    AudioSource audioData;
    private float minPitch = .75f;
    private float maxPitch = 1.5f;
    bool firstFramePassed = false;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
        StartCoroutine(WaitForFirstFrameAfterLoad());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!firstFramePassed) return;

        int randomClipIndex = (int)Random.Range(0, audioClips.Length - 1);
        float randomPitch = Random.Range(minPitch, maxPitch);

        audioData.pitch = randomPitch;

        audioData.clip = audioClips[randomClipIndex];
        audioData.Play(0);
    }

    //Wait to start running the script until a frame after loading to avoid the bead sound on startup.
    private IEnumerator WaitForFirstFrameAfterLoad()
    {
        yield return new WaitForEndOfFrame();
        firstFramePassed = true;
    }
}
