using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;

public class CustomActionBasedController : ActionBasedController
{
    [SerializeField]
    InputActionProperty m_WalkHoldLeftAction;
    public InputActionProperty walkHoldLeftAction
    {
        get => m_WalkHoldLeftAction;
        set => SetInputActionProperty(ref m_WalkHoldLeftAction, value);
    }

    [SerializeField]
    InputActionProperty m_WalkHoldRightAction;
    public InputActionProperty walkHoldRightAction
    {
        get => m_WalkHoldRightAction;
        set => SetInputActionProperty(ref m_WalkHoldRightAction, value);
    }

    [SerializeField]
    InputActionProperty m_ExitBodyAction;
    public InputActionProperty exitBodyAction
    {
        get => m_ExitBodyAction;
        set => SetInputActionProperty(ref m_ExitBodyAction, value);
    }

    [SerializeField]
    InputActionProperty m_HidePalletteAction;
    public InputActionProperty hidePalletteAction
    {
        get => m_HidePalletteAction;
        set => SetInputActionProperty(ref m_HidePalletteAction, value);
    }

    //Copied directly from action based controller because changing the function to protected in the base class is not tracked by git.
    private void SetInputActionProperty(ref InputActionProperty property, InputActionProperty value)
    {
        if (Application.isPlaying)
            property.DisableDirectAction();

        property = value;

        if (Application.isPlaying && isActiveAndEnabled)
            property.EnableDirectAction();
    }
}
