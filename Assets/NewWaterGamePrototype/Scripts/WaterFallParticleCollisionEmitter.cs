using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace NewWaterfallGame
{
    public class WaterFallParticleCollisionEmitter : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem partSystem;
        public List<ParticleCollisionEvent> collisionEvents;
        private CollisionMessage message = new CollisionMessage();

        [SerializeField]
        private int particlesForFullBasket = 500;

        [SerializeField]
        private int currentParticlesTriggers = 0;

        void Awake()
        {
            //partSystem = GetComponent<ParticleSystem>();
            message = new CollisionMessage();
        }

        void OnParticleTrigger()
        {
            

            if (currentParticlesTriggers < particlesForFullBasket)
            {

                // particles
                List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

                // get
                int numEnter = partSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

                // iterate
                for (int i = 0; i < numEnter; i++)
                {
                    ParticleSystem.Particle p = enter[i];
                    p.remainingLifetime = 0;
                    enter[i] = p;
                }

                // set
                partSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

                currentParticlesTriggers += numEnter;

                message.watelLevelT = (float)currentParticlesTriggers / particlesForFullBasket;
               
                MessageBroker.Default.Publish(message);
            }

        }
    }
}
