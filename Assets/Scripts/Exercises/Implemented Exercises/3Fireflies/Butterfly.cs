using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour, Icatchable {
    [SerializeField] private List<FireFlyNet.NetType> allowedNetTypes; // Nets that can catch this FireFly
    //public static event Action OnCaught;

    public void Catch(FireFlyNet.NetType netType) {
        if (allowedNetTypes.Contains(netType)) {
            SoundManager.soundInstance.PlaySFX("FireFlyCatch");
            ScoreManager.Instance.AddScore(-100f);
            Destroy(gameObject);
        } else {
            Debug.Log("Wrong net type, can't catch this fly.");
        }
    }
}
