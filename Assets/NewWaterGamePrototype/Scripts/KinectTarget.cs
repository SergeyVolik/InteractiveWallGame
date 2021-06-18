using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;

namespace NewWaterfallGame
{
    public class KinectTarget : MonoBehaviour
    {
        public KinectAreas.AreaType type;

        public bool Blocked { get; set; }

        private float m_DelayBeforeActive = 1f;
        private float m_DelayChangeColorAfterClick = 0.1f;
        private bool active = false;

        [SerializeField] RectTransform m_Area;
        [SerializeField] Image m_Image;

        public UnityEvent isInside = new UnityEvent();

        [SerializeField]
        private Color m_ClickColor = Color.white;
        [SerializeField]
        private Color m_NotActiveColor = Color.white;

        private Color m_DefaultColor;
        void Awake()
        {
            m_Image = GetComponent<Image>();
            m_DefaultColor = m_Image.color;

            if (m_Area == null)
            {
                m_Area = GetComponent<RectTransform>();
            }


            StartCoroutine(ActivateAfterTime());
        }

        public void SetActive(bool value)
        {
            active = value;
            if (!value)
            {
                m_Image.color = m_NotActiveColor;
            }
            else m_Image.color = m_DefaultColor;
        }

        IEnumerator ActivateAfterTime()
        {
            yield return new WaitForSeconds(m_DelayBeforeActive);
            SetActive(true);
        }
        IEnumerator Click()
        {
            active = false;
            m_Image.color = m_ClickColor;
            yield return new WaitForSeconds(m_DelayChangeColorAfterClick);
            m_Image.color = m_DefaultColor;
            active = true;
        }
        private void OnDisable()
        {
            active = false;
            MessageBroker.Default.Publish(new KinectTargetDisabled { Value = this });
        }
        private void OnEnable()
        {
            MessageBroker.Default.Publish(new KinectTargetEnabled { Value = this });
        }


        public bool IsInside(Vector2 pos)
        {
            
            var widthHalf = m_Area.rect.width / 2;
            var heightHalf = m_Area.rect.height / 2;
            var leftBottom = new Vector2(m_Area.position.x - widthHalf, m_Area.position.y - heightHalf);
            var rightTop = new Vector2(m_Area.position.x + widthHalf, m_Area.position.y + heightHalf);

            //return RectTransformUtility.RectangleContainsScreenPoint(area, pos, Camera.main);

            var result = active && pos.x >= leftBottom.x && pos.x <= rightTop.x && pos.y >= leftBottom.y && pos.y <= rightTop.y; ;
            if (result)
                StartCoroutine(Click());
                return result;


        }
    }

}