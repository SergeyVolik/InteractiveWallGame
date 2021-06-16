using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NewWaterfallGame
{
    public class DebugCirclesManagerV2 : MonoBehaviour
    {
        public static DebugCirclesManagerV2 Instance;

        public GameObject debugFolder;
        public GameObject[] circlePrefabs;

        const float SPAWN_DELAY = 0.01f;
        bool wallCreating;
        public bool Active = false;

        void Awake()
        {
            Instance = this;
            Set(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                Active = !Active;
                Set(Active);
            }
        }

        public void Set(bool value)
        {
            debugFolder.SetActive(value);
        }

        public void UpdateCircles(Vector2[] positions)
        {
            CreateWallCircles(positions);
        }

        public void CreateWallCircles(Vector2[] positions)
        {
            if (!wallCreating)
            {
                if (positions != null)
                {
                    for (int i = 0; i < positions.Length; i++)
                    {
                        GameObject circle = Instantiate(circlePrefabs[UnityEngine.Random.Range(0, circlePrefabs.Length)], debugFolder.transform);
                        circle.GetComponent<RectTransform>().anchoredPosition = positions[i];
                        wallCreating = true;
                        StartCoroutine(CreateAnimationWait(circle));
                    }
                    StartCoroutine(WaitTimeWall(SPAWN_DELAY));
                }
            }
        }

        IEnumerator WaitTimeWall(float sec)
        {
            yield return new WaitForSeconds(sec);
            wallCreating = false;
        }
        IEnumerator CreateAnimationWait(GameObject circle)
        {
            yield return new WaitForSeconds(1f);
            Destroy(circle);
        }
    }
}