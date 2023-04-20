using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySfx : MonoBehaviour
{
    
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip gettingHitSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip dyingSound;

    private AudioSource audioSource;
    private bool wasMoving;
    
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        var sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        var volume = PlayerPrefs.HasKey("SfxVolume") ? sfxVolume : 0.5f;

        audioSource.volume = volume;

        MainMenu.SfxChanged += VolumeChange;
        PausedMenu.SfxChanged += VolumeChange;
        PausedMenu.MenuOpened += PauseWalkingSound;
        PausedMenu.MenuClosed += ResumeWalkingSound;
        EnemyMovement.Dying += PlayDyingSound;
        EnemyMovement.StartMoving += PlayWalkingSound;
        EnemyMovement.StopMoving += StopWalkingSound;
        EnemyMovement.StartAttack += PlayAttackSound;
        EnemyMovement.GettingHit += PlayGettingHitSound;
    }

    private void VolumeChange(float changedVolume)
    {
        audioSource.volume = changedVolume;
    }

    private void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }
    private void PlayGettingHitSound()
    {
        audioSource.PlayOneShot(gettingHitSound);
    }
    private void PlayWalkingSound()
    {
        wasMoving = true;
        audioSource.PlayOneShot(walkSound);
    }

    private void StopWalkingSound()
    {
        wasMoving = false;
        audioSource.Stop();
    }
    
    private void PauseWalkingSound()
    {
        audioSource.Stop();
    }

    private void ResumeWalkingSound()
    {
        if (wasMoving)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }
    private void PlayDyingSound()
    {
        audioSource.PlayOneShot(dyingSound);
    }


}
