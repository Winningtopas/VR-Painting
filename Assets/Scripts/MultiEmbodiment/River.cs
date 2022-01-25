using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathCreation.Examples
{
    public class River : MonoBehaviour
    {
        private ObjectPooler objectPooler;
        private int amountOfRiverBalls = 75;
        private int amountOfElastics = 75;
        public PathCreator pathCreator;

        [SerializeField]
        private Transform riverElasticsContainer;
        [SerializeField]
        private Transform riverBallsContainer;

        void Start()
        {
            objectPooler = ObjectPooler.Instance;
            for (int i = 0; i < amountOfRiverBalls; i++)
            {
                objectPooler.SpawnFromPool("RiverBalls", transform.position, transform.rotation, riverBallsContainer);
            }
            for (int i = 0; i < amountOfElastics; i++)
            {
                objectPooler.SpawnFromPool("RiverElastics", transform.position, transform.rotation, riverElasticsContainer);
            }
        }
    }
}