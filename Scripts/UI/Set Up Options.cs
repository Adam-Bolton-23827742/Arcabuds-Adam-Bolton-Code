using UnityEngine;
using UnityEngine.UI;

public class SetUpOptions : MonoBehaviour
{
    [SerializeField] private Slider Master, Music, SFX, RumbleSlider;
    [SerializeField] private Toggle RumbleToggle, TutorialToggle;
    [HideInInspector] public static float RumbleIntensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume");
            Master.value = AudioListener.volume;
            Master.GetComponent<Options>().SetText(100.0f);
        }

        else
        {
            AudioListener.volume = Master.value;
        }

        Options MusicOptions = Music.GetComponent<Options>();

        if (MusicOptions.MusicSource != null)
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                MusicOptions.MusicSource.volume = PlayerPrefs.GetFloat("MusicVolume");

                if (MusicOptions.MusicSource2 != null)
                {
                    MusicOptions.MusicSource2.volume = PlayerPrefs.GetFloat("MusicVolume");
                }
                    
                Music.value = MusicOptions.MusicSource.volume;
                MusicOptions.SetText(100.0f);
            }

            else
            {
                MusicOptions.MusicSource.volume = Music.value;

                if (MusicOptions.MusicSource2 != null)
                {
                    MusicOptions.MusicSource2.volume = Music.value;
                }
            }
        }

        Options SFXOptions = SFX.GetComponent<Options>();
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            SFXOptions.Slider.value = PlayerPrefs.GetFloat("SFXVolume");
            SFXOptions.SetAllAudioSources(SFXOptions.Slider.value);
        }

        else
        {
            SFXOptions.SetAllAudioSources(SFXOptions.Slider.value);
        }

        Options RumbleOptions = RumbleSlider.GetComponent<Options>();
        if (PlayerPrefs.HasKey("RumbleIntensity"))
        {
            RumbleIntensity = PlayerPrefs.GetFloat("RumbleIntensity");
            RumbleSlider.value = RumbleIntensity;
            RumbleOptions.SetText(1000.0f);
        }

        else
        {
            RumbleIntensity = 0.1f;
        }

        if (PlayerPrefs.HasKey("Rumble"))
        {
            if (PlayerPrefs.GetInt("Rumble") == 0)
            {
                RumbleToggle.isOn = false;
            }

            else
            {
                RumbleToggle.isOn = true;
            }
        }

        else
        {
            RumbleToggle.isOn = true;
        }

        if (PlayerPrefs.HasKey("Tutorial"))
        {
            if (PlayerPrefs.GetInt("Tutorial") == 0)
            {
                TutorialToggle.isOn = false;
            }

            else
            {
                TutorialToggle.isOn = true;
            }
        }

        else
        {
            TutorialToggle.isOn = true;
        }
    }
}
