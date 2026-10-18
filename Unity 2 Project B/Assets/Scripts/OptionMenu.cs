using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{
    //used audio mixer so I only had to put one main audio slider
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sensitivitySlider;


    // putting what the player set for the sliders
    void Start()
    {
        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 2f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    // this is the volume slider and also saves the whatever the slider gets set to
    public void ChangeVolume()
    {
        float volume = volumeSlider.value;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();

        // had to put this or else when the slider went to low it would stop working
        // and wouldnt lower audio level
        if (volume <= 0.001f)
        {
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
 
    }

    // sensitivity slider and saves
    public void ChangeSensitivity()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivitySlider.value);
        PlayerPrefs.Save();
    }
}