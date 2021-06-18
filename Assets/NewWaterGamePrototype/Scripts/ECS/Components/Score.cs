using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
namespace NewWaterfallGame
{
    [GenerateAuthoringComponent]
    public struct Score : IComponentData
    {
        public int Value;
    }
}
