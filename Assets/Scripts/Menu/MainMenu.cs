using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Dropdown screenMode;
    [SerializeField] private Slider backgroundMusic;
    [SerializeField] private Slider sfx;


    public delegate void MainMenuWithFloatArg(float number);

    public delegate void MainMenuWithoutArgs();

    public static event MainMenuWithFloatArg BackgroundMusicChanged;
    public static event MainMenuWithFloatArg SfxChanged;
    public static event MainMenuWithoutArgs PlaySfx;
    
    public static event MainMenuWithoutArgs NewGameStarted;

    private void Start()
    {
        var backgroundVolume = PlayerPrefs.GetFloat("MusicBackgroundVolume");
        var sfxVolume = PlayerPrefs.GetFloat("SfxVolume");
        var screen = PlayerPrefs.GetInt("FullScreenMode");
        screenMode.value = PlayerPrefs.HasKey("FullScreenMode") ? screen : 0;
        Screen.fullScreenMode = PlayerPrefs.HasKey("FullScreenMode") ?  Screen.fullScreenMode = screen switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            _ => Screen.fullScreenMode
        } : FullScreenMode.ExclusiveFullScreen;
        backgroundMusic.value = PlayerPrefs.HasKey("MusicBackgroundVolume") ? backgroundVolume : 0.5f;
        BackgroundMusicChanged?.Invoke(PlayerPrefs.HasKey("MusicBackgroundVolume") ? backgroundVolume : 0.5f);
        sfx.value = PlayerPrefs.HasKey("SfxVolume") ? sfxVolume : 0.5f;
        SfxChanged?.Invoke(PlayerPrefs.HasKey("SfxVolume") ? sfxVolume : 0.5f);
        settingsMenu.SetActive(false);
    }

    public void NewGame()
    {
        PlaySfx?.Invoke();
        NewGameStarted?.Invoke();
        SceneManager.LoadScene("Tutorial level");
    }

    public void GoToSettings()
    {
        PlaySfx?.Invoke();
        menu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    
    public void Exit()
    {
        PlaySfx?.Invoke();
        Application.Quit();
    }

    public void ChangeScreenMode(Dropdown chosenOption)
    {
        PlaySfx?.Invoke();
        PlayerPrefs.SetInt("FullScreenMode", chosenOption.value);
        Screen.fullScreenMode = chosenOption.value switch
        {
            0 => FullScreenMode.ExclusiveFullScreen,
            1 => FullScreenMode.FullScreenWindow,
            2 => FullScreenMode.Windowed,
            _ => Screen.fullScreenMode
        };
        
    }

    public void MusicBackgroundChange(Slider slider)
    {
        PlaySfx?.Invoke();
        BackgroundMusicChanged?.Invoke(slider.value);
        PlayerPrefs.SetFloat("MusicBackgroundVolume", slider.value);
    }
    
    public void SfxChange(Slider slider)
    {
        PlaySfx?.Invoke();
        SfxChanged?.Invoke(slider.value);
        PlayerPrefs.SetFloat("SfxVolume", slider.value);
    }
    
    public void ReturnToMainMenu()
    {
        PlaySfx?.Invoke();
        settingsMenu.SetActive(false);
        menu.SetActive(true);
    }
}
