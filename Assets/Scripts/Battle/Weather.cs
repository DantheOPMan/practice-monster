using System.Collections.Generic;
using UnityEngine;

namespace PracticeMonster
{
    public enum WeatherType
    {
        Normal,
        Sunny,
        Rain,
        Sandstorm,
        Hail
    }
    public class Weather
    {
        public WeatherType Type { get; private set; }
        public int Duration { get; private set; }

        public Weather(WeatherType type, int duration)
        {
            Type = type;
            Duration = duration;
        }

        public void DecrementDuration()
        {
            if (Duration > 0)
            {
                Duration--;
            }
        }

        public bool IsExpired()
        {
            return Duration <= 0;
        }


        public void ApplyWeatherEffects(Monster monster, BattleUIManager battleUIManager)
        {
            switch (Type)
            {
                case WeatherType.Sunny:
                    if (monster.Data.Species.Types.Contains("Fire"))
                    {
                        battleUIManager.Log($"The sunlight is strong, {monster.Nickname}'s Fire-type moves are powered up!");
                    }
                    break;
                case WeatherType.Rain:
                    if (monster.Data.Species.Types.Contains("Water"))
                    {
                        battleUIManager.Log($"The rain is pouring, {monster.Nickname}'s Water-type moves are powered up!");
                    }
                    break;
                case WeatherType.Sandstorm:
                    if (!monster.Data.Species.Types.Contains("Rock") &&
                        !monster.Data.Species.Types.Contains("Ground") &&
                        !monster.Data.Species.Types.Contains("Steel"))
                    {
                        int damage = Mathf.FloorToInt(monster.Data.MaxHP / 16f);
                        monster.CurrentHP = Mathf.Max(monster.CurrentHP - damage, 0);
                        battleUIManager.Log($"{monster.Nickname} is buffeted by the sandstorm and took {damage} damage!");
                    }
                    break;
                case WeatherType.Hail:
                    if (!monster.Data.Species.Types.Contains("Ice"))
                    {
                        int damage = Mathf.FloorToInt(monster.Data.MaxHP / 16f);
                        monster.CurrentHP = Mathf.Max(monster.CurrentHP - damage, 0);
                        battleUIManager.Log($"{monster.Nickname} is pelted by hail and took {damage} damage!");
                    }
                    break;
            }
        }
    }
}
