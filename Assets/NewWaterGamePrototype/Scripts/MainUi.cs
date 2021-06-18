using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace NewWaterfallGame
{
    public class MainUi : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_BeforeGamePanel;

        [SerializeField]
        private GameObject m_AfterGamePanel;

        [SerializeField]
        private GameObject m_NextMapButton;

        [SerializeField]
        private GameObject m_InGameButtons;

        private void Awake()
        {
            scoreUI = FindObjectOfType<ScoreUI>();

            m_InGameButtons.SetActive(false);
            m_AfterGamePanel.SetActive(false);
            m_BeforeGamePanel.SetActive(true);
            m_NextMapButton.SetActive(false);
        }

        private ScoreUI scoreUI;

        private CompositeDisposable m_MsgHolder;
        private void OnEnable()
        {
            m_MsgHolder = new CompositeDisposable();
            MessageBroker.Default.Receive<StopTimerMessage>().Subscribe(_ => {
                m_AfterGamePanel.SetActive(true);
                m_InGameButtons.SetActive(false);

                if (scoreUI.Score >= Constants.POINTS_FOR_WIN_LEVEL1)
                {
                    m_NextMapButton.SetActive(true);
                }


            }).AddTo(m_MsgHolder);

            MessageBroker.Default.Receive<StartTimerMessage>().Subscribe(_ => {
                m_BeforeGamePanel.SetActive(false);
                m_InGameButtons.SetActive(true);


            }).AddTo(m_MsgHolder);
        }
        private void OnDisable()
        {
            m_MsgHolder.Dispose();
        }

    }
}