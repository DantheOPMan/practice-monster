using System.Collections.Generic;

namespace PracticeMonster
{
    public enum AbilityTrigger
    {
        OnSwitchIn,
        OnTurnStart,
        OnTurnEnd,
        OnMoveUsed,
        OnMoveReceived,
        OnStatusApplied,
        OnStatusRemoved,
        OnKnockedOut
    }


    public abstract class Ability
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<AbilityTrigger> Triggers { get; private set; }

        protected Ability(string name, string description, List<AbilityTrigger> triggers)
        {
            Name = name;
            Description = description;
            Triggers = triggers;
        }

        public abstract float Activate(Battle battle, Monster owner, Move move, Monster target = null);
    }
}
