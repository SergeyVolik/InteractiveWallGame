using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace NewWaterfallGame
{
    public class KinectButtonAction : MonoBehaviour
    {
        Image img;
        // Start is called before the first frame update
        void Start()
        {
            img = GetComponent<Image>();
        }

        public void Action()
        {
            img.color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            Debug.Log("Action");
        }
    }
}
