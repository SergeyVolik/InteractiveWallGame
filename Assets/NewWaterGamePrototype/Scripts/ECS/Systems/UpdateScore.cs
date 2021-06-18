using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UniRx;
using UnityEngine;

namespace NewWaterfallGame
{
    public class UpdateScore : SystemBase
    {
        private ScoreMessage message;
        protected override void OnCreate()
        {
            message = new ScoreMessage();
            base.OnCreate();
        }
        protected override void OnUpdate()
        {
            // Local variable captured in ForEach
            float dT = Time.DeltaTime;
            
            Entities.WithChangeFilter<Score>()
              .ForEach((Entity entity, in Score score) =>
              {
                  message.Value = score.Value;
                  Debug.Log("Update Score");
                  MessageBroker.Default.Publish(message);
              })
              .WithoutBurst()
              .Run();


        }

    }
}