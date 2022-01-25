using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class LegMovement : MonoBehaviour
{
    [SerializeField]
    private CustomActionBasedController leftController, rightController;

    // Walking Action Inputs
    [SerializeField]
    private bool leftStep, rightStep;

    [SerializeField]
    private Transform vrCamera, currentLegPivot, leftLegPivot, rightLegPivot, legs, leftLeg, rightLeg;

    [SerializeField]
    private float walkSpeedModifier = 3f, totalSpeedModifier = 10f, rotateToNeutralSpeed = 100f;

    private float previousHeight, legHeight, currentSpeed;
    private bool isMovingLeg;

    private Vector3 camForward;
    private Transform leftHandRig, rightHandRig;

    private Rigidbody playerRigidbody;
    private CapsuleCollider playerCollider;
    private LayerMask terrainMask;

    public bool IsButtonPressed
    {
        get
        {
            return leftController.walkHoldLeftAction.action.ReadValue<float>() == 1 || rightController.walkHoldRightAction.action.ReadValue<float>() == 1;
        }
    }

    protected virtual void Start()
    {
        XRRig rig = FindObjectOfType<XRRig>();

        leftHandRig = rig.transform.Find("Camera Offset/LeftHand Controller");
        rightHandRig = rig.transform.Find("Camera Offset/RightHand Controller");

        playerRigidbody = GetComponentInChildren<Rigidbody>();
        playerCollider = GetComponentInChildren<CapsuleCollider>();

        terrainMask = LayerMask.GetMask("Terrain");

        StartCoroutine(RotateToNeutralOverTime());
    }

    private void Update()
    {
        UpdateTransform();
        MoveLegs();
        RotateLegs();
    }

    IEnumerator RotateToNeutralOverTime()
    {
        while (true)
        {
            if (!IsButtonPressed && (leftLeg.rotation != Quaternion.identity || rightLeg.rotation != Quaternion.identity))
            {
                Quaternion leftLegRotation = Quaternion.Euler(0, leftLeg.transform.eulerAngles.y, 0);
                leftLeg.transform.rotation = Quaternion.RotateTowards(leftLeg.transform.rotation, leftLegRotation, Time.deltaTime * rotateToNeutralSpeed);

                Quaternion rightLegRotation = Quaternion.Euler(0, rightLeg.transform.eulerAngles.y, 0);
                rightLeg.transform.rotation = Quaternion.RotateTowards(rightLeg.transform.rotation, rightLegRotation, Time.deltaTime * rotateToNeutralSpeed);
            }

            yield return null;
        }
    }

    private void RotateLegs()
    {
        // Rotate the legs around the player relative to the direction that the player's looking to.
        Vector3 camRot = new Vector3(legs.eulerAngles.x, vrCamera.eulerAngles.y, legs.eulerAngles.z);
        legs.eulerAngles = camRot;

        if (IsButtonPressed)
        {
            // Set the rotation of the legs that are children of the player equal to that of the leg pivot objects.
            leftLeg.rotation = new Quaternion(leftLegPivot.rotation.x, leftLegPivot.rotation.y, leftLegPivot.rotation.z, leftLegPivot.rotation.w);
            rightLeg.rotation = new Quaternion(rightLegPivot.rotation.x, rightLegPivot.rotation.y, rightLegPivot.rotation.z, rightLegPivot.rotation.w);
        }
    }

    private void DetermineStartingLeg()
    {
        if (rightLegPivot.localPosition.y > leftLegPivot.localPosition.y)
            leftStep = true;
        else
            rightStep = true;
    }

    private void MoveLegs()
    {
        if (IsButtonPressed)
        {
            if (!isMovingLeg) // Starts a new step.
            {
                if (!rightStep && !leftStep) // If there is no current moving leg.
                    DetermineStartingLeg(); // Decide with which leg to start walking depending on the height.

                if (rightStep)
                    currentLegPivot = rightLegPivot;
                else if (leftStep)
                    currentLegPivot = leftLegPivot;

                StartLegMovement(currentLegPivot.localPosition);
            }
            else
            {
                camForward = new Vector3(vrCamera.forward.x, 0f, vrCamera.forward.z);
                legHeight = currentLegPivot.localPosition.y;

                if (legHeight <= previousHeight)
                {
                    currentSpeed = GetWalkSpeed(currentLegPivot);
                    Vector3 forwardMovement = camForward * currentSpeed;
                    forwardMovement.y += GetFloorDiff(forwardMovement);
                    playerRigidbody.velocity += forwardMovement * totalSpeedModifier * Time.deltaTime; // Update the position of the player.

                }
                else // If the leg moves against the direction it was going it ends the current "step"
                    ResetWalkValues();
            }
        }
    }

    private float GetFloorDiff(Vector3 differenceVector)
    {
        Vector3 startPos = vrCamera.position;
        Vector3 endPos = vrCamera.position + differenceVector;

        Vector3 startFloor;
        Vector3 endFloor;

        if (Physics.SphereCast(new Ray(startPos, Vector3.down), playerCollider.radius, out RaycastHit startHit, 3, terrainMask))
            startFloor = startHit.point;
        else
            return 0;

        if (Physics.SphereCast(new Ray(endPos, Vector3.down), playerCollider.radius, out RaycastHit endHit, 3, terrainMask))
            endFloor = endHit.point;
        else
            return 0;

        return endFloor.y - startFloor.y;
    }

    private void StartLegMovement(Vector3 startPosition)
    {
        previousHeight = startPosition.y;
        isMovingLeg = true;
    }

    private void ResetWalkValues()
    {
        if (!leftStep)
        {
            leftStep = true;
            rightStep = false;
        }
        else
        {
            leftStep = false;
            rightStep = true;
        }
        isMovingLeg = false;
    }

    private float GetWalkSpeed(Transform leg)
    {
        float legMoveDistance = previousHeight - legHeight;
        float walkSpeed = legMoveDistance * walkSpeedModifier;

        previousHeight = leg.localPosition.y;

        return walkSpeed;
    }

    protected virtual void UpdateTransform()
    {
        leftLegPivot.localPosition = leftHandRig.localPosition;
        leftLegPivot.localRotation = leftHandRig.rotation;
        rightLegPivot.localPosition = rightHandRig.localPosition;
        rightLegPivot.localRotation = rightHandRig.rotation;
    }
}