using System;
using UnityEngine;

namespace PracticeMonster
{
    [Serializable]
    public class PlayerCapsuleColliderUtility: CapsuleColliderUtility
    {
        [field: SerializeField] public PlayerTriggerColliderData TriggerColliderData {  get; private set; }
    }
}
