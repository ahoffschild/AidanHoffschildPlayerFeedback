using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class AmbientSound : MonoBehaviour
{
    public float chance, delay;
    public AudioClip[] clips;

    private AudioSource audioSource;
    private float lastCheckTimestamp;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastCheckTimestamp = Time.time;
    }

    void Update()
    {
        if (Time.time > lastCheckTimestamp + delay && !GameManager.instance.playerCaught)
        {
            if (Mathf.Round(Random.Range(1, chance)) == 1)
            {
                PlaySound();
            }
            lastCheckTimestamp = Time.time;
        }

        if (audioSource.isPlaying && GameManager.instance.playerCaught)
        {
            audioSource.Stop();
        }
    }

    private void PlaySound()
    {
        audioSource.clip = clips[(int)Mathf.Round(Random.Range(1, 2))];
        audioSource.Play();
    }
}
