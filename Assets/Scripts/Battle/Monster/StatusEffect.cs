using UnityEngine;

namespace PracticeMonster
{
    public enum StatusEffectType
    {
        Burn,
        Paralyze,
        Frostbite,
        Drowsy,
        Poison,
        BadlyPoisoned,
    }

    public class StatusEffect
    {
        public StatusEffectType Type { get; private set; }
        public int Duration { get; private set; } // Duration in turns, if applicable

        public StatusEffect(StatusEffectType type, int duration = 1)
        {
            Type = type;
            Duration = duration;
        }

        public void ApplyEffect(Monster monster)
        {
            switch (Type)
            {
                case StatusEffectType.Burn:
                    monster.CurrentAttack = Mathf.FloorToInt(monster.CurrentAttack * 0.5f);
                    break;
                case StatusEffectType.Paralyze:
                    monster.CurrentSpeed = Mathf.FloorToInt(monster.CurrentSpeed * 0.5f);
                    break;
                case StatusEffectType.Frostbite:
                    monster.CurrentSpecialAttack = Mathf.FloorToInt(monster.CurrentSpecialAttack * 0.5f);
                    break;
                case StatusEffectType.Drowsy:
                    monster.CurrentSpecialDefense = Mathf.FloorToInt(monster.CurrentSpecialDefense * 0.8f);
                    monster.CurrentDefense = Mathf.FloorToInt(monster.CurrentDefense * 0.8f);
                    break;

            }
        }
        public void IncreaseDuration()
        {
            Duration++;
        }
    }
}
