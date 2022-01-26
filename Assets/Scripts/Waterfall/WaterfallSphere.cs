using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallSphere : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private Vector2 forceX = new Vector2(0f, 10f);
    [SerializeField]
    private Vector2 forceY = new Vector2(0f, 10f);
    [SerializeField]
    private Vector2 forceZ = new Vector2(0f, 10f);
    [SerializeField]
    private Rigidbody rb;

    private Vector3 originalScale;

    // Start is called before the first frame update
    private void Start()
    {
        originalScale = new Vector3(1,1,1);

        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }
    public void OnObjectSpawn()
    {
        transform.localScale = originalScale;

        float xSpeed = Random.Range(forceX.x, forceX.y);
        float ySpeed = Random.Range(forceY.x, forceY.y);
        float zSpeed = Random.Range(forceZ.x, forceZ.y);

        Vector3 force = transform.rotation * new Vector3(xSpeed, ySpeed, zSpeed);
        rb.velocity = force;
    }
}
