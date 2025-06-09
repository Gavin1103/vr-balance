using UnityEngine;

public class AffordanceCheck : MonoBehaviour
{
    public GameObject line;
    // UI affordance
    
    public void ColouredLine(Color name)
    {
        line.GetComponent<Renderer>().material.color = name;
    }

    public void UIPanelAffordance()
    {
        // set UI affordance according to chosen exercise to info how the player has to stand. doesn't need to get here if the exercise uses the NPC
    }
}
