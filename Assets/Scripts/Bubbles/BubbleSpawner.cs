using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public bool spawnedBubble;
    [SerializeField]
    private GameObject bubble;
    [SerializeField]
    private GameObject bubbleShards;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBubble();
    }

    public void SpawnBubble()
    {
        if (!spawnedBubble)
        {
            bubble.SetActive(true);
            bubbleShards.SetActive(false);
        }
    }

    public void SpawnBubbleShards(Vector3 position)
    {
        if (!spawnedBubble)
        {
            bubble.SetActive(false);
            bubbleShards.transform.position = position;
            bubbleShards.SetActive(true);
        }
    }
}