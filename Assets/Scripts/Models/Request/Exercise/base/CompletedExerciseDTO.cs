using System;

namespace Models.DTO.Exercise
{
    /// <summary>
    /// Abstract base class for representing a completed exercise data transfer object (DTO).
    /// This DTO holds shared properties for all completed exercises, including:
    /// - <see cref="DifficultyEnum"/>: the difficulty level of the exercise,
    /// - <see cref="earnedPoints"/>: points earned from the exercise,
    /// - <see cref="completedAt"/>: the timestamp when the exercise was completed.
    /// 
    /// Specific exercise DTOs (e.g., BalanceTestExerciseDTO, FireflyExerciseDTO) should inherit from this class.
    /// </summary>
    public abstract class CompletedExerciseDTO
    {
        public Difficulty difficulty;
        public int earnedPoints;
        public DateTime completedAt;
    }
}