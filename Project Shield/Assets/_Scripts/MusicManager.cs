using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance { get; private set; }

    [SerializeField]
    private AudioClip menuMusic;
    [SerializeField]
    private AudioClip gameMusic;

    [SerializeField]
    private float maxVol = 0;
    [SerializeField]
    private float minVol = -65;

    private AudioSource audioSource;

    [SerializeField]
    private AudioMixer masterMixer;

    private string masterVolString = "masterVol";
    private string musicVolString = "musicVol";
    private string sfxVolString = "sfxVol";

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    void Start () {
        GameManager.Instance.GameStateChanged += GameManager_GameStateChanged;
        LoadSoundVol();
    }

    private void OnApplicationQuit()
    {
        SaveSoundVol();
    }

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void GameManager_GameStateChanged(object sender, GameManager.GameStateChangedArgs e)
    {
        if(e.newState == GameManager.GameState.PLAYING)
        {
            audioSource.clip = gameMusic;
        } else
        {
            audioSource.clip = menuMusic;
        }

        audioSource.Play();
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Sets the master volume to a specified level.
    /// </summary>
    /// <param name="masterVol">The level to set the volume to.</param>
    public void SetMasterVol(float masterVol)
    {
        masterMixer.SetFloat(masterVolString, Mathf.Lerp(minVol, maxVol, masterVol));
    }


    /// <summary>
    /// Sets the music volume to a specified level.
    /// </summary>
    /// <param name="musicVol">The level to set the volume to.</param>
    public void SetMusicVol(float musicVol)
    {
        masterMixer.SetFloat(musicVolString, Mathf.Lerp(minVol, maxVol, musicVol));
    }

    /// <summary>
    /// Sets the sfx volume to a specified level.
    /// </summary>
    /// <param name="sfxVol">The level to set the volume to.</param>
    public void SetSFXVol(float sfxVol)
    {
        masterMixer.SetFloat(sfxVolString, Mathf.Lerp(minVol, maxVol, sfxVol));
    }


    /// <summary>
    /// Save the volume of each mixer group to the PlayerPrefs.
    /// </summary>
    private void SaveSoundVol()
    {
        float master;
        float music;
        float sfx;

        masterMixer.GetFloat(masterVolString, out master);
        masterMixer.GetFloat(musicVolString, out music);
        masterMixer.GetFloat(sfxVolString, out sfx);

        PlayerPrefs.SetFloat(masterVolString, master);
        PlayerPrefs.SetFloat(musicVolString, music);
        PlayerPrefs.SetFloat(sfxVolString, sfx);
    }

    /// <summary>
    /// Load the volume of each mixer group from the player prefs.
    /// </summary>
    private void LoadSoundVol()
    {
        
        if (PlayerPrefs.HasKey(masterVolString))
        {          
            masterMixer.SetFloat(masterVolString, PlayerPrefs.GetFloat(masterVolString));
        }

        if (PlayerPrefs.HasKey(musicVolString))
        {
            masterMixer.SetFloat(musicVolString, PlayerPrefs.GetFloat(musicVolString));
        }

        if (PlayerPrefs.HasKey(sfxVolString))
        {
            masterMixer.SetFloat(sfxVolString, PlayerPrefs.GetFloat(sfxVolString));
        }
    }

    /// <summary>
    /// Get the current volume of the masterMixer.
    /// </summary>
    /// <returns>The current volume of the master mixer.</returns>
    public float GetMasterVolFloat()
    {
        float vol;
        masterMixer.GetFloat(masterVolString, out vol);

        return vol;
    }

    /// <summary>
    /// Get the current volume of the masterMixer normalized to 1.
    /// </summary>
    /// <returns>Volume of masterMixer normalized to 1.</returns>
    public float GetMasterVolFactor()
    {
        float vol;
        masterMixer.GetFloat(masterVolString, out vol);

        float factor = (vol - minVol) / (maxVol - minVol);
        return factor;
    }

    /// <summary>
    /// Get the current volume of the musicMixer.
    /// </summary>
    /// <returns>The current volume of the musicMixer.</returns>
    public float GetMusicVolFloat()
    {
        float vol;
        masterMixer.GetFloat(musicVolString, out vol);

        return vol;
    }

    /// <summary>
    /// Get the current volume of the musicMixer normalized to 1.
    /// </summary>
    /// <returns>The current volume of the musicMixer normalized to 1.</returns>
    public float GetMusicVolFactor()
    {
        float vol;
        masterMixer.GetFloat(musicVolString, out vol);

        float factor = (vol - minVol) / (maxVol - minVol);
        return factor;
    }

    /// <summary>
    /// Get the current volume of the sfxMixer.
    /// </summary>
    /// <returns>The current volume of the sfxMixer.</returns>
    public float GetSfxVolFloat()
    {
        float vol;
        masterMixer.GetFloat(sfxVolString, out vol);

        return vol;
    }

    /// <summary>
    /// Get the current volume of the sfxMixer normalized to 1.
    /// </summary>
    /// <returns>The current volume of the sfxMixer normalized to 1.</returns>
    public float GetSfxVolFactor()
    {
        float vol;
        masterMixer.GetFloat(sfxVolString, out vol);

        float factor = (vol - minVol) / (maxVol - minVol);
        return factor;
    }



}
