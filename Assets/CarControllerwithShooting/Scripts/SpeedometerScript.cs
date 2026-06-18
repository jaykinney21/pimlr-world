using AshVP;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarControllerwithShooting
{
    public class SpeedometerScript : MonoBehaviour
    {
        public CarController controller;
        public AiCarContrtoller aiCarContrtoller;
        public TextMeshProUGUI currentCarSpeedTxt;
        public float minRotation;
        public float maxRotation;

        private float _currentSpeed;
        private float _maxSpeed;
        private float rotation;

        void FixedUpdate()
        {
            if (IsMultiplayerScene())
            {
                HandleMultiplayerScene();
            }
            else if (IsCustomRaceScene())
            {
                HandleLocalGames();
            }
            else
            {
                HandleAIGames();
            }
        }
        public CarController SetController(CarController _controller)
        {
            this.controller = _controller;
            return _controller;
        }
        private bool IsMultiplayerScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return sceneName == "Gameplay(terrain)" || sceneName == "Gameplay(colloseum)";
        }

        private bool IsCustomRaceScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return sceneName == "CustomRace";
        }

        private void HandleMultiplayerScene()
        {
            //Debug.Log("Multiplayer");
            if (SocketPlayerManager.Instance.MyPlayer && SocketPlayerManager.Instance.MyPlayer.controller)
            {
                //Debug.Log("IF Multiplayer");   
                _currentSpeed = SocketPlayerManager.Instance.MyPlayer.controller.CurrentSpeed;
                currentCarSpeedTxt.text = ((int)_currentSpeed).ToString();
                _maxSpeed = SocketPlayerManager.Instance.MyPlayer.controller.MaxSpeed;
                rotation = Mathf.Lerp(minRotation, maxRotation, _currentSpeed / _maxSpeed);
                transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            }
        }

        private void HandleLocalGames()
        {
           // Debug.Log("Local Games");
            _currentSpeed = controller.CurrentSpeed;
            currentCarSpeedTxt.text = ((int)_currentSpeed).ToString();
            _maxSpeed = controller.MaxSpeed;
            rotation = Mathf.Lerp(minRotation, maxRotation, _currentSpeed / _maxSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        private void HandleAIGames()
        {
            //Debug.Log("Local Games");
            _currentSpeed = Mathf.Clamp(aiCarContrtoller.speedValue / 1000, 0f, 160f);
           // Debug.Log("_currentSpeed: " + _currentSpeed);
            //_currentSpeed = aiCarContrtoller.SpeedAI;
            currentCarSpeedTxt.text = ((int)_currentSpeed).ToString();
            _maxSpeed = 160f;
            rotation = Mathf.Lerp(minRotation, maxRotation, _currentSpeed / _maxSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }

        /*void FixedUpdate()
        {
            if (IsMultiplayerScene())
            {
                Debug.Log("Multiplayer");

                if (GameplayManager.instance.player && GameplayManager.instance.player.controller)
                {
                    UpdateSpeedValues(GameplayManager.instance.player.controller);
                }
            }
            else
            {
                Debug.Log("Local Games");
                UpdateSpeedValues(controller);
            }

            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }
        private bool IsMultiplayerScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return sceneName == "Gameplay(terrain)" || sceneName == "Gameplay(colloseum)";
        }

        private void UpdateSpeedValues(CarController targetController)
        {
            _currentSpeed = targetController.CurrentSpeed;
            currentCarSpeedTxt.text = ((int)_currentSpeed).ToString();
            _maxSpeed = targetController.MaxSpeed;
            rotation = Mathf.Lerp(minRotation, maxRotation, _currentSpeed / _maxSpeed);
            transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        }*/


        /* void FixedUpdate()
         {

             if (IsMultiplayerScene())
             {
                 Debug.Log("Multiplayer");
                 if (GameplayManager.instance.player && GameplayManager.instance.player.controller)
                 {
                     Debug.Log("GameplayManager.instance.player.controller:" + GameplayManager.instance.player.controller.gameObject.name);
                     _currentSpeed = GameplayManager.instance.player.controller.CurrentSpeed;
                     _maxSpeed = GameplayManager.instance.player.controller.MaxSpeed;
                     rotation = Mathf.Lerp(minRotation, maxRotation, _currentSpeed / _maxSpeed);
                     transform.rotation = Quaternion.Euler(0f, 0f, rotation);
                     Debug.Log("rotation:" + rotation);
                 }
             }
             else
             {
                 Debug.Log("Local Games");
                 transform.rotation = Quaternion.Euler(0f, 0f, rotation);
                 _currentSpeed = controller.CurrentSpeed;
                 currentCarSpeedTxt.text = ((int)_currentSpeed).ToString();
                 _maxSpeed = controller.MaxSpeed;
                 rotation = Mathf.Lerp(minRotation, maxRotation, _currentSpeed / _maxSpeed);
                 transform.rotation = Quaternion.Euler(0f, 0f, rotation);
             }
         }
         private bool IsMultiplayerScene()
         {
             string sceneName = SceneManager.GetActiveScene().name;
             return sceneName == "Gameplay(terrain)" || sceneName == "Gameplay(colloseum)";
         }*/

    }
}
