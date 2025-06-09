using UnityEngine;

namespace Models.DTO.Exercise
{
    public class Vector3DTO
    {
        public float x;
        public float y;
        public float z;

        public Vector3DTO(Vector3 vec)
        {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}