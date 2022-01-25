using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall : MonoBehaviour
{
    [SerializeField]
    private float minPosX = -2f, maxPosX = 2f;

    private ObjectPooler objectPooler;

    private Vector3 minPos;
    private Vector3 maxPos;

    [SerializeField]
    private int spawnRate = 2;

    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        minPos = new Vector3(minPosX, transform.position.y, transform.position.z);
        maxPos = new Vector3(maxPosX, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < spawnRate; i++)
        {
            float xOffset = Random.Range(minPosX, maxPosX);
            Vector3 rotatedXOffset = new Vector3(xOffset, 0, 0);
            rotatedXOffset = transform.rotation * rotatedXOffset;
            Vector3 position = transform.position + rotatedXOffset;
            objectPooler.SpawnFromPool("Waterfall", position, transform.rotation, transform);
        }
    }

    void OnDrawGizmosSelected()
    {
        minPos = transform.position + transform.rotation * new Vector3(minPosX, 0, 0);
        maxPos = transform.position + transform.rotation * new Vector3(maxPosX, 0, 0);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(minPos, maxPos);
    }
}
