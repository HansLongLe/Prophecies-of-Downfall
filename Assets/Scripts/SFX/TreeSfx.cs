using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TreeSfx : MonoBehaviour
{
    [SerializeField] private AudioClip gettingDamageSound;

    private AudioSource audioSource;

    private float volume;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gettingDamageSound;
        var sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        volume = PlayerPrefs.HasKey("SfxVolume") ? sfxVolume : 0.5f;
        audioSource.volume = volume;

        MainMenu.SfxChanged += ChangeVolume;
        PausedMenu.SfxChanged += ChangeVolume;
        SacredTree.TakenDamage += PlayGettingHitSound;
    }

    private void ChangeVolume(float changedVolume)
    {
        audioSource.volume = volume;
    }

    private void PlayGettingHitSound(int _)
    {
        audioSource.PlayOneShot(gettingDamageSound);
    }
}
