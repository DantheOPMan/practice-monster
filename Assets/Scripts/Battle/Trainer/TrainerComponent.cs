using UnityEngine;

namespace PracticeMonster
{
    public class TrainerComponent : MonoBehaviour
    {
        public ITrainerData TrainerData { get; private set; }
        public void Initialize(ITrainerData data)
        {
            TrainerData = data;
        }
    }
}
