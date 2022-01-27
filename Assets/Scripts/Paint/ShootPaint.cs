using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(PaintTarget))]
[RequireComponent(typeof(AudioSource))]

public class ShootPaint : XRBaseInteractable
{
    [SerializeField]
    private bool hasFire;
    [SerializeField]
    private ParticleSystem[] ps;

    private ParticleSystem.MainModule[] psMain = new ParticleSystem.MainModule[4];

    private PaintTarget paintTarget;
    private AudioSource audioSource;

    private void Start()
    {
        paintTarget = GetComponent<PaintTarget>();
        audioSource = GetComponent<AudioSource>();
        if (ps != null)
        {
            for (int i = 0; i < ps.Length; i++)
            {
                psMain[i] = ps[i].main;
            }
        }

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
        audioSource.Play();
        if (hasFire)
            ChangeParticleColors(color);
    }

    private void ChangeParticleColors(Color color)
    {
        for (int i = 0; i < psMain.Length; i++)
        {
            psMain[i].startColor = color;
        }
    }
}
