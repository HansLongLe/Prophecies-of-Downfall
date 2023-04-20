using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartWaveUI : MonoBehaviour
{
    [SerializeField] private GameObject startWave;
    private GameObject buttonText;
    private GameObject button;
    private GameObject dialogueText;

    private bool waveStarted = false;

    public delegate void StartWaveUIWithoutArgs();
    public static event StartWaveUIWithoutArgs StartWaveEvent;
    public static event StartWaveUIWithoutArgs ChangePlaylist;
    
    // Start is called before the first frame update
    private void Start()
    {
        StartWave.WaveStarted += WaveStarted;
        StartWave.WaveEnded += WaveEnded;
        buttonText = startWave.transform.Find("buttonText").GameObject();
        button = startWave.transform.Find("button").GameObject();
        dialogueText = startWave.transform.Find("dialogueText").GameObject();
        buttonText.SetActive(false);
        button.SetActive(false);
        dialogueText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<HeroKnight>(out var _) || waveStarted) return;
        buttonText.SetActive(true);
        button.SetActive(true);
        dialogueText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<HeroKnight>(out var _) || waveStarted) return;
        buttonText.SetActive(false);
        button.SetActive(false);
        dialogueText.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (!buttonText.activeSelf || !button.activeSelf || waveStarted || !value.performed || PausedMenu.menuOpened) return;
        StartWaveEvent?.Invoke();
    }

    private void WaveStarted()
    {
        if (buttonText == null || button == null || dialogueText == null) return;
        waveStarted = true;
        buttonText.SetActive(false);
        button.SetActive(false);
        dialogueText.SetActive(false);
        ChangePlaylist?.Invoke();
    }

    private void WaveEnded()
    {
        waveStarted = false;
    }
}
