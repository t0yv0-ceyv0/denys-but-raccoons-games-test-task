using System.Collections.Generic;

namespace Project.Managers 
{
    public static class SpawnPresetSelector
    {
        #region Random
        public static CubesSpawnConfig GetRandomPreset(List<CubesSpawnConfig> presets)
        {

            float totalChance = 0;
            foreach (var preset in presets) totalChance += preset.spawnChance;

            float randomPoint = UnityEngine.Random.Range(0, totalChance);

            CubesSpawnConfig selectedPreset = presets[0];
            float currentSum = 0;

            foreach (var preset in presets)
            {
                currentSum += preset.spawnChance;
                if (randomPoint <= currentSum)
                {
                    selectedPreset = preset;
                    break;
                }
            }

            return selectedPreset;

        }
        #endregion
    }
}

