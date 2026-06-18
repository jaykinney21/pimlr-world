using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class MusicPlayer : MonoBehaviour
{
    public enum ReplayState
    {
        NoRepeat,
        RepeatAll,
        RepeatSingle
    }

    [SerializeField] MusicLibrary m_MusicLibrary;
    [SerializeField] AudioSource m_AudioSource;
    [SerializeField] Image m_AlbumImage;

    // SLIDERS
    [SerializeField] Slider m_VolumeSlider;

    [SerializeField] Slider m_CurrentTimeSlider;
    [SerializeField] Slider m_InteractiveSlider;

    [SerializeField] Text m_CurrentTimeText;
    [SerializeField] Text m_TotalTimeText;

    // PLAY
    [SerializeField] Image m_PlayImage;
    [SerializeField] Image m_PauseImage;

    // SHUFFLE
    [SerializeField] Image m_ShuffleImage;
    [SerializeField] Color32 m_ShuffleButtonColor_Off = Color.white;
    [SerializeField] Color32 m_ShuffleButtonColor_On = new Color32(246, 87, 86, 255);
    private bool m_Shuffle = false;

    // REPLAY
    [SerializeField] Image m_ReplayImage;
    [SerializeField] Text m_ReplayText;
    [SerializeField] Color32 m_ReplayButtonColor_Off = Color.white;
    [SerializeField] Color32 m_ReplayButtonCOlor_On = new Color32(246, 87, 86, 255);
    private ReplayState m_CurrentReplayState = ReplayState.NoRepeat;

    // If song has reached end, should the music app continue to play?
    [SerializeField] bool m_AutoPlay = true;

    [SerializeField] Text m_ArtistText;
    [SerializeField] Text m_SongText;

    private bool m_IsPlaying = false;

    private Artist m_CurrentArtist;
    private Album m_CurrentAlbum;
    private int m_CurrentSongIndex = 0;

    private bool m_IsDragging = false;


    [SerializeField] StandaloneInputModule m_StandaloneInputModule;


    [SerializeField] GameObject shopPanel;
    [SerializeField] TextMeshProUGUI shopPoints;
    [SerializeField] TextMeshProUGUI buy_SongTxt;
    [SerializeField] GameObject buy_Pop_Up;
    bool shopPanelStatus = false;
    public int shopPointsInt;
    

    // have to implement our custom timer as audiosource is playing is not reliable
    private float m_AudioClipLength = 0;

    private void Start()
    {
        if (CoinManager.Instance == null)
        {
            return;           
        }
        //CoinManager.instance.SetCoins(50);
        LoadAlbum(0, 0);
        SongData();
        LoadSong(0);
        
    }

    /// <summary>
    ///  Custom oncomplete callback, because audiosource doesn't have one
    /// </summary>
    private void AudioSource_CheckComplete()
    {
        if (m_IsPlaying && m_AudioSource.time >= m_AudioClipLength)
        {
            //m_StandaloneInputModule.DeactivateModule();
            //m_StandaloneInputModule.ActivateModule();

            m_IsPlaying = false;

            if (m_AutoPlay)
            {
                Reset();

                if(m_CurrentReplayState == ReplayState.RepeatSingle)
                {
                    Play();
                }
                else if (IsLastSong() && m_CurrentReplayState == ReplayState.RepeatAll && !m_Shuffle)
                {
                    IncrementIndex();
                    LoadSong(m_CurrentSongIndex);
                    Play();
                }
                else if (IsLastSong() && m_CurrentReplayState != ReplayState.RepeatAll && !m_Shuffle)
                {
                    Pause();
                }
                else if (!IsLastSong() && !m_Shuffle)
                {
                    IncrementIndex();
                    LoadSong(m_CurrentSongIndex);
                    Play();
                }
                else if (m_Shuffle)
                {
                    m_CurrentSongIndex = GetShuffledIndex();
                    LoadSong(m_CurrentSongIndex);
                    Play();
                }
            }
            else
            {
                Pause();
                Reset();
            }
        }
    }

    private void OnEnable()
    {
        if (CoinManager.Instance)
            CoinManager.Instance.coinValueChanged += OnCoinValueChange;
    }
    private void OnDisable()
    {
        if (CoinManager.Instance)
            CoinManager.Instance.coinValueChanged -= OnCoinValueChange;
    }

    public void OnCoinValueChange(int value)
    {
        shopPoints.text = "Total Points : "+ value.ToString();
    }
    private void Update()
    {

        //Debug.Log(shopPoints.text); 
       // shopPoints.text = "Total Points : " + CoinManager.Instance.GetCoins().ToString();
        //Debug.Log(shopPoints.text); 
        if (m_IsPlaying)
        {
            AudioSource_CheckComplete();

            if(!m_IsDragging)
            {
                MovePlayHead();
            }          
            SetCurrentTimeUI();
        }
    }

    private void MovePlayHead()
    {
        m_CurrentTimeSlider.value = m_AudioSource.time;
    }

    private void LoadAlbum(int artistId, int albumId)
    {
        m_CurrentArtist = m_MusicLibrary.Artists[artistId];
        m_CurrentAlbum = m_CurrentArtist.Albums[albumId];

        if(m_CurrentAlbum.AlbumSprite != null)
        {
            SetAlbumImage(m_CurrentAlbum.AlbumSprite);
        }
        else
        {
            Debug.Log("Could not find album image");
        }
    }

    private void LoadSong( int songIndex)
    {
        Reset();

        if (m_CurrentAlbum == null)
        {
            Debug.LogError("Current album == null");
            return;
        }

        if(songIndex >= m_CurrentAlbum.songClips_Bools.Count)
        {
            Debug.LogError("Could not find song/index");
            return;
        }

        m_CurrentSongIndex = songIndex;

        m_AudioSource.clip = m_CurrentAlbum.songClips_Bools[songIndex].song;

        m_CurrentTimeSlider.maxValue = m_CurrentAlbum.songClips_Bools[songIndex].song.length;
        m_InteractiveSlider.maxValue = m_CurrentAlbum.songClips_Bools[songIndex].song.length;

        m_TotalTimeText.text = m_CurrentAlbum.songClips_Bools[songIndex].song.length.ToString();

        UpdateArtistAndSongLabels(m_CurrentArtist.ArtistName, m_CurrentAlbum.songClips_Bools[songIndex].song.name);

        m_AudioClipLength = m_CurrentAlbum.songClips_Bools[songIndex].song.length;
        
        SetTotalTimeText();
    }

    public void OnVolumeChanged()
    {
        m_AudioSource.volume = m_VolumeSlider.value;
    }

    public void OnScrubBarHeadMove()
    {
        if(!m_IsPlaying)
        {
            if ((int)m_InteractiveSlider.value == 0 || (int)m_InteractiveSlider.value == m_AudioClipLength)
                return;

            if ((int)m_InteractiveSlider.value != m_AudioClipLength)
            {
                Play();
            }
            else
            {
                // no point if were already using the slider at the end and not playing
                return;
            }
        }


        m_CurrentTimeSlider.value = (int)m_InteractiveSlider.value;

        m_IsDragging = true;

    }

    public void OnPointerUp()
    {
        m_AudioSource.time = (int)m_InteractiveSlider.value;
        m_IsDragging = false;
    }


    public void OnClick_PreviousSong()
    {
        if (m_Shuffle)
        {
            m_CurrentSongIndex = GetShuffledIndex();
        }
        else if (m_CurrentReplayState != ReplayState.RepeatSingle)
        {
            DecrementIndex();
        }

        LoadSong(m_CurrentSongIndex);
        Play();
    }

    public void OnClick_NextSong()
    {
        if (m_Shuffle)
        {
            m_CurrentSongIndex = GetShuffledIndex();
        }
        else if(m_CurrentReplayState != ReplayState.RepeatSingle)
        {
            IncrementIndex();
        }

        LoadSong(m_CurrentSongIndex);
        Play();
    }

    public void OnClick_TogglePlay()
    {
        
        m_IsPlaying = !m_IsPlaying;

        if(m_IsPlaying)
        {
            Play();
        }

        if(!m_IsPlaying)
        {
            Pause();
        }
    }

    public void OnClick_Shuffle()
    {
        m_Shuffle = !m_Shuffle;

        m_ShuffleImage.color = (m_Shuffle) ? m_ShuffleButtonColor_On : m_ShuffleButtonColor_Off;

        if(m_Shuffle && m_CurrentReplayState == ReplayState.RepeatSingle)
        {
            OnClick_ToggleRepeat();
        }
    }

    public void OnClick_Shop()
    {
        if(buy_Pop_Up.activeInHierarchy == true)
        {
            buy_Pop_Up.SetActive(false);
            return;
        }

        if(shopPanelStatus == true)
        {
            shopPanel.SetActive(false);
            shopPanelStatus = false;
        }
        else
        {
            shopPanel.SetActive(true);
            shopPanelStatus = true;
        }
    }

    public void OnClick_AddSong(string songName)
    {
        if (CoinManager.Instance.GetCoins() < 10)
            return;

        for(int i = 0; i< m_CurrentAlbum.songClips_Bools.Count;i++)
        { 
            if (songName == m_CurrentAlbum.songClips_Bools[i].song.name)
            {
                if (m_CurrentAlbum.songClips_Bools[i].unlocked == true)
                {
                    return;
                }
                m_CurrentAlbum.songClips_Bools[i].unlocked = true;
                buy_SongTxt.text = "Congratulations you have successfully bought sound track named :  " + "'"+m_CurrentAlbum.songClips_Bools[i].song.name +"'";
                PlayerPrefs.SetString(songName, "true");
                CoinManager.Instance.SetCoins(CoinManager.Instance.GetCoins() - 10);
            }
        }
        buy_Pop_Up.SetActive(true);    
    }
    private void TurnOffShuffle()
    {
        m_Shuffle = false;

        m_ShuffleImage.color =  m_ShuffleButtonColor_Off;
    }

    public void OnClick_ToggleRepeat()
    {
        // 0 = no repeat
        // 1 = repeat all
        // 2 = repeat single

        int clickCount = (int)m_CurrentReplayState;
        clickCount++;

        if(clickCount > 2)
            clickCount = 0;


        m_CurrentReplayState = (ReplayState)clickCount;

        m_ReplayText.enabled = false;

        switch (m_CurrentReplayState)
        {
            case ReplayState.NoRepeat:
                m_ReplayImage.color = m_ReplayButtonColor_Off;
                break;
            case ReplayState.RepeatAll:
                m_ReplayImage.color = m_ReplayButtonCOlor_On;
                break;
            case ReplayState.RepeatSingle:
                m_ReplayImage.color = m_ReplayButtonCOlor_On;
                m_ReplayText.color = m_ReplayButtonCOlor_On;
                m_ReplayText.enabled = true;
                TurnOffShuffle();
                break;
        }
    }

    private void Pause()
    {
        m_PlayImage.gameObject.SetActive(true);
        m_PauseImage.gameObject.SetActive(false);
        m_AudioSource.Pause();
        m_IsPlaying = false;
    }

    private void Play()
    {
        m_PlayImage.gameObject.SetActive(false);
        m_PauseImage.gameObject.SetActive(true);
        m_AudioSource.Play();
        m_IsPlaying = true;
    }

    // sets audio clip to 0
    private void Reset()
    {
        m_AudioSource.time = 0;
        m_CurrentTimeSlider.value = 0;
        m_InteractiveSlider.value = 0;
    }

    private int GetShuffledIndex()
    {
        while(true)
        {
            int i = Random.Range(0, m_CurrentAlbum.songClips_Bools.Count);

            // prevent the index from being the same
            if(i != m_CurrentSongIndex)
            {
                if(m_CurrentAlbum.songClips_Bools[i].unlocked != false)
                {
                         return i;
                }
            }
        }
    }

    private void IncrementIndex()
    {
        m_CurrentSongIndex++;
        int tempIndex = m_CurrentSongIndex;
        if (m_CurrentAlbum.songClips_Bools[m_CurrentSongIndex].unlocked == false)
        {
            for (int i = m_CurrentSongIndex; i < m_CurrentAlbum.songClips_Bools.Count; i++)
            {
                if (m_CurrentAlbum.songClips_Bools[i].unlocked == true)
                {
                    m_CurrentSongIndex = i;
                    Debug.Log(" if  +" + m_CurrentSongIndex);
                    return;
                }
            }
            if(tempIndex == m_CurrentSongIndex)
            {
                m_CurrentSongIndex = 0;
            }
        }
        if (m_CurrentSongIndex >= m_CurrentAlbum.songClips_Bools.Count)
        {
            m_CurrentSongIndex = m_CurrentSongIndex % m_CurrentAlbum.songClips_Bools.Count;
            return;
        }
    }

    private bool IsLastSong()
    {
        if(m_CurrentSongIndex + 1 >= m_CurrentAlbum.songClips_Bools.Count)
        {
            return true;
        }
        return false;
    }

    private void DecrementIndex()
    {
        m_CurrentSongIndex--;
        if (m_CurrentSongIndex < 0)
        {
            m_CurrentSongIndex = m_CurrentAlbum.songClips_Bools.Count - 1;
        }
        int tempIndex = m_CurrentSongIndex;
        if (m_CurrentAlbum.songClips_Bools[m_CurrentSongIndex].unlocked == false)
        {
            for (int i = m_CurrentSongIndex; i >= 0 ; i--)
            {
                if (m_CurrentAlbum.songClips_Bools[i].unlocked == true)
                {
                    m_CurrentSongIndex = i;
                    return;
                }
            }
            if (tempIndex == m_CurrentSongIndex)
            {
                m_CurrentSongIndex = 0;
            }
        }
    }

    private void SetCurrentTimeUI()
    {
        string minutes = Mathf.Floor((int)m_AudioSource.time / 60).ToString("00");
        string seconds = ((int)m_AudioSource.time % 60).ToString("00");

        m_CurrentTimeText.text = minutes + ":" + seconds;
    }

    private void SetTotalTimeText()
    {
        string minutes = Mathf.Floor((int)m_AudioSource.clip.length / 60).ToString("00");
        string seconds = ((int)m_AudioSource.clip.length % 60).ToString("00");

        m_TotalTimeText.text = minutes + ":" + seconds;
    }

    private void UpdateArtistAndSongLabels(string artist, string song)
    {
        m_ArtistText.text = artist;
        m_SongText.text = song;
    }

    private void SetAlbumImage(Sprite sprite)
    {
        if(m_AlbumImage != null)
        {
            m_AlbumImage.sprite = sprite;
        }
        else
        {
            Debug.Log("Warning album image wont be set as m_AlbumImage == null");
        }
    }

    private void SongData()
    {
        for(int i = 0; i< m_CurrentAlbum.songClips_Bools.Count; i++)
        {
            string boolean = PlayerPrefs.GetString(m_CurrentAlbum.songClips_Bools[i].song.name, "false");
           

            if(boolean == "true")
            {
                m_CurrentAlbum.songClips_Bools[i].unlocked = true;
            }
            else if (boolean == "false")
            {
                m_CurrentAlbum.songClips_Bools[i].unlocked = false;
            }
            else
            {
                Debug.Log("  none");
            }

            if (i == 0)
            {
                m_CurrentAlbum.songClips_Bools[i].unlocked = true;
            }
        }
    }

}
