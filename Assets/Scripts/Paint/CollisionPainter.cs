using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CollisionPainter : MonoBehaviour
{
    public Brush brush;
    public Material paintMaterial;

    [SerializeField]
    private bool breakOnCollision;
    private bool isShrinking;


    private void OnEnable()
    {
        isShrinking = false;
    }

    private void Start()
    {
        if (paintMaterial == null)
            paintMaterial = GetComponent<MeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Paint")
        {
            paintMaterial.color = other.GetComponent<MeshRenderer>().material.color;
            //PaintTarget.PaintObject(GetComponent<PaintTarget>(), Vector3.zero, Vector3.zero, brush, paintMaterial.color, true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            PaintTarget paintTarget = contact.otherCollider.GetComponent<PaintTarget>();
            if (paintTarget != null)
            {
                PaintTarget.PaintObject(paintTarget, contact.point, contact.normal, brush, paintMaterial.color, false);
            }
        }
        if (breakOnCollision && !isShrinking)
        {
            isShrinking = true;
            //StartCoroutine(ScaleDown(Vector3.zero, .1f));
        }

        if (collision.gameObject.tag == "Paint")
        {
            paintMaterial.color = collision.gameObject.GetComponent<MeshRenderer>().material.color;
            //PaintTarget.PaintObject(GetComponent<PaintTarget>(), Vector3.zero, Vector3.zero, brush, paintMaterial.color, true);
        }
    }

    IEnumerator ScaleDown(Vector3 targetScale, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetScale;
        gameObject.SetActive(false);
        isShrinking = false;
    }
}