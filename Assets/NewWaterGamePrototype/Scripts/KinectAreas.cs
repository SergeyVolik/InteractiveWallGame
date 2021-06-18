using System.Collections;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Linq;

namespace NewWaterfallGame
{
    public class KinectAreas : MonoBehaviour
    {
        const float DELAY_TIME = 0.75f;
        const float DELAY_LONG_TIME = 2;

        public enum AreaType
        {
            _1,
            _2,
            _3,
            _4,
            _5,
            _6
        }

        List<KinectTarget> targets = new List<KinectTarget>();

        CompositeDisposable msgHolder;

        void Awake()
        {       
            targets = FindObjectsOfType<KinectTarget>().ToList();
        }

        private void OnEnable()
        {
            msgHolder = new CompositeDisposable();
            MessageBroker.Default.Receive<KinectTargetEnabled>().Subscribe(msg => {

                if(!targets.Contains(msg.Value))
                    targets.Add(msg.Value);
            }).AddTo(msgHolder);
            MessageBroker.Default.Receive<KinectTargetDisabled>().Subscribe(msg => {
                targets.Remove(msg.Value);
            }).AddTo(msgHolder);
        }
        private void OnDisable()
        {
            msgHolder.Dispose();
        }

        void Update()
        {
            // --- DEBUG ---

            ActivateTarget(Input.mousePosition);

            // --- DSCA ---

            var positions = PipeManagerWallV2.Instance?.GetCenters();
            if (positions != null)
            {
                if (DebugCirclesManagerV2.Instance.Active)
                {
                    Debug.Log(positions.Length);
                    DebugCirclesManagerV2.Instance.UpdateCircles(positions);
                }

                foreach (var pos in positions)
                {
                    ActivateTarget(pos);
                }
            }
        }

        void ActivateTarget(Vector2 pos)
        {


            foreach (var target in targets)
            {
                
                if (!target.Blocked && target.IsInside(pos))
                {
                    target.Blocked = true;
                    StartCoroutine(ActivateDelay(target));
                }
            }
        }

        IEnumerator ActivateDelay(KinectTarget target)
        {
            yield return new WaitForSeconds(DELAY_TIME);
            target.isInside.Invoke();
            //GameManager.Instance.userAction.Invoke();
            yield return new WaitForSeconds(DELAY_LONG_TIME);
            //target.source.Disactivate();
            target.Blocked = false;
        }
    }

}