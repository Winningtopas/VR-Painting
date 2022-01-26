using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PaintTarget))]
public class ShootPaint : XRBaseInteractable
{
    private PaintTarget paintTarget;
    private void Start()
    {
        paintTarget = GetComponent<PaintTarget>();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        //args.interactor.allowSelect = false; //Disable the ray selector so that multiple stacked bodies don't all get inhabited
        BucketPaintObject(args.interactor.gameObject);
        base.OnSelectEntered(args);
    }

    private void BucketPaintObject(GameObject paintObject)
    {
        CollisionPainter collisionPainter = paintObject.GetComponent<CollisionPainter>();
        Brush brush = collisionPainter.brush;
        Color color = collisionPainter.paintMaterial.color;

        PaintTarget.PaintObject(paintTarget, Vector3.zero, Vector3.zero, brush, color, true);
    }
}
