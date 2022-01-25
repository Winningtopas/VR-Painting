using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CollisionPainter : MonoBehaviour
{
    public Brush brush;

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
                PaintTarget.PaintObject(paintTarget, contact.point, contact.normal, brush, GetComponent<MeshRenderer>().material.color);
            }
        }
    }
}