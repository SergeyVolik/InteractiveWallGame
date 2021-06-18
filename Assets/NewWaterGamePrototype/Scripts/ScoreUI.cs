using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;

namespace NewWaterfallGame
{
    public class ScoreUI : MonoBehaviour
    {
        public int Score => m_Score;
        private int m_Score;

        private CompositeDisposable m_MsgHolder;
        [SerializeField]
        private TMP_Text scoreText;
        private void OnEnable()
        {
            m_MsgHolder = new CompositeDisposable();
            MessageBroker.Default.Receive<ScoreMessage>().Subscribe(score => {
                m_Score = score.Value;

                scoreText.text = $"Score: {score.Value}";
            }).AddTo(m_MsgHolder);
        }
        private void OnDisable()
        {
            m_MsgHolder.Dispose();
        }
    }

}