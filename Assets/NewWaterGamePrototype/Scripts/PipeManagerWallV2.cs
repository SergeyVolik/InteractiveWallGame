using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;
namespace NewWaterfallGame
{

    public class PipeManagerWallV2 : MonoBehaviour
    {
        public static PipeManagerWallV2 Instance;

        readonly List<Vector2> centers = new List<Vector2>();
        bool centersPipeIsRunning;
        bool shouldSend;

        static readonly int PIPE_BUFFER_BYTES = 50000;
        const string CENTERS_PIPE_NAME = "centers_pipe_wall";
        static int WIDTH;
        static int HEIGHT;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            if (Display.displays.Length > 1)
            {
                WIDTH = Display.displays[1].renderingWidth;
                HEIGHT = Display.displays[1].renderingHeight;
            }
            else
            {
                // --- DEBUG ---

                WIDTH = Display.displays[0].renderingWidth;
                HEIGHT = Display.displays[0].renderingHeight;
            }

            centersPipeIsRunning = false;
            shouldSend = false;
        }

        void Update()
        {
            if (!centersPipeIsRunning)
            {
                StartCentersPipe();
                centersPipeIsRunning = true;
            }
        }

        void OnDestroy()
        {
            shouldSend = false;
        }

        private void StartCentersPipe()
        {
            shouldSend = true;
            Thread pipeCentersThread = new Thread(pipeCentersThreadFunc);
            pipeCentersThread.Start();
        }

        private void pipeCentersThreadFunc()
        {
            try
            {
                using (NamedPipeClientStream centersPipe = new NamedPipeClientStream(".", CENTERS_PIPE_NAME, PipeDirection.In))
                {
                    centersPipe.Connect();
                    while (centersPipe.IsConnected && shouldSend)
                    {

                        byte[] bytes = new byte[PIPE_BUFFER_BYTES];
                        int len = centersPipe.Read(bytes, 0, PIPE_BUFFER_BYTES);

                        SaveCenters(bytes, len);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Centers pipe failed, reconnecting... " + ex.Message);
                Thread.Sleep(5000);
            }

            centersPipeIsRunning = false;
        }

        private void SaveCenters(byte[] bytes, int len)
        {
            lock (centers)
            {
                centers.Clear();
                float[] floats = new float[len / 4];
                Buffer.BlockCopy(bytes, 0, floats, 0, len);

                int length = floats.Length;
                if (length < 1) return;
                int i = 0;

                while (i < length)
                {
                    centers.Add(new Vector2(
                        floats[i] * WIDTH,
                        (1.0f - floats[i + 1]) * HEIGHT
                    ));

                    i += 2;
                }
            }
        }

        public Vector2[] GetCenters()
        {
            lock (centers)
            {
                return centers.ToArray();
            }
        }
    }
}
