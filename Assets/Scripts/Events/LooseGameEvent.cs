using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LooseGameEvent : MonoBehaviour
{
    private Text waveText;
    // Start is called before the first frame update
    private void Start()
    {
        PlayerHealth.ZeroHealth += LooseEvent;
        SacredTreeHealth.TreeDestroyed += LooseEvent;
        waveText = transform.Find("WavesNumber").GetComponent<Text>();
        gameObject.SetActive(false);
    }

    private void LooseEvent()
    {
        waveText.text = WaveCounter.currentWave.ToString();
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        PlayerHealth.ZeroHealth -= LooseEvent;
        SacredTreeHealth.TreeDestroyed -= LooseEvent;
    }
}
