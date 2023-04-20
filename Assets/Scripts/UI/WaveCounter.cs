using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounter : MonoBehaviour
{
    public static int currentWave = 0;
    private Text waveText;
    
    // Start is called before the first frame update
    private void Start()
    {
        StartWave.WaveStarted += NextWave;
        waveText = GetComponent<Text>();
        waveText.text = currentWave.ToString();
    }

    private void NextWave()
    {
        if (waveText == null) return;
        currentWave++;
        waveText.text = currentWave.ToString();
    }
    
}
