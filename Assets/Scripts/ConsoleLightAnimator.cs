using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleLightAnimator : MonoBehaviour
{
    public float chance, delay;

    private Animator animator;
    private AudioSource lightHum, lightClick;
    private float timeStamp;

    private void Start()
    {
        animator = GetComponent<Animator>();
        lightHum = GetComponent<AudioSource>();
        lightClick = transform.Find("Click").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Time.time > timeStamp + delay)
        {
            if (Mathf.Round(Random.Range(1, chance)) == 1)
            {
                animator.SetTrigger("Flicker");
            }
            timeStamp = Time.time;
        }
    }

    void FlickerOut()
    {
        lightClick.Play();
        lightHum.Stop();
    }

    void TurnOn()
    {
        lightHum.Play();
    }
}
