using System;
using UnityEngine;

namespace PracticeMonster
{
    [Serializable]
    public class PlayerTriggerColliderData 
    {
        [field: SerializeField] public BoxCollider GroundCheckCollider {  get; private set; }
    }
}
