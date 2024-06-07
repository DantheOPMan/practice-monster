using System.Collections.Generic;

namespace PracticeMonster
{
    public static class AllMoves
    {
        public static readonly Move ThunderShock = new Move(
            "Thunder Shock", 40, 20, 1.0f, "Electric", MoveCategory.Special, 0, 0.1f, 0.0f, null, null,
            new Dictionary<StatusEffectType, float> { { StatusEffectType.Paralyze, 0.1f } }
        );

        public static readonly Move QuickAttack = new Move(
            "Quick Attack", 40, 10, 1.0f, "Normal", MoveCategory.Physical, -1, 0.1f, 0.2f
        );

        public static readonly Move TailWhip = new Move(
            "Tail Whip", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0, 0.0f, 0.0f, null,
            new Dictionary<string, int> { { "Defense", -1 } }
        );

        public static readonly Move RockThrow = new Move(
            "Rock Throw", 50, 20, 0.9f, "Rock", MoveCategory.Physical, 0, 0.1f, 0.2f
        );

        public static readonly Move DefenseCurl = new Move(
            "Defense Curl", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0, 0.0f, 0.0f,
            new Dictionary<string, int> { { "Defense", 1 } }
        );

        public static readonly Move Tackle = new Move(
            "Tackle", 40, 10, 1.0f, "Normal", MoveCategory.Physical, 0, 0.1f
        );

        public static readonly Move Ember = new Move(
            "Ember", 40, 20, 1.0f, "Fire", MoveCategory.Special, 0, 0.1f, 0.0f, null, null,
            new Dictionary<StatusEffectType, float> { { StatusEffectType.Burn, 0.1f } }
        );

        public static readonly Move Scratch = new Move(
            "Scratch", 40, 10, 1.0f, "Normal", MoveCategory.Physical, 0, 0.1f
        );

        public static readonly Move Growl = new Move(
            "Growl", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0, 0.0f, 0.0f, null,
            new Dictionary<string, int> { { "Attack", -1 } }
        );

        public static readonly Move WaterGun = new Move(
            "Water Gun", 40, 20, 1.0f, "Water", MoveCategory.Special, 0, 0.1f
        );

        public static readonly Move VineWhip = new Move(
            "Vine Whip", 45, 20, 1.0f, "Nature", MoveCategory.Physical, 0, 0.1f, 0.0f, null, null,
            new Dictionary<StatusEffectType, float> { { StatusEffectType.Paralyze, 0.1f } }
        );

        public static readonly Move Bite = new Move(
            "Bite", 60, 20, 1.0f, "Dark", MoveCategory.Physical, 0, 0.1f, 0.2f
        );

        public static readonly Move WingAttack = new Move(
            "Wing Attack", 60, 20, 1.0f, "Flying", MoveCategory.Physical, 0, 0.1f
        );

        public static readonly Move Screech = new Move(
            "Screech", 0, 10, 1.0f, "Normal", MoveCategory.Status, 0, 0.0f, 0.0f, null,
            new Dictionary<string, int> { { "Defense", -2 } }
        );
    }
}
