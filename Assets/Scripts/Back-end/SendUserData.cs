using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DataSender : MonoBehaviour {
    private string serverUrl = "http://localhost:8080";
    // "https://unity-balance.up.railway.app";

    /// <summary>
    /// Call this method to send data async
    /// </summary>
    public void SendExerciseData(CompletedExerciseData data) {
        StartCoroutine(SendJson(JsonUtility.ToJson(data)));
    }

    private IEnumerator SendJson(string jsonData) {
        UnityWebRequest request = new UnityWebRequest(serverUrl, "POST");

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer(); // Required to see the response
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Data sent successfully!");
        else
            Debug.LogError("Error sending data: " + request.error);
    }
}