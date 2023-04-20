using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayRandomSong : MonoBehaviour
{

    [SerializeField] private List<AudioClip> menuPlayList;
    [SerializeField] private List<AudioClip> dayPlayList;
    [SerializeField] private List<AudioClip> nightPlayList;
    [SerializeField] private List<AudioClip> actionPlayList;

    private List<AudioClip> currentPlayList;

    private AudioSource audioSource;

    private bool waveInProgress = false;

    private string currentTime = "day";
         // Start is called before the first frame update
         private void Start()
         {
             audioSource = GetComponent<AudioSource>();
             var volume = PlayerPrefs.GetFloat("MusicBackgroundVolume");
             audioSource.volume = volume;
             currentPlayList = menuPlayList;
             PausedMenu.BackgroundMusicChanged += VolumeChanged;
             MainMenu.BackgroundMusicChanged += VolumeChanged;
             MainMenu.NewGameStarted += ChangeAudioClipListToNormal;
             StartWaveUI.ChangePlaylist += ChangeAudioClipListToAction;
             DayNightSystem2D.ChangePlaylistToNight += ChangeAudioClipListToNight;
             DayNightSystem2D.ChangePlaylistToNormal += ChangeAudioClipListToNormal;
             StartWave.WaveEnded += WaveEnded;
             StartRandomAudio();
         }
     
         private void StartRandomAudio()
         {
             var randomIndex = Random.Range(0, currentPlayList.Count - 1);
             StartCoroutine(AudioPlaying(randomIndex));
         }
     
         private IEnumerator AudioPlaying(int index)
         {
             audioSource.Stop();
             var audioTimer = 0f;
             audioSource.clip = currentPlayList[index];
             audioSource.Play();
             while (audioTimer <= audioSource.clip.length)
             {
                 audioTimer += Time.deltaTime;
                 yield return null;
             }
             audioSource.Stop();
             StartRandomAudio();
         }
     
         private void VolumeChanged(float number)
         {
             audioSource.volume = number;
         }

         private void ChangeAudioClipListToAction()
         {
             currentPlayList = actionPlayList;
             StartRandomAudio();
             waveInProgress = true;
         }
         
         private void ChangeAudioClipListToNight()
         {
             currentTime = "night";
             if(waveInProgress) return;
             currentPlayList = nightPlayList;
             StartRandomAudio();
         }
         private void ChangeAudioClipListToNormal()
         {
             currentTime = "day";
             if(waveInProgress) return;
             currentPlayList = dayPlayList;
             StartRandomAudio();
         }

         private void WaveEnded()
         {
             waveInProgress = false;
             if (currentTime == "day")
             {
                 ChangeAudioClipListToNormal();
             }
             else
             {
                 ChangeAudioClipListToNight();
             }
         }
    
}
