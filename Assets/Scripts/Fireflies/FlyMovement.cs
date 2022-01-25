using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    float baseSpeed = 1;

    private List<GameObject> fliesInRange = new List<GameObject>();

    [SerializeField]
    private float alignmentWeight = 1, cohesionWeight = 1, seperationWeight = 1, towardsPlayerWeight = 1;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float distanceToPlayerTarget = 4, maxDistanceFromSpawn = 20;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //Set velocity to average between old velocity and target velocity in order to make flies turn gradually.
        rb.velocity = (rb.velocity.normalized + ChangeFlyVelocity()).normalized * baseSpeed;
        transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z)) * Quaternion.LookRotation(Vector3.right);
    }

    //Return the combined vector of seperation, alignment, cohesion and the vector towards either the player or spawn.
    private Vector3 ChangeFlyVelocity()
    {
        Vector3 seperationVector = Vector3.zero;
        Vector3 alignmentVector = Vector3.zero;
        Vector3 centreOfMass = Vector3.zero;

        if (fliesInRange.Count != 0)
        {
            //First add the positions/velocities of all flies in range to the seperation, cohesion and centre of mass vectors.
            for (int i = 0; i < fliesInRange.Count; i++)
            {
                GameObject otherFly = fliesInRange[i];
                seperationVector += otherFly.transform.position - transform.position;
                alignmentVector += otherFly.GetComponent<Rigidbody>().velocity;
                centreOfMass += otherFly.transform.position;
            }

            //Afterwards divide those vectors by the total flies in range.
            centreOfMass /= fliesInRange.Count;
            seperationVector /= fliesInRange.Count;
            alignmentVector /= fliesInRange.Count;

            //Return the computed vectors.
            return (ComputeAlignment(alignmentVector) * alignmentWeight + 
                ComputeCohesion(centreOfMass) * cohesionWeight + 
                ComputeSeperation(seperationVector) * seperationWeight + 
                GetVectorToPlayerOrSpawn() * towardsPlayerWeight).normalized;

        }

        return (GetVectorToPlayerOrSpawn() * towardsPlayerWeight).normalized;
    }

    private void OnTriggerEnter(Collider rangeCollision)
    {
        if (rangeCollision.gameObject.tag == "Fly")
        {
            GameObject fly = rangeCollision.transform.parent.gameObject;
            fliesInRange.Add(fly);
        }
    }

    private void OnTriggerExit(Collider rangeCollision)
    {
        if (rangeCollision.gameObject.tag == "Fly")
        {
            GameObject fly = rangeCollision.transform.parent.gameObject;
            fliesInRange.Remove(fly);
        }
    }

    //Return the average alignment of all nearby flies.
    private Vector3 ComputeAlignment(Vector3 alignmentVector)
    {
        return alignmentVector.normalized;
    }

    //Return the vector towards the centre of mass of all nearby flies.
    private Vector3 ComputeCohesion(Vector3 centreOfMass)
    {
        return (centreOfMass - transform.position).normalized;
    }

    //Return the vector away from all nearby flies.
    private Vector3 ComputeSeperation(Vector3 seperationVector)
    {
        return -seperationVector.normalized;
    }

    //If the player is within the max travel distance for the fly, move towards the player, otherwise return to spawn area.
    private Vector3 GetVectorToPlayerOrSpawn()
    {
        if (Vector3.Distance(transform.parent.position, player.transform.position) < maxDistanceFromSpawn)
            return VectorToPlayer();
        else
            return VectorToSpawn();
    }

    //If the flies are farther than the target distance to the player return vector toward player, else return 0.
    private Vector3 VectorToPlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= distanceToPlayerTarget)
            return (player.transform.position - transform.position).normalized;
        else
            return Vector3.zero;
    }

    private Vector3 VectorToSpawn()
    {
        return (transform.parent.position - transform.position).normalized;
    }
}
