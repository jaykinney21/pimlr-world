
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SocketPlayerManager : MonoBehaviour
{
    public static SocketPlayerManager Instance { get; private set; }
    public PlayerID MyPlayer;
    public string playerName;
    public string player_Id;
    public bool IsMaster = false;
    public int MaxPlayer = 10;


    public HelixPlayerInfo helixPlayerInfo;

  
    public Dictionary<string, HelixPlayerInfo> helixPlayerInfoList = new Dictionary<string, HelixPlayerInfo>();
    public Dictionary<string, PlayerID> allPlayers = new Dictionary<string, PlayerID>();



  

    //public delegate void MissileFiredHandler(Transform missileTransform, string playerId);
    //public static event MissileFiredHandler OnMissileFired;


    #region Region Name
    [Header("Region Name")]
    private List<string> regionNames = new List<string>() { "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua & Deps", "Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan", "Bolivia", "Bosnia Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria", "Burkina", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Central African Rep", "Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Congo {Democratic Rep}", "Costa Rica", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "East Timor", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq", "Ireland {Republic}", "Israel", "Italy", "Ivory Coast", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea North", "Korea South", "Kosovo", "Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya", "Liechtenstein", "Lithuania", "Luxembourg", "Macedonia", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia", "Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar, {Burma}", "Namibia", "Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Norway", "Oman", "Pakistan", "Palau", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar", "Romania", "Russian Federation", "Rwanda", "St Kitts & Nevis", "St Lucia", "Saint Vincent & the Grenadines", "Samoa", "San Marino", "Sao Tome & Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Swaziland", "Sweden", "Switzerland", "Syria", "Taiwan", "Tajikistan", "Tanzania", "Thailand", "Togo", "Tonga", "Trinidad & Tobago", "Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Vatican City", "Venezuela", "Vietnam", "Yemen", "Zambia", "Zimbabwe" };
    #endregion
    public string randomRegionName;

    private void Start()
    {
        randomRegionName = GetRandomRegionName();
        helixPlayerInfo.userRegion = randomRegionName;       
    }


    string GetRandomRegionName()
    {
        // Generate a random index within the range of the list
        int randomIndex = Random.Range(0, regionNames.Count);

        // Return the region name at the randomly selected index
        return regionNames[randomIndex];
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);

        }
        else Destroy(this.gameObject);

    }
    public void LeaveRoom()
    {
        IsMaster = false;
    }
    public void OnOthorplayerLeft(Data data)
    {

        allPlayers.Remove(data.left_player_id);
        helixPlayerInfoList.Remove(data.left_player_id);
    }

    public void MyPlayerLeft()
    {

        helixPlayerInfoList.Clear();
        allPlayers.Clear();
    }


}
[System.Serializable]
public class HelixPlayerInfo
{
    public string userName;
    public string userID;
    public bool isLocalUser = false;
    public string userRegion;
    public string roomID;
    public string roomName;
    public bool isRoomHostUser = false;
    public int roomMaxPlayer = 10;
    public int roomCurrentPlayer;
    public bool teamA = false, teamB = false;
    public bool userReadyRoom = false, userStartRoom = false;
    public int userKillPoint;
    public int team_A_KillPoint;
    public int team_B_KillPoint;

}

