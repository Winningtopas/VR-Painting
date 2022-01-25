using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;

#if UNITY_EDITOR
[InitializeOnLoad] // Automatically register in editor.
#endif

[DisplayStringFormat("{firstPart}+{secondPart}")]
public class Vector3SubtractionComposite : InputBindingComposite<Vector3>
{
    [InputControl(layout = "Button")]
    public int firstPart;

    [InputControl(layout = "Button")]
    public int secondPart;

    public override Vector3 ReadValue(ref InputBindingCompositeContext context)
    {
        var firstPartValue = context.ReadValue<Vector3, Vector3MagnitudeComparer>(firstPart);
        var secondPartValue = context.ReadValue<Vector3, Vector3MagnitudeComparer>(secondPart);

        return firstPartValue - secondPartValue;
    }


    static Vector3SubtractionComposite()
    {
        InputSystem.RegisterBindingComposite<Vector3SubtractionComposite>();
    }

    [RuntimeInitializeOnLoadMethod]
    static void Init() { } // Trigger static constructor.
}
