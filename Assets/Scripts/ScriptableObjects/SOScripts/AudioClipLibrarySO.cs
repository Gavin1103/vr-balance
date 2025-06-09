using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio/Clip Library")]
public class AudioClipLibrarySO : ScriptableObject {
    [System.Serializable]
    public class Clip {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private List<Clip> clips;

    private Dictionary<string, AudioClip> clipDict;

    private void OnEnable() {
        clipDict = new Dictionary<string, AudioClip>();
        foreach (var c in clips) {
            if (!clipDict.ContainsKey(c.name))
                clipDict.Add(c.name, c.clip);
        }
    }

    public AudioClip GetClip(string name) {
        if (clipDict == null) OnEnable(); 
        clipDict.TryGetValue(name, out AudioClip clip);
        return clip;
    }
}

