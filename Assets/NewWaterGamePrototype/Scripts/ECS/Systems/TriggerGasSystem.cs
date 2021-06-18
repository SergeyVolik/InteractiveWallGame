using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UniRx;
using Unity.Collections;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using System;

namespace NewWaterfallGame
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class TriggerGasSystem : JobComponentSystem
    {
        //public event EventHandler OnScoreEvent;

        public struct AddScoreEvent {
            public int Value;
        }

        private NativeQueue<AddScoreEvent> m_EventQueue;

        private BuildPhysicsWorld m_BuildPhysicsWorld;
        private StepPhysicsWorld m_StepPhysicsWorld;
        private EndSimulationEntityCommandBufferSystem m_EndSimulationEntityCommandBufferSystem;

        protected override void OnCreate()
        {
            m_EndSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            m_BuildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            m_StepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();


            m_EventQueue = new NativeQueue<AddScoreEvent>(Allocator.Persistent);
        }
        protected override void OnDestroy()
        {
            m_EventQueue.Dispose();
        }

        [BurstCompile]
        struct TriggerScores : ITriggerEventsJob
        {
            public ComponentDataFromEntity<GasTrigger_Tag> WaterTrigger;
            public ComponentDataFromEntity<Gas_Tag> WaterPaticles;
            public ComponentDataFromEntity<PhysicsVelocity> PhysicsVelocityGroup;

            public NativeQueue<AddScoreEvent>.ParallelWriter EventQueueParallel;
            public EntityCommandBuffer.ParallelWriter ECB;
            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                bool isBodyATrigger = WaterTrigger.HasComponent(entityA);
                bool isBodyBTrigger = WaterTrigger.HasComponent(entityB);

                if (isBodyBTrigger && isBodyATrigger)
                    return;

                bool isBodyADynamic = PhysicsVelocityGroup.HasComponent(entityA);
                bool isBodyBDynamic = PhysicsVelocityGroup.HasComponent(entityB);

                if (isBodyATrigger && !isBodyBDynamic ||
                    isBodyBTrigger && !isBodyADynamic)
                    return;

                var triggerEntity = isBodyATrigger ? entityA : entityB;
                var triggerEntityIndex = isBodyATrigger ? triggerEvent.BodyIndexA : triggerEvent.BodyIndexB;
                var dynamicEntity = isBodyATrigger ? entityB : entityA;
                var dynamicEntityIndex = isBodyATrigger ? triggerEvent.BodyIndexB : triggerEvent.BodyIndexA;
                if (WaterPaticles.HasComponent(dynamicEntity))
                {

                    EventQueueParallel.Enqueue(new AddScoreEvent { Value = 1 });
                    ECB.RemoveComponent<Gas_Tag>(dynamicEntityIndex, dynamicEntity);
                }

               

            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {

            var job = new TriggerScores() {
                WaterPaticles = GetComponentDataFromEntity<Gas_Tag>(),
                WaterTrigger = GetComponentDataFromEntity<GasTrigger_Tag>(),
                PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>(),
                EventQueueParallel = m_EventQueue.AsParallelWriter(),
                ECB = m_EndSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter()
           };



            JobHandle handle = job.Schedule(m_StepPhysicsWorld.Simulation,
                ref m_BuildPhysicsWorld.PhysicsWorld, inputDeps);

            handle.Complete();

           
            var sum = 0;
            while (m_EventQueue.TryDequeue(out AddScoreEvent addScore))
            {
                sum += addScore.Value;
            }
            if (sum > 0)
            {
                var score = GetSingleton<Score>();
                score.Value += sum;
                SetSingleton(score);
            }
            m_EndSimulationEntityCommandBufferSystem.AddJobHandleForProducer(handle);

            return handle;
        }

        
    }
}