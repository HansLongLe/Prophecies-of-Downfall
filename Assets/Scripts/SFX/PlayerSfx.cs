using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSfx : MonoBehaviour
{

    
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip gettingHitSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip raiseShieldSound;
    [SerializeField] private AudioClip shieldAttackSound;
    [SerializeField] private AudioClip rollCooldownReadySound;
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

        MainMenu.SfxChanged += SfxChanged;
        PausedMenu.SfxChanged += SfxChanged;
        PausedMenu.MenuOpened += PauseWalkSound;
        PausedMenu.MenuClosed += ResumeWalkSound;
        HeroKnight.Attacked += PlayAttackSound;
        HeroKnight.DamageTaken += PlayGettingHitSound;
        HeroKnight.Rolled += PlayRollSound;
        HeroKnight.Moved += PlayWalkSound;
        HeroKnight.StoppedMoving += StopWalkSound;
        HeroKnight.StartedDefending += PlayRaiseShieldSound;
        HeroKnight.GotHitWhileDefending += PlayGotHitWhileDefendingSound;
        HeroParticles.RollCooldownReady += PlayRollCooldownReadySound;
        PlayerHealth.ZeroHealth += PlayDyingSound;
    }

    private void SfxChanged(float changeVolume)
    {
        audioSource.volume = changeVolume;
    }

    private void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackSound);
    }
    
    private void PlayGettingHitSound(int _)
    {
        audioSource.PlayOneShot(gettingHitSound);
    }

    private void PlayWalkSound()
    {
        wasMoving = true;
        audioSource.PlayOneShot(walkSound);
    }

    private void StopWalkSound()
    {
        wasMoving = false;
        audioSource.Stop();
    }
    
    private void PauseWalkSound()
    {
        audioSource.Stop();
    }

    private void ResumeWalkSound()
    {
        if (wasMoving)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    private void PlayRollSound()
    {
        audioSource.PlayOneShot(rollSound);
    }

    private void PlayRaiseShieldSound()
    {
        audioSource.PlayOneShot(raiseShieldSound);
    }

    private void PlayGotHitWhileDefendingSound()
    {
        audioSource.PlayOneShot(shieldAttackSound);
    }

    private void PlayRollCooldownReadySound()
    {
        audioSource.PlayOneShot(rollCooldownReadySound);
    }

    private void PlayDyingSound()
    {
        audioSource.PlayOneShot(dyingSound);
    }
    
}
