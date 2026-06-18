using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSelection : MonoBehaviour
{
    public void GoToLobby()
    {
        SocketUIManager.Instance.SetScreen(SocketScreens.Helix_Your_Team);
    }
}
