using UnityEngine;
using UnityEngine.InputSystem;

namespace CarControllerwithShooting
{
    public class PlayerInput : MonoBehaviour
    {
        private CarController _carController;


        [Header("Camera Rotetion")]
        // Variables

        public float mouseSensitivity = 2f;
        //float cameraVerticalRotation = 0f;

        //bool lockedCursor = true;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;





        private const float _threshold = 0.01f;



        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;


        public bool LockCameraPosition = false;


        private void Awake()
        {
            _carController = GetComponent<CarController>();
            if (_carController.playerid.isLocalPlayer)
                _cinemachineTargetYaw = _carController.camTarget.transform.rotation.eulerAngles.y;

        }
        private void Start()
        {




            if (_carController.playerid.isLocalPlayer)
            {
                _cinemachineTargetYaw = _carController.camTarget.transform.rotation.eulerAngles.y;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = true;

            }

        }

        private void FixedUpdate()
        {
            // Only drive the car if this is the local player's instance
            _carController.Move();



            if (_carController && _carController.playerid && _carController.playerid.isLocalPlayer && Input.GetMouseButtonDown(0) && !SceneManagerScript.Instance.IsPointerOverUIObject() && Cursor.lockState != CursorLockMode.Locked && GameCanvas.Instance && !GameCanvas.Instance.leaderboardBattleRoyale.gameObject.activeSelf)
            {
                _cinemachineTargetYaw = _carController.camTarget.transform.rotation.eulerAngles.y;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab) && _carController && _carController.playerid && _carController.playerid.isLocalPlayer)
            {

                Debug.Log("Unload");
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }


        //Vector2 oldPos = Vector2.zero;
        //Vector2 newPos = Vector2.zero;
        //Vector2 diff;




        //float rotationX = 0f;
        //float rotationY = 0f;
        public float sensitivity = 15f;


        [Header("Mouse sensitivity")]
        public float mousesensitivity = 1;

        Vector2 pointerPos = Vector2.zero;




        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.V) || Input.GetMouseButton(1) ) &&  _carController.playerid.isLocalPlayer && _carController.currentSecondaryPowerUp != null  && _carController.currentSecondaryPowerUp.missilePowerUp==null && !_carController.currentSecondaryPowerUp.isUsed)
            {

                _carController.currentSecondaryPowerUp.OnStartUsePowerUp(_carController.playerid.playerID, true);
            }

            //if (_carController != null && _carController.playerid.isLocalPlayer)
            //{
            //    //newPos = Input.mousePosition;
            //    //if (newPos != oldPos)
            //    //{
            //    //    diff = newPos - oldPos;
            //    //    oldPos = newPos;
            //    //    CameraRotation(diff);
            //    //}

            //    rotationY += Input.GetAxis("Mouse X") * sensitivity;
            //    rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;


            //    //rotationX = Mathf.Clamp(rotationX, -90, 90);
            //    //rotationY = Mathf.Clamp(rotationX, -180, 180);
            //    //_carController.camTarget.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
            //    _carController.camTarget.transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
            //}


            if (_carController != null && _carController.playerid.isLocalPlayer)
            {
                //rotationY += Input.GetAxis("Mouse X") * sensitivity;
                //rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;

                // newPos = Input.mousePosition;
                //if (newPos != oldPos)
                //{
                //    diff = newPos - oldPos;
                //    oldPos = newPos;
                //    CameraRotation(diff);
                //}


                _cinemachineTargetYaw += pointerPos.x * mousesensitivity;
                _cinemachineTargetPitch += pointerPos.y * mousesensitivity;


                // clamp our rotations so our values are limited 360 degrees
                _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

                // Cinemachine will follow this target
                _carController.camTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                    _cinemachineTargetYaw, 0.0f);


            }
        }
        private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public void OnLook(InputValue value)
        {
            if (_carController.playerid.isLocalPlayer)
                pointerPos = value.Get<Vector2>();

        }

        //void Update()
        //{
        //    rotationy += Input.GetAxis("Mouse X") * sensitivity;
        //    rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;
        //    transform.localEulerAngles = new Vector3(rotationx, rotationY, 0);
        //}


        //private void CameraRotation(Vector2 look)
        //{
        //    float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
        //    float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        //    // Rotate the Camera around its local X axis

        //    cameraVerticalRotation -= inputY;
        //    cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        //    _carController.camTarget.transform.eulerAngles =  Vector3.up * cameraVerticalRotation;


        //    //Debug.Log(Vector3.up * cameraVerticalRotation);

        //    // Rotate the Player Object and the Camera around its Y axis

        // //   _carController.camTarget.transform.Rotate(Vector3.up * inputX);
        //}





        //private void CameraRotation(Vector2 look)
        //{
        //    // if there is an input and camera position is not fixed
        //    if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
        //    {
        //        //Don't multiply mouse input by Time.deltaTime;


        //        _cinemachineTargetYaw += look.x;
        //        _cinemachineTargetPitch += look.y;
        //    }

        //    // clamp our rotations so our values are limited 360 degrees
        //    _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        //    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        //    // Cinemachine will follow this target
        //    _carController.camTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
        //        _cinemachineTargetYaw, 0.0f);
        //}
        //private float ClampAngle(float lfAngle, float lfMin, float lfMax)
        //{
        //    if (lfAngle < -360f) lfAngle += 360f;
        //    if (lfAngle > 360f) lfAngle -= 360f;
        //    return Mathf.Clamp(lfAngle, lfMin, lfMax);
        //}

    }
}
