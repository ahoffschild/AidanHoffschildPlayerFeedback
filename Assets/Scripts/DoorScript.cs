using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float delay, chance;
    public bool isSparks;

    private Animator animator;
    private ParticleSystem sparks;
    private AudioSource electric, doorClang;

    private float lastCheckTimestamp;

    void Start()
    {
        if (isSparks)
        {
            animator = GetComponent<Animator>();
            sparks = transform.Find("Sparks").GetComponent<ParticleSystem>();
            electric = transform.Find("Sparks").GetComponent<AudioSource>();
        }
        else
        {
            doorClang = GetComponent<AudioSource>();
        }
        lastCheckTimestamp = Time.time;
    }

    public void Flash()
    {
        animator.SetTrigger("FlashFX");
    }

    public void SpawnSparks()
    {
        sparks.Play();
        electric.Play();
    }

    public void DoorBang()
    {
        doorClang.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSparks)
        {
            if (Time.time > lastCheckTimestamp + delay)
            {
                if (Mathf.Round(Random.Range(1, chance)) == 1)
                {
                    Flash();
                }
                lastCheckTimestamp = Time.time;
            }
        }
    }
}
