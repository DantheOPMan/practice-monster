using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public enum MoveCategory
    {
        Physical,
        Special,
        Status
    }

    public class Move
    {
        public string Name { get; private set; }
        public int Power { get; private set; }
        public int StaminaCost { get; private set; }
        public float Accuracy { get; private set; }
        public string Type { get; private set; }
        public MoveCategory Category { get; private set; }
        public int TurnAdjustment { get; private set; }
        public float CritRate { get; private set; }
        public float FlinchChance { get; private set; }
        public Dictionary<string, int> AttackerStageChanges { get; private set; }
        public Dictionary<string, int> DefenderStageChanges { get; private set; }
        public Dictionary<StatusEffectType, float> StatusEffects { get; private set; }

        public Move(string name, int power, int staminaCost, float accuracy, string moveType, MoveCategory category, int turnAdjustment, float critRate = 0.1f, float flinchChance = 0.0f, Dictionary<string, int> attackerStageChanges = null, Dictionary<string, int> defenderStageChanges = null, Dictionary<StatusEffectType, float> statusEffects = null)
        {
            Name = name;
            Power = power;
            StaminaCost = staminaCost;
            Accuracy = accuracy;
            Type = moveType;
            Category = category;
            TurnAdjustment = turnAdjustment;
            CritRate = critRate;
            FlinchChance = flinchChance;
            AttackerStageChanges = attackerStageChanges ?? new Dictionary<string, int>();
            DefenderStageChanges = defenderStageChanges ?? new Dictionary<string, int>();
            StatusEffects = statusEffects ?? new Dictionary<StatusEffectType, float>();
        }

        public void Execute(BattleTrainer attackerTrainer, BattleTrainer defenderTrainer, string defensiveAction, BattleUIManager battleUIManager)
        {
            Monster attackerMonster = attackerTrainer.GetCurrentMonster();
            Monster defenderMonster = defenderTrainer.GetCurrentMonster();

            battleUIManager.Log($"{attackerMonster.Nickname} uses {Name}!");

            if (attackerMonster.CheckStatusEffectForAction(battleUIManager))
            {
                attackerTrainer.ActionTurn += attackerMonster.GetActionTurnReset();
                attackerMonster.ApplyStatusEffectDamage(battleUIManager);
                return;
            }

            if (attackerMonster.Stamina < StaminaCost)
            {
                battleUIManager.Log($"{attackerMonster.Nickname} does not have enough stamina to use {Name}!");
                attackerTrainer.ActionTurn += attackerMonster.GetActionTurnReset();
                attackerMonster.ApplyStatusEffectDamage(battleUIManager);
                return;
            }

            attackerMonster.Stamina -= StaminaCost;
            attackerTrainer.ActionTurn += TurnAdjustment + attackerMonster.GetActionTurnReset();

            if (!CalculateHit(attackerMonster, defenderMonster, defensiveAction))
            {
                battleUIManager.Log($"{attackerMonster.Nickname} missed with {Name}!");
                attackerMonster.ApplyStatusEffectDamage(battleUIManager);
                return;
            }

            int damage = CalculateDamage(attackerMonster, defenderMonster, defensiveAction, battleUIManager);
            float abilityMultiplier = attackerMonster.ActivateAbility(null, AbilityTrigger.OnMoveUsed, this, defenderMonster, battleUIManager);

            attackerMonster.ApplyStageChanges(this, true, battleUIManager);
            defenderMonster.ApplyStageChanges(this, false, battleUIManager);
            defenderMonster.CurrentHP = Mathf.Max(defenderMonster.CurrentHP - damage, 0);
            battleUIManager.Log($"{attackerMonster.Nickname} used {Name} and dealt {damage} damage to {defenderMonster.Nickname}!");
            attackerMonster.ApplyStatusEffectDamage(battleUIManager);

            if (Random.value < FlinchChance)
            {
                defenderTrainer.ActionTurn += defenderMonster.GetActionTurnReset();
                battleUIManager.Log($"{defenderMonster.Nickname} flinched!");
            }

            foreach (var statusEffect in StatusEffects)
            {
                if (Random.value < statusEffect.Value)
                {
                    defenderMonster.ApplyStatusEffect(new StatusEffect(statusEffect.Key), battleUIManager);
                }
            }
        }

        private bool CalculateHit(Monster attacker, Monster defender, string defensiveAction)
        {
            if (defensiveAction == "Brace")
            {
                return true;
            }
            float dodgeProbability = defensiveAction == "Dodge" ? defender.GetDodgeProbability(attacker.CurrentSpeed) : 0;
            float hitProbability = Accuracy * (1 - dodgeProbability) * attacker.GetAccuracyMultiplier() * defender.GetEvasivenessMultiplier();
            return Random.value <= hitProbability;
        }

        private int CalculateDamage(Monster attacker, Monster defender, string defensiveAction, BattleUIManager battleUIManager)
        {
            float defensiveMultiplier = defensiveAction == "Brace" ? 1.5f : 1f;

            int attackStat = Category == MoveCategory.Physical ? attacker.CurrentAttack : attacker.CurrentSpecialAttack;
            int defenseStat = Category == MoveCategory.Physical ? defender.CurrentDefense : defender.CurrentSpecialDefense;

            float modifier = 1;

            if (attacker.Data.Species.Types.Contains(Type))
            {
                modifier *= 1.5f;
            }

            if (Random.value < CritRate)
            {
                modifier *= 1.5f;
                battleUIManager.Log("Critical hit!");
            }

            float typeEffectiveness = 1;
            foreach (var defenderType in defender.Data.Species.Types)
            {
                typeEffectiveness *= TypeEffectiveness.GetEffectiveness(Type, defenderType);
            }
            modifier *= typeEffectiveness;

            float damage = (((attacker.Data.Level * 2f + 20f) / 300f) * (Power * modifier) * (attackStat / (defensiveMultiplier * defenseStat)));
            return Mathf.FloorToInt(damage);
        }
    }
}
