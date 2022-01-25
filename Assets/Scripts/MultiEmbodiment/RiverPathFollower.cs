using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class RiverPathFollower : PathFollower, IPooledObject
    {
        [SerializeField]
        private float minSpeed = .5f, maxSpeed = 2.5f;

        [SerializeField]
        private float minPathOffset = -.5f, maxPathOffset = .5f;

        private float pathLength;
        private float startPosition;
        private float pathOffset;

        private Quaternion randomRotation;

        [SerializeField]
        private Material[] materials = new Material[3];

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        private void AssignPosition()
        {
            if (GetComponentInParent<River>())
                pathCreator = GetComponentInParent<River>().pathCreator;

            pathLength = pathCreator.path.length;
            startPosition = Random.Range(0f, pathLength);
            distanceTravelled = startPosition;

            pathOffset = Random.Range(minPathOffset, maxPathOffset);
            speed = Random.Range(minSpeed, maxSpeed);
            Vector3 pathPostion = pathCreator.path.GetPointAtDistance(startPosition, endOfPathInstruction);
            transform.position = pathPostion;
        }

        private void AssignRotation()
        {
            randomRotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), 0, 0));
        }

        private void AssignMaterial()
        {
            Material randomMaterial = materials[Random.Range(0, materials.Length)];
            if (transform.childCount != 0)
                transform.GetChild(0).GetComponent<MeshRenderer>().material = randomMaterial;
            else
                GetComponent<MeshRenderer>().material = randomMaterial;
        }

        // Update is called once per frame
        protected override void Update()
        {
            if (pathCreator != null)
            {
                distanceTravelled += speed * Time.deltaTime;

                Vector3 pathPostion = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                Vector3 pathNormal = pathCreator.path.GetNormalAtDistance(distanceTravelled, endOfPathInstruction);

                transform.position = pathPostion + pathNormal * pathOffset;
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction) * randomRotation;
            }
        }
        public void OnObjectSpawn()
        {
            AssignPosition();
            AssignRotation();
            AssignMaterial();
        }

    }
}
