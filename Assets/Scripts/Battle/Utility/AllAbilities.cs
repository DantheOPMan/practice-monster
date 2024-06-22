using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{


    public class Blaze : Ability
    {
        public Blaze() : base("Blaze", "Increases Fire-type move power when HP is low.", new List<AbilityTrigger> { AbilityTrigger.OnMoveUsed }) { }

        public override float Activate(Battle battle, Monster owner, Move move, Monster target, BattleUIManager battleUIManager)
        {
            if (owner.CurrentHP <= owner.Data.MaxHP / 3 && move.Type == "Fire")
            {
                battleUIManager.Log($"{owner.Nickname}'s Blaze activated, increasing the power of {move.Name}!");
                return 1.5f;
            }
             return 1.0f;
            }
        }

    public class Intimidate : Ability
    {
        public Intimidate() : base("Intimidate", "Lowers the opposing monster's Attack on battle start or when switched in.", new List<AbilityTrigger> { AbilityTrigger.OnSwitchIn }) { }

        public override float Activate(Battle battle, Monster owner, Move move, Monster target, BattleUIManager battleUIManager)
        {
            if (target != null)
            {
                target.AttackStage -= 1;
                target.UpdateCurrentStats();
                battleUIManager.Log($"{owner.Nickname}'s Intimidate activated, lowering {target.Nickname}'s Attack!");
            }
            return 1.0f;
        }
    }


    public class Overgrow : Ability
    {
        public Overgrow() : base("Overgrow", "Increases Grass-type move power when HP is low.", new List<AbilityTrigger> { AbilityTrigger.OnMoveUsed }) { }

        public override float Activate(Battle battle, Monster owner, Move move, Monster target, BattleUIManager battleUIManager)
        {
            if (owner.CurrentHP <= owner.Data.MaxHP / 3 && move.Type == "Grass")
            {
                battleUIManager.Log($"{owner.Nickname}'s Overgrow activated, increasing the power of {move.Name}!");
                return 1.5f;
            }
            return 1.0f;
        }
    }
    /*
    public class SwiftSwim : Ability
    {
        public SwiftSwim() : base("Swift Swim", "Doubles Speed in rain.", new List<AbilityTrigger> {AbilityTrigger.OnBattleStart}) { }

        public override float Activate(Battle battle, Monster owner, Move move, Monster target, BattleUIManager battleUIManager)
        {
            if (battle.Weather == Weather.Rain)
            {
                owner.CurrentSpeed *= 2;
                owner.UpdateCurrentStats();
                battleUIManager.Log($"{owner.Nickname}'s Swift Swim activated, doubling its Speed in rain!");
            }
            return 1.0f;
        }
    }

    public class Sturdy : Ability
    {
        public Sturdy() : base("Sturdy", "Prevents being knocked out in one hit.", new List<AbilityTrigger> {AbilityTrigger.OnMoveReceived}) { }

        public override float Activate(Battle battle, Monster owner, Move move, Monster target, BattleUIManager battleUIManager)
        {
            if (owner.CurrentHP == owner.Data.MaxHP && owner.CurrentHP > 0 && owner.CurrentHP - target.CalculateDamage(owner, battleUIManager) <= 0)
            {
                owner.CurrentHP = 1;
                battleUIManager.Log($"{owner.Nickname}'s Sturdy activated, preventing it from being knocked out!");
            }
            return 1.0f;
        }
    }*/

}
