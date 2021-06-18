using Unity.Entities;
namespace NewWaterfallGame
{
    [GenerateAuthoringComponent]
    public struct GatePart : IComponentData
    {
        public float OpenDuration;
        public float CloseDuration;
        public float IdleDuration;
    }
}