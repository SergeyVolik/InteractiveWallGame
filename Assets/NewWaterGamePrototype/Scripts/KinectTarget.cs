using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace NewWaterfallGame
{
    public class KinectTarget : MonoBehaviour
    {
        public KinectAreas.AreaType type;

        public bool Blocked { get; set; }

        [SerializeField] RectTransform area;
        public UnityEvent isInside = new UnityEvent();

        void Awake()
        {
            if (area == null)
            {
                area = GetComponent<RectTransform>();
            }
        }

        public bool IsInside(Vector2 pos)
        {

            var widthHalf = area.rect.width / 2;
            var heightHalf = area.rect.height / 2;
            var leftBottom = new Vector2(area.position.x - widthHalf, area.position.y - heightHalf);
            var rightTop = new Vector2(area.position.x + widthHalf, area.position.y + heightHalf);

            //return RectTransformUtility.RectangleContainsScreenPoint(area, pos, Camera.main);

            return pos.x >= leftBottom.x && pos.x <= rightTop.x && pos.y >= leftBottom.y && pos.y <= rightTop.y;
        }
    }

}