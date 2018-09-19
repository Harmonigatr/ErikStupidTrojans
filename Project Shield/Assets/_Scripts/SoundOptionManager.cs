using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptionManager : MonoBehaviour {

    private AudioSource audioSource;
    private bool playSound = true;
    private WaitForSeconds wfs = new WaitForSeconds(0.07f);

    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        masterSlider.value = MusicManager.Instance.GetMasterVolFactor();
        musicSlider.value = MusicManager.Instance.GetMusicVolFactor();
        sfxSlider.value = MusicManager.Instance.GetSfxVolFactor();
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Plays a sample audio clip.
    /// </summary>
    public void PlayTestAudio()
    {
        if (!playSound) { return; }
        audioSource.Play();
        playSound = false;
        StartCoroutine(ResetPlaySound());
    }

    /// <summary>
    /// Coroutine that limits the sample audio to play at a specified interval.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ResetPlaySound()
    {
        yield return wfs;
        playSound = true;
    }
}
