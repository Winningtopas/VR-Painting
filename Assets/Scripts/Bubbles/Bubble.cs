using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float force;
    [SerializeField]
    private float popDelay = 3f;
    [SerializeField]
    private float scaleDuration = 2f;
    [SerializeField]
    private Vector3 destinationScale = new Vector3(2, 2, 2);

    [SerializeField]
    private bool popTheBubble;

    [SerializeField]
    private AudioClip audioClip;

    [SerializeField]
    private float volume = 1;

    private Vector3 originalScale;
    private AudioSource audioSource;

    private float maxFloatDistance = .75f;
    private float deltaDistance = .3f;
    private Vector3 floatPosition;
    private bool isFloating = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = transform.parent.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (popTheBubble)
        {
            PopBubble();
            popTheBubble = false;
        }
    }

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        rb.velocity = new Vector3(0, force, 0);
        transform.localPosition = Vector3.zero;
        StartCoroutine(CountDown());
    }

    IEnumerator ScaleOverTime(float time)
    {
        float currentTime = 0.0f;

        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime < time);

        // sets the exact value, because Lerp never get's there
        if (currentTime >= time)
        {
            transform.localScale = destinationScale;
            yield return null;
        }
    }

    IEnumerator CountDown()
    {
        StartCoroutine(ScaleOverTime(scaleDuration));

        yield return new WaitForSeconds(popDelay);
        rb.velocity = Vector3.zero;
        isFloating = true;
        floatPosition = transform.position;
        StartCoroutine(BubbleFloat());
    }

    IEnumerator BubbleFloat()
    {
        while (isFloating)
        {
            if (transform.position.y >= floatPosition.y + maxFloatDistance)
            {
                deltaDistance = -deltaDistance;
            }
            if (transform.position.y <= floatPosition.y - maxFloatDistance)
            {
                deltaDistance = -deltaDistance;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * deltaDistance, transform.position.z);
            yield return null;
        }
    }

    private void PopBubble()
    {
        isFloating = false;
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClip);
        GetComponentInParent<BubbleSpawner>().SpawnBubbleShards(transform.position); // spawn the bubble
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            PopBubble();
    }
}
