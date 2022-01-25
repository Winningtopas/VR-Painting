using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BeadChainGenerator : MonoBehaviour
{
    [SerializeField]
    private int amountBeads = 0;

    [SerializeField]
    private bool regenerate = false;

    private GameObject beadPrefab;

    private void Update()
    {
        if (Application.isEditor && gameObject.activeInHierarchy && regenerate)
        {
            HingeJoint hingeJoint;
            beadPrefab = Resources.Load<GameObject>("Prefabs/Bead");

            foreach (Transform child in transform)
                StartCoroutine(DestroyRoutine(child.gameObject));

            GameObject previousBead = null;

            for (int i = 0; i < amountBeads; i++)
            {
                GameObject bead = Instantiate(beadPrefab, new Vector3(0, -1 * i, 0), Quaternion.identity);
                bead.transform.SetParent(gameObject.transform, false);
                if (i == 0)
                {
                    Rigidbody rigidBody = bead.GetComponent<Rigidbody>();
                    rigidBody.isKinematic = true;
                    previousBead = bead;
                }
                else
                {
                    Rigidbody rigidBodyPrevious = previousBead.GetComponent<Rigidbody>();
                    hingeJoint = bead.GetComponent<HingeJoint>();
                    hingeJoint.connectedBody = rigidBodyPrevious;
                    previousBead = bead;
                }
            }
            regenerate = false;
        }
    }

    IEnumerator DestroyRoutine(GameObject gameObject)
    {
        yield return null;
        DestroyImmediate(gameObject);
    }
}
