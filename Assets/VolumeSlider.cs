using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioSource audioSource;

    public Slider slider;

    public void OnSliderValueChanged() {
        audioSource.volume = slider.value;

    }
}
