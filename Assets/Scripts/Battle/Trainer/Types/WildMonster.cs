using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public class WildMonster : BattleTrainer
    {
        public WildMonster(WildMonsterData trainerData) : base(trainerData)
        {
        }

        public override IEnumerator SelectMove(Battle battle, System.Action<int> onMoveSelected)
        {
            yield return new WaitForSeconds(1); // Simulate thinking time
            int randomMoveIndex = Random.Range(0, Monsters[CurrentMonsterIndex].Data.Moves.Count);
            onMoveSelected(randomMoveIndex);
            Debug.Log($"{Monsters[CurrentMonsterIndex].Nickname} uses {Monsters[CurrentMonsterIndex].Data.Moves[randomMoveIndex].Name}!");
        }
        public override IEnumerator Defend(Battle battle, System.Action<int> onDefenseSelected)
        {
            yield return new WaitForSeconds(1); // Simulate thinking time
            int randomDefenseIndex = Random.Range(0, 3); // 0 for Dodge, 1 for Brace, 2 for Standby
            onDefenseSelected(randomDefenseIndex);
            Debug.Log($"{Monsters[CurrentMonsterIndex].Nickname} selected defense index {randomDefenseIndex}");
        }
        public override Monster GetNextMonster()
        {
            // WildMonster typically only has one monster, so always return the same monster
            return null;
        }
    }
}
