using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeScript : MonoBehaviour
{
    public float stareDuration;

    private GameObject playerCamera;
    private Collider smokeTarget;
    private bool pipeActive, staring;
    private AudioSource pipeBurstSound, pipeLoop;
    private ParticleSystem pipeBurstSmoke;
    private float stareStartTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        smokeTarget = GetComponent<Collider>();
        smokeTarget.enabled = false;
        playerCamera = GameObject.FindWithTag("MainCamera");
        pipeLoop = GetComponent<AudioSource>();
        pipeBurstSound = transform.Find("PipeFX").GetComponent<AudioSource>();
        pipeBurstSmoke = transform.Find("PipeFX").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pipeActive && GameManager.instance.pipeBurst)
        {
            pipeLoop.Play();
            pipeBurstSound.Play();
            pipeBurstSmoke.Play();
            smokeTarget.enabled = true;
            pipeActive = true;
        }

        if (pipeActive)
        {
            RaycastHit hit;
            Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit);
            if (hit.collider == smokeTarget)
            {
                if (!staring)
                {
                    stareStartTimestamp = Time.time;
                    staring = true;
                }
            }
            else
            {
                staring = false;
            }
        }

        if (pipeActive && staring && Time.time > stareStartTimestamp + stareDuration)
        {
            GameManager.instance.smokeSeen = true;
        }
    }
}
