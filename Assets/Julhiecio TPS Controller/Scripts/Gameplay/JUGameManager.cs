using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JUTPS
{
	[AddComponentMenu("JU TPS/Gameplay/Game/Game Manager")]
	public class JUGameManager : MonoBehaviour
	{
		[HideInInspector] public static JUCharacterController InstancedPlayer;

		public static bool IsMobile;
		[SerializeField] private bool SimulateMobileDevice = false;

		void Start()
		{
			if (InstancedPlayer == null)
			{
				GameObject player_obj = GameObject.FindGameObjectWithTag("Player");
				InstancedPlayer = player_obj != null ? player_obj.GetComponent<JUCharacterController>() : null;
			}

			IsMobile = SimulateMobileDevice ? true : (SystemInfo.deviceType == DeviceType.Handheld);

			AuthManager.Instance.currentGameMode = GameMode.Pimlr;
		}
		private void OnDestroy()
		{
			InstancedPlayer = null;
		}

        private void Update()
        {
		//	LoadIDEAMainLandScene();
        }

        public void LoadIDEAMainLandScene()
        {
			if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("YannicksWorld - Copy") && Input.GetKeyDown(KeyCode.Escape)) 
			{
				SceneManager.LoadScene("IDEAMainLand");
			}
        }

    }

}