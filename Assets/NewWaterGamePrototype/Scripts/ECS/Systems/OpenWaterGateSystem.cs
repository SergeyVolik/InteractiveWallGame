using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UniRx;

namespace NewWaterfallGame
{
    public class OpenWaterGateSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem m_ecbSystem;
        PlayGateSoundMessage m_SoundMessage;
        protected override void OnCreate()
        {
            m_SoundMessage = new PlayGateSoundMessage();
            m_ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
            base.OnCreate();
        }
        protected override void OnUpdate()
        {
            // Local variable captured in ForEach
            float dT = Time.DeltaTime;
            var ecb = m_ecbSystem.CreateCommandBuffer();



            Entities
              .ForEach((Entity entity, ref Rotation rotation,  ref GateOpening openingTag, in GatePart gatePart, in Speed speed) =>
              {
                  if (openingTag.T == 0)
                  {
                      MessageBroker.Default.Publish(m_SoundMessage);
                  }
                  openingTag.T += dT;
                  rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.RotateZ(
                    math.radians(
                        speed.Value * dT
                    )
                ));


                  if (openingTag.T > gatePart.OpenDuration)
                  {
                      ecb.RemoveComponent<GateOpening>(entity);
                      ecb.AddComponent<GateIdle>(entity);
                  }
              })
              .WithoutBurst()
              .Run();

            Entities
             .ForEach((Entity entity, ref GateIdle gateIdle, in GatePart gatePart,in Speed speed) =>
             {
                 gateIdle.T += dT;

                 if (gateIdle.T > gatePart.IdleDuration)
                 {
                     ecb.RemoveComponent<GateIdle>(entity);
                     ecb.AddComponent<GateClosing>(entity);
                 }
             })
             .WithoutBurst()
             .Run();

            Entities
            .ForEach((Entity entity, ref Rotation rotation, ref GateClosing gateClosing, in GatePart gatePart, in Speed speed) =>
            {
                if (gateClosing.T == 0)
                {
                    MessageBroker.Default.Publish(m_SoundMessage);
                }

                rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.RotateZ(
                   math.radians(
                       -speed.Value * dT
                   )
               ));

                gateClosing.T += dT;

                if (gateClosing.T > gatePart.CloseDuration)
                {
                    ecb.RemoveComponent<GateClosing>(entity);

                }
            })
            .WithoutBurst()
            .Run();
        }
    }

}