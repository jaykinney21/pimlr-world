using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace TS.Generics
{
    public class ApiDataFetchAndObjectSpawnSpaceshipRace : MonoBehaviour
    {
        public string URL;  // CURRENT API USED https://www.idea-labs.xyz/api/humanityrocks/vehicles/1

        public PlayerSpaceshipInformation currentPlayerSoaceshipInformation;

        public void GetData()
        {
            StartCoroutine(FetchData());
        }

        IEnumerator FetchData()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);

                    JsonUtility.FromJsonOverwrite(request.downloadHandler.text, currentPlayerSoaceshipInformation);

                    SpawnObject();
                }
            }
        }

        [ContextMenu("GET DATA")]
        public void GetDataForCheck()
        {
            StartCoroutine(FetchDataForCheck());
        }

        IEnumerator FetchDataForCheck()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL))
            {
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    Debug.Log(request.downloadHandler.text);

                    JsonUtility.FromJsonOverwrite(request.downloadHandler.text, currentPlayerSoaceshipInformation);
                }
            }
        }

        [ContextMenu("RESET DATA")]
        public void ResetData()
        {
            currentPlayerSoaceshipInformation = new PlayerSpaceshipInformation();
        }

        [ContextMenu("TEST JSON DATA")]
        public void TestDataToJson()
        {

        }


        private void Start()
        {
            GetData();
        }


        [ContextMenu("OBJECT SPAWNER")]
        void SpawnObject()
        {

        }



    }

    [Serializable]
    public class PlayerSpaceshipInformation
    {
        public int id;

        public string spaceship;

        public string character;

        public string flame;

        public string created_at;

        public string updated_at;
    }
}
