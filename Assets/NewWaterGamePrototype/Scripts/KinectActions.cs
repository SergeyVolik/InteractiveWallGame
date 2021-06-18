using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine.SceneManagement;
using UniRx;

namespace NewWaterfallGame
{
    public class KinectActions : MonoBehaviour
    {
        // Start is called before the first frame update
        private EntityManager m_Manager;

        private EntityQuery m_QueryWaterGate;
        private EntityQuery m_QueryLavaGate;
        private EntityQuery m_QueryGasGate;

        void Awake()
        {


            m_Manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            m_QueryWaterGate = m_Manager.CreateEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(GateOpening), typeof(GateIdle), typeof(GateClosing) },
                All = new ComponentType[] { typeof(Water_Tag),
               ComponentType.ReadOnly<GatePart>() }
            });
            m_QueryGasGate = m_Manager.CreateEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(GateOpening), typeof(GateIdle), typeof(GateClosing) },
                All = new ComponentType[] { typeof(Gas_Tag),
               ComponentType.ReadOnly<GatePart>() }
            });
            m_QueryLavaGate = m_Manager.CreateEntityQuery(new EntityQueryDesc
            {
                None = new ComponentType[] { typeof(GateOpening), typeof(GateIdle), typeof(GateClosing) },
                All = new ComponentType[] { typeof(Lava_Tag),
               ComponentType.ReadOnly<GatePart>() }
            });
        }


        public void OpenGasGates()
        {
            m_Manager.AddComponent<GateOpening>(m_QueryGasGate);
        }
        public void OpenWaterGates()
        {
            m_Manager.AddComponent<GateOpening>(m_QueryWaterGate);
        }
        public void OpenLavaGates()
        {
            m_Manager.AddComponent<GateOpening>(m_QueryLavaGate);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void StartGame()
        {
            MessageBroker.Default.Publish(new StartTimerMessage());
        }
    }
}