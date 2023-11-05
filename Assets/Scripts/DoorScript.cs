using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public float delay, chance;

    private Animator animator;
    private ParticleSystem sparks;

    private float lastCheckTimestamp;

    void Start()
    {
        animator = GetComponent<Animator>();
        sparks = transform.Find("Sparks").GetComponent<ParticleSystem>();
        lastCheckTimestamp = Time.time;
    }

    public void Flash()
    {
        animator.SetTrigger("FlashFX");
    }

    public void SpawnSparks()
    {
        sparks.Play();
    }

    // Update is called once per frame
    void Update()
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
