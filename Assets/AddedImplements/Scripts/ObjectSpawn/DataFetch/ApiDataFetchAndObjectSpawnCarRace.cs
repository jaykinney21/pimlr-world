using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ApiDataFetchAndObjectSpawnCarRace : MonoBehaviour
{
    public string URL;  // CURRENT API USED https://nft.thecela.com/helix/api/configurator/1

    public PlayerInformation currentPlayerInformation;

    [Space]

    public GameObject HunterPrefab;

    public GameObject HammerPrefab;

    public GameObject HeroPrefab;

    public GameObject HackerPrefab;

    public GameObject HypePrefab;

    public void GetData () 
    {
        StartCoroutine(FetchData());
    }

    IEnumerator FetchData () {
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

                JsonUtility.FromJsonOverwrite(request.downloadHandler.text, currentPlayerInformation);

                JsonUtility.FromJsonOverwrite(currentPlayerInformation.attributes, currentPlayerInformation.playerAttributes);

                //#region ARRAY OF OBJECTS CONVERSION
                JSONObject newJsonObject = new JSONObject(currentPlayerInformation.weapons);

                JsonUtility.FromJsonOverwrite(currentPlayerInformation.weapons, currentPlayerInformation.playerWeapons);

                //PlayerWeapons currentPlayerWeapons = new();
                //currentPlayerInformation.playerWeapons.Clear();

                //if (newJsonObject.Count > 1)
                //{
                //    for (int i = 0; i < newJsonObject.Count; i++)
                //    {
                //        JsonUtility.FromJsonOverwrite(newJsonObject[i].ToString(), currentPlayerWeapons);

                //        currentPlayerInformation.playerWeapons.Add(currentPlayerWeapons);
                //    }
                //}
                //else if (newJsonObject.Count == 1)
                //{
                //    JsonUtility.FromJsonOverwrite(newJsonObject[0].ToString(), currentPlayerWeapons);

                //    currentPlayerInformation.playerWeapons.Add(currentPlayerWeapons);
                //}
                //#endregion

                SpawnObject();
            }
        }
    }

    [ContextMenu("GET DATA")]
    public void GetDataForCheck () {
        StartCoroutine(FetchDataForCheck());
    }

    IEnumerator FetchDataForCheck () {
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

                JsonUtility.FromJsonOverwrite(request.downloadHandler.text, currentPlayerInformation);

                JsonUtility.FromJsonOverwrite(currentPlayerInformation.attributes, currentPlayerInformation.playerAttributes);

                //#region ARRAY OF OBJECTS CONVERSION
                JSONObject newJsonObject = new JSONObject(currentPlayerInformation.weapons);

                //var str = newJsonObject.GetField("Weapon 1").ToString();
                //
                //JSONObject weaponJsonObject = new JSONObject(currentPlayerInformation);

                JsonUtility.FromJsonOverwrite(currentPlayerInformation.weapons, currentPlayerInformation.playerWeapons);

                //PlayerWeapons currentPlayerWeapons = new();
                //currentPlayerInformation.playerWeapons.Clear();

                //if(newJsonObject.Count > 1)
                //{
                //    for(int i = 0; i < newJsonObject.Count; i++)
                //    {
                //        JsonUtility.FromJsonOverwrite(newJsonObject[i].ToString(), currentPlayerWeapons);

                //        currentPlayerInformation.playerWeapons.Add(currentPlayerWeapons);
                //    }
                //}
                //else if(newJsonObject.Count == 1)
                //{
                //    JsonUtility.FromJsonOverwrite(newJsonObject[0].ToString(), currentPlayerWeapons);

                //    currentPlayerInformation.playerWeapons.Add(currentPlayerWeapons);
                //}
                //#endregion
            }
        }
    }

    [ContextMenu("RESET DATA")]
    public void ResetData () 
    {
        currentPlayerInformation = new PlayerInformation();
    }

    [ContextMenu("TEST JSON DATA")]
    public void TestDataToJson () 
    {
        Debug.Log(JsonUtility.ToJson(currentPlayerInformation)); 
    }


    private void Start ()
    {
        GetData();
    }


    [ContextMenu("OBJECT SPAWNER")]
    void SpawnObject () 
    {
        GameObject newVehicle;

        ColorChangeAndWeaponAssign meshScript;

        #region COLORS

        Color bland = new Color32(255, 255, 255, 255);



        Color wheel_light = new Color32(255, 255, 255, 255);

        Color body = new Color32(255, 255, 255, 255);

        Color molding = new Color32(255, 255, 255, 255);

        Color frame = new Color32(255, 255, 255, 255);

        Color inner_frame = new Color32(255, 255, 255, 255);

        Color logo = new Color32(255, 255, 255, 255);

        Color bumpers = new Color32(255, 255, 255, 255);

        Color spoiler = new Color32(255, 255, 255, 255);

        Color front_rear_fascia = new Color32(255, 255, 255, 255);

        Color accent_lights = new Color32(255, 255, 255, 255);

        Color led_logo = new Color32(255, 255, 255, 255);

        Color cargo_cover = new Color32(255, 255, 255, 255);

        Color front_grille = new Color32(255, 255, 255, 255);

        Color warning_lights = new Color32(255, 255, 255, 255);

        Color upper_body = new Color32(255, 255, 255, 255);

        Color lower_body = new Color32(255, 255, 255, 255);

        Color rear_frame = new Color32(255, 255, 255, 255);

        Color accent_piece_one = new Color32(255, 255, 255, 255);
        #endregion

        string[] colorArray;

        byte subRed;

        byte subGreen;

        byte subBlue;

        byte subAlpha = 255;

        switch(currentPlayerInformation.title) 
        {
            case "Hunter":

                newVehicle = Instantiate(HunterPrefab, this.transform);

                meshScript = newVehicle.GetComponent<ColorChangeAndWeaponAssign>();

                #region COLOR SETTINGS

                #region WHEEL LIGHT

                if (currentPlayerInformation.playerAttributes.wheel_light.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.wheel_light.color.Substring(4, currentPlayerInformation.playerAttributes.wheel_light.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    wheel_light = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region BODY

                if (currentPlayerInformation.playerAttributes.body.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.body.color.Substring(4, currentPlayerInformation.playerAttributes.body.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    body = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region FRAME

                if (currentPlayerInformation.playerAttributes.frame.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.frame.color.Substring(4, currentPlayerInformation.playerAttributes.frame.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    frame = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region INNER FRAME

                if (currentPlayerInformation.playerAttributes.inner_frame.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.inner_frame.color.Substring(4, currentPlayerInformation.playerAttributes.inner_frame.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    inner_frame = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region LOGO

                if (currentPlayerInformation.playerAttributes.logo.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.logo.color.Substring(4, currentPlayerInformation.playerAttributes.logo.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    logo = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                meshScript.ChangePartsColor_Hunter(body, frame, inner_frame, bland, logo, wheel_light);

                #endregion

                break;
            case "Hacker":

                newVehicle = Instantiate(HackerPrefab, this.transform);

                meshScript = newVehicle.GetComponent<ColorChangeAndWeaponAssign>();

                #region COLOR SETTINGS

                #region LED LOGO

                if (currentPlayerInformation.playerAttributes.led_logo.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.led_logo.color.Substring(4, currentPlayerInformation.playerAttributes.led_logo.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    led_logo = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region UPPER BODY

                if (currentPlayerInformation.playerAttributes.upper_body.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.upper_body.color.Substring(4, currentPlayerInformation.playerAttributes.upper_body.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    upper_body = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region LOWER BODY

                if (currentPlayerInformation.playerAttributes.lower_body.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.lower_body.color.Substring(4, currentPlayerInformation.playerAttributes.lower_body.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    lower_body = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                #region ACCENT PIECE ONE

                if (currentPlayerInformation.playerAttributes.accent_piece_one.color != "original")
                {
                    colorArray = currentPlayerInformation.playerAttributes.accent_piece_one.color.Substring(4, currentPlayerInformation.playerAttributes.accent_piece_one.color.Length - 4 - 1).Split(char.Parse(","));

                    subRed = (byte)int.Parse(colorArray[0]);

                    subGreen = (byte)int.Parse(colorArray[1]);

                    subBlue = (byte)int.Parse(colorArray[2]);

                    accent_piece_one = new Color32(subRed, subGreen, subBlue, subAlpha);
                }
                #endregion

                meshScript.ChangePartsColor_Hacker(led_logo,upper_body,lower_body,accent_piece_one);

                #endregion
                break;

            case "Hammer":

                newVehicle = Instantiate(HammerPrefab, this.transform);

                meshScript = newVehicle.GetComponent<ColorChangeAndWeaponAssign>();

                break;
            case "Hype":

                newVehicle = Instantiate(HypePrefab, this.transform);

                meshScript = newVehicle.GetComponent<ColorChangeAndWeaponAssign>();

                break;
            case "Hero":

                newVehicle = Instantiate(HeroPrefab, this.transform);

                meshScript = newVehicle.GetComponent<ColorChangeAndWeaponAssign>();

                break;
            default:
                Debug.Log("VEHICLE DOES NOT EXIST");
                break;
        }
    }

}

[Serializable]
public class PlayerInformation 
{
    public int id;

    public string title;

    public int wheelLight;

    public string rimType;

    public int wheelSize;

    public string attributes;

    public PlayerAttributes playerAttributes;

    [Space]

    public string weapons;

    //public PlayerWeapons playerWeapons;

    public List<PlayerWeapons> playerWeapons;

    [Space]

    public string created_at;

    public string updated_at;
    
}

[Serializable]
public class PlayerAttributes 
{
    public PlayerAttributeValues body;

    public PlayerAttributeValues molding;

    public PlayerAttributeValues frame;

    public PlayerAttributeValues inner_frame;

    public PlayerAttributeValues lights;

    public PlayerAttributeValues logo;

    public PlayerAttributeValues wheel_light;

    public PlayerAttributeValues bumpers;

    public PlayerAttributeValues spoiler;

    public PlayerAttributeValues accent_lights;

    public PlayerAttributeValues front_rear_fascia;

    public PlayerAttributeValues led_logo;

    public PlayerAttributeValues cargo_cover;

    public PlayerAttributeValues front_grille;

    public PlayerAttributeValues warning_lights;

    public PlayerAttributeValues upper_body;

    public PlayerAttributeValues lower_body;

    public PlayerAttributeValues rear_frame;

    public PlayerAttributeValues accent_piece_one;

    public PlayerAttributeValues accent_piece_two;

    public PlayerAttributeValues led_lights;
}


[Serializable]
public class PlayerWeapons 
{
    public string type;

    public string color;

    public string weapon;
}


[Serializable]
public class PlayerAttributeValues 
{
    public string title;

    public string color;
}
