using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx;
using System;

namespace NewWaterfallGame
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private float m_Time = 10;
        private float m_CurrentTime;

        [SerializeField]
        private TMP_Text m_TimerText;

        const string BEFORE_TIMER_TEXT = "Time: ";
        // Start is called before the first frame update

        StopTimerMessage m_StopTimerMessage;
        void Awake()
        {
            m_StopTimerMessage = new StopTimerMessage();
            m_CurrentTime = m_Time;
            m_TimerText.text = BEFORE_TIMER_TEXT + m_Time;

           
        }

        
        IEnumerator StartTimer()
        {
            while (m_CurrentTime > 0)
            {
                
                m_CurrentTime -= 0.1f;
                m_TimerText.text = BEFORE_TIMER_TEXT + Math.Round(m_CurrentTime, 1);

                yield return new WaitForSeconds(0.1f);

            }

            MessageBroker.Default.Publish(m_StopTimerMessage);
        }

        private CompositeDisposable m_MsgHolder;

        private void OnEnable()
        {
            m_MsgHolder = new CompositeDisposable();
            MessageBroker.Default.Receive<StartTimerMessage>().Subscribe(score => {
                StartCoroutine(StartTimer());
            }).AddTo(m_MsgHolder);
        }
        private void OnDisable()
        {
            m_MsgHolder.Dispose();
        }


    }
}