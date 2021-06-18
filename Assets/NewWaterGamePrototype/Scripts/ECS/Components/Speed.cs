using Unity.Entities;
namespace NewWaterfallGame
{
    [GenerateAuthoringComponent]
    public struct Speed : IComponentData
    {
        public float Value;

    }
}