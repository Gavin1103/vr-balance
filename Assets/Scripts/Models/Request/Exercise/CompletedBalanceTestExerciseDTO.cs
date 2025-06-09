using System.Collections.Generic;
using UnityEngine;

namespace Models.DTO.Exercise
{
    public class CompletedBalanceTestExerciseDTO : CompletedExerciseDTO
    {
        public List<Vector3DTO> phase_1;
        public List<Vector3DTO> phase_2;
        public List<Vector3DTO> phase_3;
        public List<Vector3DTO> phase_4;
    }
}