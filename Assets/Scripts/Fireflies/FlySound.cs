using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FlySound : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audioClipArray;

    [SerializeField]
    private float timeBetweenClips = 0.25f;

    private AudioSource audioSource;
    private float timer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenClips)
        {
            audioSource.PlayOneShot(GetRandomClip());
            timer = 0;
        }
    }

    private AudioClip GetRandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length)];
    }
}
