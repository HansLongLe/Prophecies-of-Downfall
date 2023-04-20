using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUIChange : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClick;
    private AudioSource audioSource;
    
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = buttonClick;
        var volume = PlayerPrefs.GetFloat("SfxVolume");
        audioSource.volume = volume;
        MainMenu.SfxChanged += VolumeChanged;
        PausedMenu.SfxChanged += VolumeChanged;
        MainMenu.PlaySfx += PlaySound;
        PausedMenu.PlaySfx += PlaySound;
        DialogueUI.Interact += PlaySound;
    }

    private void VolumeChanged(float number)
    {
        audioSource.volume = number;
    }

    private void PlaySound()
    {
        audioSource.Play();
    }
    
}
