using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwapEquipment : MonoBehaviour
{
    [SerializeField]
    private GameObject leftBrush, rightBrush;
    [SerializeField]
    private GameObject leftPallette, rightPallette;

    private CustomActionBasedController controller;
    private bool leftHanded;
    private bool palleteActive = true;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CustomActionBasedController>();
        controller.exitBodyAction.action.performed += Switch;
        controller.hidePalletteAction.action.performed += HidePallette;
    }

    private void Switch(InputAction.CallbackContext obj)
    {
        leftHanded = !leftHanded;
        if (leftHanded)
        {
            leftBrush.SetActive(true);
            rightBrush.SetActive(false);
            leftPallette.SetActive(false);

            if (palleteActive)
                rightPallette.SetActive(true);
        }
        else
        {
            rightBrush.SetActive(true);
            leftBrush.SetActive(false);
            rightPallette.SetActive(false);

            if (palleteActive)
                leftPallette.SetActive(true);
        }
    }

    private void HidePallette(InputAction.CallbackContext obj)
    {
        palleteActive = !palleteActive;
        if (palleteActive)
        {
            if (leftHanded)
            {
                rightPallette.SetActive(true);
                leftPallette.SetActive(false);
            }
            else
            {
                rightPallette.SetActive(false);
                leftPallette.SetActive(true);
            }
        }
        else
        {
            rightPallette.SetActive(false);
            leftPallette.SetActive(false);
        }
    }
}
