using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Slider Slider;
    public TextMeshProUGUI Value;
    public AudioSource MusicSource, MusicSource2;
    public Toggle RumbleToggle, TutorialToggle;

    private void Start()
    {
        if (Slider != null)
        {
            SetText(100.0f);
        }
    }

    public void SetText(float Multiplication)
    {
        int SliderValue = Mathf.RoundToInt(Multiplication * Slider.value);
        Value.text = SliderValue.ToString() + "%";
    }

    public void MasterVolume()
    {
        AudioListener.volume = Slider.value;
        PlayerPrefs.SetFloat("MasterVolume", AudioListener.volume);
        SetText(100.0f);
    }

    public void MusicVolume()
    {
        if (MusicSource != null)
        {
            MusicSource.volume = Slider.value;
        }

        if (MusicSource2 != null)
        {
            MusicSource2.volume = Slider.value;
        }

        PlayerPrefs.SetFloat("MusicVolume", Slider.value);
        SetText(100.0f);
    }

    public void SFXVolume()
    {
        SetAllAudioSources(Slider.value);
        PlayerPrefs.SetFloat("SFXVolume", Slider.value);
    }

    public void SetAllAudioSources(float Value)
    {
        AudioSource[] All = Resources.FindObjectsOfTypeAll<AudioSource>();

        foreach (AudioSource Object in All)
        {
            if (Object.tag != "Music")
            {
                Object.volume = Value;
            }
        }

        All = Array.Empty<AudioSource>();
        SetText(100.0f);
    }

    public void RumbleIntensity()
    {
        SetUpOptions.RumbleIntensity = Slider.value;
        PlayerPrefs.SetFloat("RumbleIntensity", Slider.value);
        SetText(1000.0f);
    }

    public void SetRumbleToggle()
    {
        if (RumbleToggle.isOn)
        {
            PlayerPrefs.SetInt("Rumble", 1);
        }

        else
        {
            PlayerPrefs.SetInt("Rumble", 0);
        }
    }

    public void SetTutorialToggle()
    {
        if (TutorialToggle.isOn)
        {
            PlayerPrefs.SetInt("Tutorial", 1);
        }

        else
        {
            PlayerPrefs.SetInt("Tutorial", 0);
        }
    }
}
