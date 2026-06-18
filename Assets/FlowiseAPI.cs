//author : Dhruv Adhia

using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FlowiseAPI : MonoBehaviour
{
    //private readonly string url = "https://flowise.thecela.com/api/v1/prediction/587e29be-52c5-471d-985c-42838b115776";
    //private readonly string token = "REDACTED_SEE_README"; 
    //private readonly string url = "https://flowise-h06w.onrender.com/api/v1/prediction/f7e3eb09-a5e7-49d8-b1be-ebd0b02951bf";
    //private readonly string token = "REDACTED_SEE_README"; 
    private readonly string url = "https://flowise-h06w.onrender.com/api/v1/prediction/f7e3eb09-a5e7-49d8-b1be-ebd0b02951bf";
    private readonly string token = "REDACTED_SEE_README"; 
    public delegate void ResponseReceived(string response);
    public event ResponseReceived OnResponseReceived;

    [System.Serializable]
    public class ApiResponse
    {
        public string text;
        // Add other fields if necessary
    }

    // Public method to send a question to the API
    public void SendQuestion(string question)
    {
        string jsonPayload = "{\"question\": \"" + question + "\"}";
        StartCoroutine(QueryFlowise(jsonPayload));
    }

    IEnumerator QueryFlowise(string json)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + token);

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + webRequest.error);
                OnResponseReceived?.Invoke("Error: " + webRequest.error);
            }
            else
            {
                var responseText = webRequest.downloadHandler.text;
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(responseText);
                Debug.Log("Response: " + response.text);
                OnResponseReceived?.Invoke(response.text);
            }
        }
    }
}
