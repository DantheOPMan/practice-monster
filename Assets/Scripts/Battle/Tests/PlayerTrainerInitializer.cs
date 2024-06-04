using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class PlayerTrainerInitializer : MonoBehaviour
    {
        void Start()
        {
            // Create moves for the player's monsters
            List<Move> fireCatMoves = new List<Move>
            {
                new Move("Ember", 40, 10, 1.0f, "Fire", 0),
                new Move("Scratch", 40, 5, 1.0f, "Normal", 0),
                new Move("Growl", 0, 5, 1.0f, "Normal", 0)
            };

            List<Move> waterOtterMoves = new List<Move>
            {
                new Move("Water Gun", 40, 10, 1.0f, "Water", 0),
                new Move("Tackle", 40, 5, 1.0f, "Normal", 0),
                new Move("Tail Whip", 0, 5, 1.0f, "Normal", 0)
            };

            List<Move> natureBearMoves = new List<Move>
            {
                new Move("Vine Whip", 45, 10, 1.0f, "Nature", 0),
                new Move("Scratch", 40, 5, 1.0f, "Normal", 0),
                new Move("Growl", 0, 5, 1.0f, "Normal", 0)
            };

            // Create monsters for the player
            MonsterSpecies species1 = new MonsterSpecies("FireCat", new Dictionary<string, int> { { "hp", 50 }, { "attack", 52 }, { "defense", 43 }, { "special_attack", 60 }, { "special_defense", 50 }, { "speed", 120 } }, fireCatMoves, new List<string> { "Fire" }, new List<string> { "Blaze" }, new List<string>());
            MonsterSpecies species2 = new MonsterSpecies("WaterOtter", new Dictionary<string, int> { { "hp", 55 }, { "attack", 65 }, { "defense", 60 }, { "special_attack", 50 }, { "special_defense", 50 }, { "speed", 45 } }, waterOtterMoves, new List<string> { "Water" }, new List<string> { "Torrent" }, new List<string>());
            MonsterSpecies species3 = new MonsterSpecies("NatureBear", new Dictionary<string, int> { { "hp", 60 }, { "attack", 80 }, { "defense", 50 }, { "special_attack", 50 }, { "special_defense", 50 }, { "speed", 40 } }, natureBearMoves, new List<string> { "Nature" }, new List<string> { "Overgrow" }, new List<string>());

            Monster monster1 = new Monster(species1, 15);
            Monster monster2 = new Monster(species2, 15);
            Monster monster3 = new Monster(species3, 15);

            List<Monster> playerMonsters = new List<Monster> { monster1, monster2, monster3 };
            List<Monster> pcMonsters = new List<Monster>(); // Empty PC monsters list

            PlayerTrainerData playerTrainerData = new PlayerTrainerData("PlayerName", playerMonsters, pcMonsters);
            TrainerComponent trainerComponent = GetComponent<TrainerComponent>();
            if (trainerComponent == null)
            {
                trainerComponent = gameObject.AddComponent<TrainerComponent>();
            }
            trainerComponent.Initialize(playerTrainerData);
        }
    }
}
