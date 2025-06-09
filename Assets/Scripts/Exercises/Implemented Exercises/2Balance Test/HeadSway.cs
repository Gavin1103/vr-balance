using System.Collections.Generic;
using System.Linq;
using Models.DTO.Exercise;
using UnityEngine;

namespace Exercises.Implemented_Exercises._2Balance_Test
{
    /// <summary>
    /// Calculates head sway based on headset movement data and determines difficulty level.
    /// Used in balance test exercises to advise difficulty based on user stability.
    /// </summary>
    public class HeadSway
    {
        // Thresholds for sway values to determine difficulty
        private float easy = 0.005f;
        private float medium = 0.001f;
        private float hard = 0.0003f;

        /// <summary>
        /// Calculates the average head sway from phase 1 and phase 2,
        /// then determines the appropriate difficulty level based on the result.
        /// </summary>
        /// <param name="phase1">List of Vector3DTO positions from phase 1</param>
        /// <param name="phase2">List of Vector3DTO positions from phase 2</param>
        public void DetermineDifficultyFromFirstTwoPhases(List<Vector3DTO> phase1, List<Vector3DTO> phase2)
        {
            float sway1 = CalculateAverageSway(phase1);
            float sway2 = CalculateAverageSway(phase2);
            float averageSway = (sway1 + sway2) / 2f;

            DetermineDifficultyBasedOnSway(averageSway);
        }

        /// <summary>
        /// Determines the difficulty level based on the given sway value.
        /// The lower the sway, the higher the difficulty (more stable user).
        /// </summary>
        /// <param name="sway">Average sway distance</param>
        private void DetermineDifficultyBasedOnSway(float sway)
        {
            Difficulty difficulty;
            int index;

            if (sway < hard)
            {
                difficulty = Difficulty.Hard;
                index = 2;
            }
            else if (sway < medium)
            {
                difficulty = Difficulty.Medium;
                index = 1;
            }
            else if (sway > easy)
            {
                difficulty = Difficulty.Easy;
                index = 0;
            }
            else
            {
                difficulty = Difficulty.Easy;
                index = 0;
            }

            DifficultyManager.Instance.SetAdvisedDifficulty(index, difficulty);
        }

        /// <summary>
        /// Calculates the average sway (distance between consecutive points)
        /// based on a list of Vector3DTO positions.
        /// </summary>
        /// <param name="phase">List of Vector3DTO positions for one test phase</param>
        /// <returns>The average sway distance</returns>
        private float CalculateAverageSway(List<Vector3DTO> phase)
        {
            if (phase == null || phase.Count < 2)
                return 0f;

            float totalDistance = 0f;
            for (int i = 1; i < phase.Count; i++)
            {
                Vector3 current = phase[i].ToVector3();
                Vector3 previous = phase[i - 1].ToVector3();
                totalDistance += Vector3.Distance(current, previous);
            }

            return totalDistance / (phase.Count - 1);
        }
    }
}