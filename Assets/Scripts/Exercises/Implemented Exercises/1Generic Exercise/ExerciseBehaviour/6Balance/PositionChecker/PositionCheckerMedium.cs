using UnityEngine;

[CreateAssetMenu(fileName = "PositionCheckerMedium", menuName = "Scriptable Objects/PositionCheckerMedium")]
public class PositionCheckerMedium : PositionChecker
{
    public float holdTime;
    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }
    public float minZ { get; private set; }
    public float maxZ { get; private set; }
    public PositionCheckerMedium(float holdTime, float minX, float maxX, float minY, float maxY, float minZ, float maxZ) : base(holdTime, minX, maxX, minY, maxY, minZ, maxZ)
    {
        string chosenDifficulty = DifficultyManager.Instance.SelectedDifficulty.ToString();
        if (chosenDifficulty == "Medium")
        {
            this.holdTime = holdTime;
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.minZ = minZ;
            this.maxZ = maxZ;
        }
    }
}
