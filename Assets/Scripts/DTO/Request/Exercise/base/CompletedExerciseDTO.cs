using System;

namespace DTO.Request.Exercise.@base
{

    public class CompletedExerciseDTO
    {
        public string exercise;
        public Difficulty difficulty;
        public int earnedPoints;
        public DateTime completedAt;
    }
}