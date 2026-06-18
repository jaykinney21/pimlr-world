using UnityEngine;

namespace CarControllerwithShooting
{
    public class CarSystemManager : MonoBehaviour
    {
        public ControllerType controllerType;
        public CameraType cameraType;
        public bool ShowRadar = true;

        //public GameObject cameraFPS;
        public GameObject cameraTPS;
        public static CarSystemManager Instance;

        public bool isWeaponsActive = true;

        public void Awake()
        {
            Instance = this;

            if (cameraTPS == null)
                cameraTPS = Camera.main.gameObject;

            //// Find the FPSCamera and assign it to cameraFPS
            //Transform fpsCameraTransform = transform.Find("AdvancedCarSystem/Vehicle/Chasis/Body/FPSCamera");
            //if (fpsCameraTransform != null)
            //{
            //    cameraFPS = fpsCameraTransform.gameObject;
            //}
            //else
            //{
            //    Debug.LogWarning("FPSCamera not found under AdvancedCarSystem/Vehicle/Chasis/Body!");
            //}
        }

        private void Start()
        {
            if (controllerType == ControllerType.KeyboardMouse)
            {
                //GameCanvas.Instance.Configure_For_PCConsole();
                //Cursor.visible = false;
                //GameCanvas.Instance.button_HandBrake.gameObject.SetActive(false);
                //Cursor.lockState = CursorLockMode.Locked;
            }
            else if (controllerType == ControllerType.Mobile)
            {
                //GameCanvas.Instance.Configure_For_Mobile();
            }

            if (cameraType == CameraType.Interior_FPS)
            {

                if (SocketPlayerManager.Instance && SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera)
                    SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera.SetActive(true);
               // cameraTPS.SetActive(false);
            }
            else if (cameraType == CameraType.Outdoor_TPS)
            {
                if (SocketPlayerManager.Instance && SocketPlayerManager.Instance.MyPlayer && SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera)
                    SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera.SetActive(false);
               // cameraTPS.SetActive(true);
            }
            //if (!isWeaponsActive)
            //{
            //    GunController.Instance.DeactivateWeapons();
            //}
        }

        public Transform GetCamera()
        {
            if (cameraType == CameraType.Interior_FPS)
            {
                if (SocketPlayerManager.Instance && SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera)
                    return SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera.transform;
                else
                    return null;
            }
            else
            {
                return cameraTPS.transform;
            }
        }
    }

    public enum ControllerType
    {
        KeyboardMouse,
        Mobile
    }

    public enum CameraType
    {
        Interior_FPS,
        Outdoor_TPS
    }
}
