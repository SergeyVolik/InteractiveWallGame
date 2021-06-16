using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NewWaterfallGame
{
    public class BasketWithWater : MonoBehaviour
    {
        [SerializeField]
        private Transform m_Water;

        [SerializeField]
        private float maxWaterLevel;
        private void Awake()
        {
            MessageBroker.Default.Receive<CollisionMessage>().Subscribe(_ => { 

            });
        }
    }
}