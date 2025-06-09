using UnityEngine;

public class PositionCheckerEasy : PositionChecker
{
    public float holdTime;
    public float minX { get; private set; }
    public float maxX { get; private set; }
    public float minY { get; private set; }
    public float maxY { get; private set; }
    public float minZ { get; private set; }
    public float maxZ { get; private set; }

    public PositionCheckerEasy(float holdTime, float minX, float maxX, float minY, float maxY, float minZ, float maxZ) : base(holdTime, minX, maxX, minY, maxY, minZ, maxZ)
    {
        string chosenDifficulty = DifficultyManager.Instance.SelectedDifficulty.ToString();
        if (chosenDifficulty == "Easy")
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
