using System;
using UnityEngine;

namespace PracticeMonster
{
    [Serializable]
    public class PlayerRotationData
    {
        [field:SerializeField] public Vector3 TargetRotationReachTime { get; private set; }
    }
}
