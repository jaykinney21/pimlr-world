// Description: VehicleRef: Access from anywhere to vehicleInfo.
// Use by CamDuringCountdownAssistant | CountdownAssistant | LapCOunterAndPosition | LapCounterBadge | MiniMapManager
// StepAssistantModes | VehicleAI | VehicleFlagManager | VehicleFlagOnCam | VehicleVisibleByTheCamList
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace TS.Generics
{
    public class VehiclesRef : MonoBehaviour
    {
        public static VehiclesRef instance = null;
        public List<VehicleInfo> listVehicles = new List<VehicleInfo>();
        public bool b_InitDone;
        private bool b_InitInProgress;
        public VehicleGlobalData vehicleGlobalData;

        //public ChoosablePlayerSpaceshipData choosablePlayerSpaceshipData;
        public string URL = "https://www.idea-labs.xyz/api/humanityrocks/vehicles/1";

        void Awake()
        {
            //-> Check if instance already exists
            if (instance == null)
                instance = this;
        }



        //-> Time Trial Step 0:  -> Instantiate Vehicle
        public bool bInstantiateVehicle(List<int> vehicles)
        {
            #region
            //-> Play the coroutine Once
            if (!b_InitInProgress)
            {
                b_InitInProgress = true;
                b_InitDone = false;
                //Debug.Log(":::::>>>>>>bInstantiateVehicle");
                //StartCoroutine(bInstantiateVehicleRoutine(vehicles));
                StartCoroutine(FetchDataForVehicleModel(vehicles));
            }
            //-> Check if the coroutine is finished
            else if (b_InitDone)
                b_InitInProgress = false;

            //StartCoroutine(FetchDataForVehicleModel(vehicles));

            return !b_InitDone;
            #endregion
        }
        IEnumerator FetchDataForVehicleModel(List<int> vehicles)
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
                    Debug.Log("Vehicle Data::::::::::>>>>>>>" + request.downloadHandler.text);

                    JsonUtility.FromJsonOverwrite(request.downloadHandler.text, vehicleGlobalData.carParametersList[0].currentPlayerSpaceshipInformation);
                    StartCoroutine(bInstantiateVehicleRoutine(vehicles));
                }
            }
        }

        IEnumerator bInstantiateVehicleRoutine(List<int> vehicles)
        {
            #region
            b_InitDone = false;
            Debug.Log("Vehicles: " + vehicles.Count);
            for (var i = 0; i < vehicles.Count; i++)
            {
                //VehicleSpaceshipGenerator newVehicle = Instantiate(vehicleGlobalData.carParametersList[vehicles[i]].Prefab.GetComponent<VehicleSpaceshipGenerator>());
                VehicleSpaceshipGenerator newVehicle = null;

                #region SPACESHIP ASSIGNING PLACE


                //GameObject currentSpaceShip = null;
                if (i == 0 && !string.IsNullOrEmpty(vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship)) //FOR THE PLAYER SINCE THE PLAYER VEHICLE INSTANTIATES FIRST AS PER THIS IMPORTED MODULE
                {

                    //Debug.Log(":::::::>>>>>>>>>>>>" + vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship);

                    #region OLD
                    //switch (vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship)
                    //{
                    //    case "black shuttile":

                    //        Instantiate(choosablePlayerSpaceshipData.spaceships[0], newVehicle.SpaceshipGenerationSpace.transform);
                    //        break;

                    //    case "white shuttle":

                    //        Instantiate(choosablePlayerSpaceshipData.spaceships[1], newVehicle.SpaceshipGenerationSpace.transform);
                    //        break;

                    //    case "green shuttle":

                    //        Instantiate(choosablePlayerSpaceshipData.spaceships[2], newVehicle.SpaceshipGenerationSpace.transform);
                    //        break;

                    //    case "lava shuttle":
                    //        Instantiate(choosablePlayerSpaceshipData.spaceships[3], newVehicle.SpaceshipGenerationSpace.transform);
                    //        break;

                    //    default:

                    //        Instantiate(choosablePlayerSpaceshipData.spaceships[0], newVehicle.SpaceshipGenerationSpace.transform);
                    //        break;
                    //}
                    #endregion

                    for (int j = 0; j < vehicleGlobalData.carParametersList.Count; j++)
                    {
                        if ((vehicleGlobalData.carParametersList[j].name).ToLower() == (vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship).ToLower())
                        {
                            // currentSpaceShip = Instantiate(vehicleGlobalData.carParametersList[j].Prefab, newVehicle.SpaceshipGenerationSpace.transform);
                            newVehicle = Instantiate(vehicleGlobalData.carParametersList[j].Prefab.GetComponent<VehicleSpaceshipGenerator>());
                            //Debug.Log(":::::::::::::>>" +j +"&&&&&"+vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship);
                            break;
                        }
                        else
                        {
                            //Debug.Log(":::::::::::::>>$$$" + (vehicleGlobalData.carParametersList[j].name).ToLower() + ":::::" + (vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship).ToLower());
                        }
                    }

                    if (newVehicle == null)
                    {
                        newVehicle = Instantiate(vehicleGlobalData.carParametersList[vehicles[0]].Prefab.GetComponent<VehicleSpaceshipGenerator>());
                        //Debug.Log(":::::::::::::>>[0]::::" + newVehicle.name);
                    }
                }
                else /*if (newVehicle==null &&!string.IsNullOrEmpty(vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.spaceship))*/
                {
                    //Debug.Log("Spwan Random");
                    newVehicle = Instantiate(vehicleGlobalData.carParametersList[UnityEngine.Random.Range(0, vehicleGlobalData.carParametersList.Count)].Prefab).GetComponent<VehicleSpaceshipGenerator>(); //RANDOM SPACESHIP GENERATOR

                }
                //else
                //{
                //    Debug.Log(" Not Spwan Random");
                //}
                if (newVehicle != null)
                {
                    Debug.Log("::::::>>>>>>>>" + newVehicle.gameObject.name);
                    newVehicle.gameObject.name = "Player_" + i;
                }


                //
                #endregion

                #region CHARACTER ASSIGNING PLACE

                if (newVehicle != null)
                {
                    if (i == 0) //FOR THE PLAYER SINCE THE PLAYER VEHICLE INSTANTIATES FIRST AS PER THIS IMPORTED MODULE
                    {
                        GameObject currentCharacter = null;
                        for (int j = 0; j < vehicleGlobalData.CharacterList.Count; j++)
                        {
                            if (vehicleGlobalData.CharacterList[j].Id == vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.character)
                            {
                                currentCharacter = Instantiate(vehicleGlobalData.CharacterList[j].targetObject, newVehicle.CharacterGenerationSpace.transform);
                                break;
                            }
                        }

                        if (currentCharacter == null)
                        {
                            currentCharacter = Instantiate(vehicleGlobalData.CharacterList[0].targetObject, newVehicle.CharacterGenerationSpace.transform);
                        }


                        #region With out Optimize
                        //switch (vehicleGlobalData.carParametersList[vehicles[i]].currentPlayerSpaceshipInformation.character)
                        //{
                        //    case "stryker":

                        //        Instantiate(choosablePlayerSpaceshipData.characters[0], newVehicle.GetComponent<VehicleSpaceshipGenerator>().CharacterGenerationSpace.transform);
                        //        break;

                        //    case "yogi":

                        //        Instantiate(choosablePlayerSpaceshipData.characters[1], newVehicle.GetComponent<VehicleSpaceshipGenerator>().CharacterGenerationSpace.transform);
                        //        break;

                        //    case "papi":

                        //        Instantiate(choosablePlayerSpaceshipData.characters[2], newVehicle.GetComponent<VehicleSpaceshipGenerator>().CharacterGenerationSpace.transform);
                        //        break;

                        //    case "christine":

                        //        Instantiate(choosablePlayerSpaceshipData.characters[3], newVehicle.GetComponent<VehicleSpaceshipGenerator>().CharacterGenerationSpace.transform);
                        //        break;

                        //    default:

                        //        Instantiate(choosablePlayerSpaceshipData.characters[0], newVehicle.GetComponent<VehicleSpaceshipGenerator>().CharacterGenerationSpace.transform);
                        //        break;
                        //}
                        #endregion

                    }
                    else
                    {
                        Instantiate(vehicleGlobalData.CharacterList[UnityEngine.Random.Range(0, vehicleGlobalData.CharacterList.Count)].targetObject, newVehicle.CharacterGenerationSpace.transform); //RANDOM SPACESHIP GENERATOR
                    }
                }

                #endregion


                #region Flame ASSIGNING PLACE
                if (newVehicle != null)
                {
                    if (i == 0) //FOR THE PLAYER SINCE THE PLAYER VEHICLE INSTANTIATES FIRST AS PER THIS IMPORTED MODULE
                    {
                        GameObject currentFlame = null;
                        for (int k = 0; k < vehicleGlobalData.FlameList.Count; k++)
                        {
                            if (vehicleGlobalData.FlameList[k].Id == vehicleGlobalData.carParametersList[vehicles[k]].currentPlayerSpaceshipInformation.flame)
                            {
                                currentFlame = Instantiate(vehicleGlobalData.FlameList[k].targetObject, newVehicle.FlameGenerationSpaceRightSide.transform);
                                currentFlame = Instantiate(vehicleGlobalData.FlameList[k].targetObject, newVehicle.FlameGenerationSpaceLeftSide.transform);
                                break;
                            }
                        }
                        if (currentFlame == null)
                        {
                            //Instantiate(vehicleGlobalData.FlameList[0].targetObject, newVehicle.FlameGenerationSpaceRightSide.transform);
                            //Instantiate(vehicleGlobalData.FlameList[0].targetObject, newVehicle.FlameGenerationSpaceLeftSide.transform);
                        }
                    }
                    else
                    {
                        int selectedFlame = UnityEngine.Random.Range(0, vehicleGlobalData.FlameList.Count);
                        Instantiate(vehicleGlobalData.FlameList[selectedFlame].targetObject, newVehicle.FlameGenerationSpaceRightSide.transform);
                        Instantiate(vehicleGlobalData.FlameList[selectedFlame].targetObject, newVehicle.FlameGenerationSpaceLeftSide.transform);
                    }
                }

                #endregion
                //-> Path exist

                if (newVehicle != null)
                {
                    if (PathRef.instance.Track && PathRef.instance.Track.checkpoints.Count > 2)
                    {
                        yield return new WaitUntil(() => StartLine.instance.ReturnGridPosition(i, newVehicle.transform));
                    }
                    else
                    {
                        newVehicle.transform.position = StartLine.instance.Grp_StartLineColliders.transform.position;
                        newVehicle.transform.rotation = StartLine.instance.Grp_StartLineColliders.transform.rotation;
                        newVehicle.transform.localEulerAngles += new Vector3(0, 180, 0);
                    }

                    //-> Init vehicle for Test P1 + No Collision
                    if (InfoRememberMainMenuSelection.instance.playerMainMenuSelection.currentGameMode == 5)
                    {
                        //Debug.Log("New Car");
                        newVehicle.GetComponent<VehiclePrefabInit>().bInitVehicleInfo(2, i, vehicles[i]);
                    }
                    //-> Init vehicle for a race
                    else
                        newVehicle.GetComponent<VehiclePrefabInit>().bInitVehicleInfo(0, i, vehicles[i]);


                    yield return new WaitUntil(() => newVehicle.GetComponent<VehiclePrefabInit>().b_InitDone);

                    listVehicles.Add(newVehicle.GetComponent<VehiclePrefabInit>().vehicleInfo);
                }
            }

            b_InitDone = true;

            //Debug.Log("Time Trial Step 0:  -> Instantiate Vehicle Done: " + vehicles.Count);

            yield return null;
            #endregion
        }
    }

}
[Serializable]
public class ChoosablePlayerSpaceshipData
{
    public List<GameObject> spaceships;

    public List<GameObject> characters;

    public List<GameObject> flames;
}

