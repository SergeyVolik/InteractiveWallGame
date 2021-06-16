using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace NewWaterfallGame
{
    public class BasketWater : MonoBehaviour
    {
        [SerializeField]
        private float maxWaterLevel;

      

        private float startedWaterLevel;
        private float waterPosY;
        private float maxWaterHeight;

        private ParticleSystem ps;
        private void Awake()
        {
            startedWaterLevel = transform.localScale.y;
            waterPosY = transform.localPosition.y;
            maxWaterHeight = maxWaterLevel - startedWaterLevel;

            ps = GetComponent<ParticleSystem>();
            MessageBroker.Default.Receive<CollisionMessage>().Subscribe(msg =>
            {
                CalcWaterLevel(msg.watelLevelT);
            });
        }

       

        private void CalcWaterLevel(float waterLevelT)
        {
            
            var newWaterLevel = startedWaterLevel + maxWaterHeight * waterLevelT;
            transform.localScale = new Vector3(transform.localScale.x, newWaterLevel, transform.localScale.z);
            transform.localPosition = new Vector3(transform.localPosition.x, waterPosY + maxWaterHeight * waterLevelT / 2, transform.localPosition.z);
        }
    }


   
}
