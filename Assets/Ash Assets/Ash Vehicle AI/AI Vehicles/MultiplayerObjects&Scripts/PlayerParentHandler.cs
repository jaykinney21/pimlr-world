//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Mirror;
//using AshVP;
//using Cinemachine;
//using System.Linq;

//namespace QuickStart 
//{
//    public class PlayerParentHandler : NetworkBehaviour {

//        public PlayerCarContrtoller currentPlayerCarController;

//        //public WaypointProgressTracker currentPlayerProgressTracker;

//        public CinemachineVirtualCamera virtualCamera;


//        public override void OnStartLocalPlayer () 
//        {
//            base.OnStartLocalPlayer();

//            if(TrackManager.instance.playerCarContrtollers.Count > 0)
//            {
//                for (int i = 0; i < TrackManager.instance.playerCarContrtollers.Count; i++)
//                {
//                    TrackManager.instance.playerCarContrtollers[i].thisPlayer = false;

//                    TrackManager.instance.currentPlayerCamera[i].SetActive(false);
//                }
//            }

//            TrackManager.instance.playerCarContrtollers.Add(currentPlayerCarController);

//            TrackManager.instance.currentPlayerCamera.Add(virtualCamera.gameObject);

//            currentPlayerCarController.thisPlayer = true;

//            Invoke(nameof(InvokedCameraAwaken), 0.3f);
//            //currentPlayerProgressTracker.circuit = TrackManager.instance.circuit;
//        }

//        void InvokedCameraAwaken ()
//        {
//            virtualCamera.gameObject.SetActive(true);
//        }

//        // Start is called before the first frame update
//        void Start () 
//        {
//            //currentPlayerProgressTracker.circuit = TrackManager.instance.circuit;
//        }

//        // Update is called once per frame
//        void Update ()
//        {

//        }
//    }
//}


