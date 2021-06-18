using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UniRx;

namespace NewWaterfallGame
{
    public class RotationSystem : SystemBase
    {
        protected override void OnCreate()
        {

            base.OnCreate();
        }
        protected override void OnUpdate()
        {
            // Local variable captured in ForEach
            float dT = Time.DeltaTime;

            Entities
                .WithAll<Rotate>()
              .ForEach((ref Rotation rotation, in Speed speed) =>
              {
                  rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.RotateZ(
                   math.radians(
                       -speed.Value * dT
                   )));
              })
              .WithoutBurst()
              .Run();


        }

    }
}