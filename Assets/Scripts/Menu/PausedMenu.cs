using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausedMenu : MonoBehaviour
{
   [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Dropdown screenMode;
    [SerializeField] private Slider backgroundMusic;
    [SerializeField] private Slider sfx;


    public delegate void PausedMenuWithFloatArg(float number);

    public delegate void PausedMenuWithoutArgs();

    public static event PausedMenuWithFloatArg BackgroundMusicChanged;
    public static event PausedMenuWithFloatArg SfxChanged;
    public static event PausedMenuWithoutArgs PlaySfx;

    public static event PausedMenuWithoutArgs MenuOpened;
    public static event PausedMenuWithoutArgs MenuClosed;
    public static event PausedMenuWithoutArgs ReturnedToTitleScreen;

    public static bool menuOpened = false;

    private void Start()
    {
        BackgroundMusicSliderChange.SliderMoved += MusicBackgroundChange;
        SfxSliderChange.SliderMoved += SfxChange;
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
        if (menuBackground != null) menuBackground.SetActive(false);
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

    private void MusicBackgroundChange(float value)
    {
        PlaySfx?.Invoke();
        BackgroundMusicChanged?.Invoke(value);
        PlayerPrefs.SetFloat("MusicBackgroundVolume", value);
    }
    
    private void SfxChange(float value)
    {
        PlaySfx?.Invoke();
        SfxChanged?.Invoke(value);
        PlayerPrefs.SetFloat("SfxVolume", value);
    }

    public void OpenCloseMenu(InputAction.CallbackContext value)
    {
        if (!value.performed) return;
        PlaySfx?.Invoke();
        if (menuOpened)
        {
            ResumeGame();  
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        MenuClosed?.Invoke();
        settingsMenu.SetActive(false);
        menu.SetActive(true);
        menuOpened = false;
        Time.timeScale = 1;
        if (menuBackground != null) menuBackground.SetActive(false);
    }

    private void PauseGame()
    {
        MenuOpened?.Invoke();
        menuOpened = true;
        Time.timeScale = 0;
        if (menuBackground != null) menuBackground.SetActive(true);
    }

    public void ReturnToTitleScreen()
    {
        PlaySfx?.Invoke();
        WaveCounter.currentWave = 0;
        ResumeGame();
        ReturnedToTitleScreen?.Invoke();
        SceneManager.LoadScene("Main Menu");
    }

    public void ReturnToMainMenu()
    {
        PlaySfx?.Invoke();
        settingsMenu.SetActive(false);
        menu.SetActive(true);
    }
}
