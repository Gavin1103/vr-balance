using DTO.Request.Exercise.@base;

namespace DTO.Request.Exercise
{
    public class CompletedFireflyExerciseDTO : CompletedExerciseDTO
    {
        public int caughtFirefliesCount;
        public int caughtWrongFirefliesCount;
        // TODO: Add more data to store
    }
}