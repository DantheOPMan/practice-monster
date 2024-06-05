using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public static class TypeEffectiveness
    {
        private static readonly Dictionary<string, Dictionary<string, float>> effectiveness = new Dictionary<string, Dictionary<string, float>>
        {
            { "Fire", new Dictionary<string, float> { { "Fire", 0.5f }, { "Water", 0.5f }, { "Nature", 2.0f } } },
            { "Water", new Dictionary<string, float> { { "Fire", 2.0f }, { "Water", 0.5f }, { "Nature", 0.5f } } },
            { "Nature", new Dictionary<string, float> { { "Fire", 0.5f }, { "Water", 2.0f }, { "Nature", 0.5f } } },
            // Add more types and their effectiveness here
        };

        public static float GetEffectiveness(string attackType, string defenseType)
        {
            if (effectiveness.ContainsKey(attackType) && effectiveness[attackType].ContainsKey(defenseType))
            {
                return effectiveness[attackType][defenseType];
            }
            return 1.0f; // Neutral effectiveness if not found
        }
    }
}
