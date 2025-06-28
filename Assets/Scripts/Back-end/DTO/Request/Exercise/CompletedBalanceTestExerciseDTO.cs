using System.Collections.Generic;
using DTO.Request.Exercise.@base;

namespace DTO.Request.Exercise
{
    public class CompletedBalanceTestExerciseDTO : CompletedExerciseDTO
    {
        public List<Vector3DTO> phase_1;
        public List<Vector3DTO> phase_2;
        public List<Vector3DTO> phase_3;
        public List<Vector3DTO> phase_4;
    }
}