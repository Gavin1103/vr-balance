using UnityEngine;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

public class GoBackButton: MonoBehaviour
{
    public GameObject CurrentScreen;
    public GameObject NextScreen;
    
    public void GoBack()
    {
        if (NonNativeKeyboard.Instance != null)
        {
            NonNativeKeyboard.Instance.gameObject.SetActive(false);
        }
        
        CurrentScreen.SetActive(false);
        NextScreen.SetActive(true);
    }

}