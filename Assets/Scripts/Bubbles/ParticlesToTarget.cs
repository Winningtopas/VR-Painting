using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.ParticleSystem;

public class ParticlesToTarget : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem system;
    private Particle[] particles = new Particle[100];
    private float[] fallSpeeds = new float[100];

    private float minFallSpeed = 6f, maxFallSpeed = 7f;
    private float speedToCenter = 2f;

    void Start()
    {
        if (system == null)
            system = GetComponent<ParticleSystem>();

        for (int i = 0; i < fallSpeeds.Length; i++)
            fallSpeeds[i] = Random.Range(minFallSpeed, maxFallSpeed);

        system.Play();
    }


    private IEnumerator MoveDownward()
    {
        while (system.GetParticles(particles) == 0)
            yield return null;

        bool allParticlesDone = false;

        while (!allParticlesDone)
        {
            allParticlesDone = true;
            for (int i = 0; i < particles.Length; i++)
            {
                float targetHeight = transform.parent.position.y;
                float particleHeight = particles[i].position.y;
                float newHeight = particleHeight - fallSpeeds[i] * Time.deltaTime;

                if (newHeight > targetHeight)
                {
                    particles[i].position = new Vector3(particles[i].position.x, newHeight, particles[i].position.z);
                    allParticlesDone = false;
                }
                else
                    particles[i].position = new Vector3(particles[i].position.x, targetHeight, particles[i].position.z);
            }
            system.SetParticles(particles);
            yield return null;
        }
        StartCoroutine(MoveParticlesToCenter());
    }

    private IEnumerator MoveParticlesToCenter()
    {
        bool allParticlesDone = false;

        while (!allParticlesDone)
        {
            allParticlesDone = true;
            for (int i = 0; i < particles.Length; i++)
            {
                Vector3 targetPos = transform.parent.position;
                float distToCenter = Vector3.Distance(particles[i].position, targetPos);
                float travelDist = Time.deltaTime * speedToCenter;

                if (distToCenter > travelDist)
                {
                    Vector3 diff = targetPos - particles[i].position;
                    particles[i].position += diff.normalized * travelDist;
                    allParticlesDone = false;
                }
                else
                    particles[i].position = targetPos;
            }
            system.SetParticles(particles);
            yield return null;
        }

        DestroyShards();
    }

    private void OnEnable()
    {
        system.Play();
        StartCoroutine(MoveDownward());
    }

    private void OnDisable()
    {
        system.Stop();
    }

    private void DestroyShards()
    {
        GetComponentInParent<BubbleSpawner>().SpawnBubble(); // spawn the bubble
    }
}