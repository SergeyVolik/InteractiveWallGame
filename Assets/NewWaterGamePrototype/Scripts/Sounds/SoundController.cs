using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
namespace NewWaterfallGame
{



    public class SoundController : MonoBehaviour
    {

        CompositeDisposable m_MsgHolder;

        [SerializeField]
        public AudioSource[] m_GateSources;

        [SerializeField]
        public AudioSource[] m_ScoreSound;

        [SerializeField]
        public AudioSource m_TimerSound;

        [SerializeField]
        public AudioSource m_WinSound;

        [SerializeField]
        public AudioSource m_LoseSound;
        private void OnEnable()
        {
            m_MsgHolder = new CompositeDisposable();
            MessageBroker.Default.Receive<PlayGateSoundMessage>().Subscribe(_ => {

                for (int i = 0; i < m_GateSources.Length; i++)
                {
                    if (!m_GateSources[i].isPlaying)
                    {
                        m_GateSources[i].Play();
                        break;
                    }

                }
            }).AddTo(m_MsgHolder);

            MessageBroker.Default.Receive<ScoreMessage>().Subscribe(_ => {

                for (int i = 0; i < m_ScoreSound.Length; i++)
                {
                    if (!m_ScoreSound[i].isPlaying)
                    {
                        m_ScoreSound[i].Play();
                        break;
                    }

                }
            }).AddTo(m_MsgHolder);

            MessageBroker.Default.Receive<StopTimerMessage>().Subscribe(_ => {

                m_TimerSound.Stop();

            }).AddTo(m_MsgHolder);

            MessageBroker.Default.Receive<StartTimerMessage>().Subscribe(_ => {

                m_TimerSound.Play();

            }).AddTo(m_MsgHolder);

        }
        private void OnDisable()
        {
            m_MsgHolder.Dispose();
        }


    }
}
