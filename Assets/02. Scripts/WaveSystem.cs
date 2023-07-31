using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    private float waveTime;
    private float breakTime;
    [SerializeField]
    private bool isBreak;

    void Start()
    {
        waveTime = 120.0f;
        breakTime = 60.0f;
    }

    void Update()
    {
        // 타이머
        if (!isBreak)
        {
            waveTime -= Time.deltaTime;
            if (waveTime <= 0)
            {
                isBreak = !isBreak;
                breakTime = 60.0f;
            }
        }
        else
        {
            breakTime -= Time.deltaTime;
            if (breakTime <= 0)
            {
                isBreak = !isBreak;
                waveTime = 120.0f;
            }
        }
    }

}
