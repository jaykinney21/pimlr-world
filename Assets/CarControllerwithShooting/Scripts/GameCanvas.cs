using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace CarControllerwithShooting
{
	public class GameCanvas : Singleton<GameCanvas>
	{
		public Button button_Missile;
		public Button button_HandBrake;
		public Button button_Machinegun;
		public GameObject joystick;
		public Button button_CameraChange;
		public int isFiringUpdate = 0;
		public Slider Slider_CurrentFuel;
		public Text Text_CurrentFuel;

		public Text Text_Ammo_Machinegun;
		public Text Text_Ammo_Missile;


		public GameObject GameUI;
		public LeaderboardBattleRoyale leaderboardBattleRoyale;
		public GameObject GasolineUI;
		public TextMeshProUGUI text_speed;
		public TextMeshProUGUI text_timer;
		public TextMeshProUGUI text_RespwanTime;
		public TextMeshProUGUI teamAScoreText, teamBScoreText;

		//public GameObject TimeOverPanel;
		public TextMeshProUGUI winText;

		[Header("AudioSection")]
		public AudioSource musicSystem;
		public AudioClip[] audioList;
		public bool music;
		public string songName;
		public TextMeshProUGUI songNameTXT;
		public Slider musicSlider;
		public GameObject idleSystem, activeSystem;


		[Header("HealthBar")]
		public HelixHealthBar helixHealthBar;
		public TextMeshProUGUI text_health;
		public HelixHealthSlider healthBar;

		public SpeedometerScript speedometerScript;
		public HelixGameplayPlayerInfo helixGameplayPlayerInfo;
		public Transform killParent;
		//public HelixGameplayKillInfo killInfoObject;
		public HelixMiniMapCamera helixMiniMapCamera;

		public Image powerUpImage;
		public TextMeshProUGUI powerUpValue;
		public Image secondarypowerUpImage;
		public TextMeshProUGUI secondaryPowerUpValue;


		[Header("CrossHair")]
		public GameObject crossHair;
		public GameObject crossHairHolder;


		public Sprite scoreBgImage;
		public Image scoreBgImageHolder;


		#region Unity Callbacks


		IEnumerator Start()
		{

			if (SceneManager.GetActiveScene().ToString() == "Gameplay(terrain)" || SceneManager.GetActiveScene().ToString() == "Gameplay(colloseum)")
			{
				yield return new WaitUntil(() => SocketPlayerManager.Instance.MyPlayer != null && SocketPlayerManager.Instance.MyPlayer.controller);
				healthBar.ModifyHealth(SocketPlayerManager.Instance.MyPlayer.controller.Health);

				

			}

		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.C) && CarSystemManager.Instance.controllerType == ControllerType.KeyboardMouse)
			{
				Click_Button_CameraSwitch();
			}

			Update_Text_Health();
			CarSpeedoMeter();
			songNameTXT.text = songName;
			musicSlider.maxValue = musicSystem.clip.length;
			musicSlider.value = musicSystem.time;
			//Update_Text_Timer();
		}
		#endregion

		#region Updating Functions
		public void Update_Text_Health()
		{
			try
			{
				if (SocketPlayerManager.Instance.MyPlayer && SocketPlayerManager.Instance.MyPlayer.controller != null)
				{
					//text_health.text = "HEALTH: " + carController.Health.ToString();
					var _Health = SocketPlayerManager.Instance.MyPlayer.controller.Health;
					text_health.text = $"<size=35>100</size>/<size=25>{_Health}</size>";
				}
				else
				{
					//  Debug.LogWarning("CarController not found on any Vehicle GameObject!");
				}
			}
			catch (Exception e)
			{
				Debug.Log($"GameCanvas ---> Update_Text_Health ---> {e.ToString()}");
			}

		}

		public void CarSpeedoMeter()
		{
			try
			{
				if (SocketPlayerManager.Instance.MyPlayer && SocketPlayerManager.Instance.MyPlayer.controller != null)
				{
					text_speed.text = Convert.ToInt32(SocketPlayerManager.Instance.MyPlayer.controller.speed).ToString();
				}
				else
				{
					//Debug.LogWarning("CarController not found on any Vehicle GameObject!");
				}
			}
			catch (Exception e)
			{
				Debug.Log($"GameCanvas ---> CarSpeedoMeter ---> {e.ToString()}");
			}

		}

		/*public void Update_Text_Timer(int count)
        {

            try
            {
                int minutes = Mathf.FloorToInt(count / 60);
                int seconds = Mathf.FloorToInt(count % 60);

                // Display the time in a specific format (e.g., "00:00")
                string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
                text_timer.text = timeString;
                if (count <= 0f)
                {
                    // Do something when the timer reaches zero (e.g., end the game)
                    WinText();
                    TimeOverPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }
            catch(Exception e)
            {
                Debug.Log($"GameCanvas ---> Update_Text_Timer ---> {e.ToString()}");
            }
        }*/
		//this method call a Update_Text_Timer
		#endregion
		public void WinText()
		{
			try
			{
				/*if (teamAPoints > teamBPoints)
                {
                    leaderboardBattleRoyale.gameObject.SetActive(true);
                    leaderboardBattleRoyale.TeamA();
                    //winText.text = "TEAM-A WIN";
                }
                else if (teamAPoints < teamBPoints)
                {
                    leaderboardBattleRoyale.gameObject.SetActive(true);
                    leaderboardBattleRoyale.TeamB();
                    //winText.text = "TEAM-B WIN";
                }
                else
                {
                    leaderboardBattleRoyale.TeamA();

                    leaderboardBattleRoyale.gameObject.SetActive(true);
                    //winText.text = " TIE ";
                }*/
			}
			catch (Exception e)
			{
				Debug.Log($"GameCanvas ---> WinText ---> {e.ToString()}");
			}

		}
		public void Configure_For_PCConsole()
		{
			joystick.gameObject.SetActive(false);
			button_CameraChange.GetComponentInChildren<Text>().text = "Camera (C)";
		}

		public void Click_Button_CameraSwitch()
		{
			//if (button_CameraChange.IsInteractable())
			//{
			if (SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera != null && SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera.activeSelf)
			{
				SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera.SetActive(false);
				// CarSystemManager.Instance.cameraTPS.SetActive(true);
			}
			else if (CarSystemManager.Instance.cameraTPS != null)
			{

				if (SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera != null)
					SocketPlayerManager.Instance.MyPlayer.controller.FPS_Camera.SetActive(true);
				//CarSystemManager.Instance.cameraTPS.SetActive(false);
			}
			//}
		}


		public void Click_Button_HandBrake_Down()
		{
			//CarController.Instance.handBrake = 1;
		}

		public void Click_Button_HandBrake_Up()
		{
			//CarController.Instance.handBrake = 0;
		}

		public void Hide_GameUI()
		{
			GameUI.SetActive(false);
		}

		public void Click_Button_MachineGun_Down()
		{
			isFiringUpdate = 1;
		}

		public void Click_Button_Guns_Up()
		{
			isFiringUpdate = 0;
		}

		public void Click_Button_Missle_Down()
		{
			isFiringUpdate = -1;
		}

		public void OnExitGameLobbyClick()
		{
			SocketNetworkManager.Instance.LeaveRoom();

		}
		public void OnLoadeScene(string sceneName)
		{

		}

		#region AudioSection

		public void PlayMusic()
		{
			if (music)
			{

				musicSystem.Pause();
				idleSystem.SetActive(true);
				activeSystem.SetActive(false);
				music = false;
			}
			else
			{
				musicSystem.Play();
				songName = musicSystem.clip.name;
				idleSystem.SetActive(false);
				activeSystem.SetActive(true);
				music = true;
			}

		}
		int i = 0;
		public void ChangeMusicForward()
		{

			i++;
			musicSystem.clip = audioList[i];
			songName = musicSystem.clip.name;
			Debug.Log("  " + musicSystem.clip.length);
			musicSystem.Play();
			if (i >= 2)
			{
				i = 0;
			}

		}


		public void FireEffect()
		{
			crossHair.transform.DOScale(1.5f, 0.1f).OnComplete(() => { crossHair.transform.DOScale(1f, 0.1f); });
		}

		public void ChangeMusicBackward()
		{
			musicSystem.clip = audioList[i];
			songName = musicSystem.clip.name;
			musicSystem.Play();
			i--;
			if (i < 0)
			{
				i = 0;
			}
		}
		#endregion




	}
}