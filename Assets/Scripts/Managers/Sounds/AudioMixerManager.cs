using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour
{
    public AudioMixer _audioMixer;
    public Slider _musicSlider;
    public Slider _soundEffectsSlider;

    private void Start()
    {
        _musicSlider.value = PlayerPrefs.GetFloat("MusicValue", 1f);
        _soundEffectsSlider.value = PlayerPrefs.GetFloat("SoundEffectsValue", 1f);
    }
    public void updateMusicVolume(float sliderValue)
    {
        _audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicValue", sliderValue);
    }
    public void updateSoundEffectsVolume(float sliderValue)
    {
        _audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("SoundEffectsValue", sliderValue);
    }
}
