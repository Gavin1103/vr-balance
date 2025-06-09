using UnityEngine;

    public class GoBackButton: MonoBehaviour
    {
        public GameObject CurrentScreen;
        public GameObject NextScreen;
        
        public void GoBack()
        {
            CurrentScreen.SetActive(false);
            NextScreen.SetActive(true);
        }

    }