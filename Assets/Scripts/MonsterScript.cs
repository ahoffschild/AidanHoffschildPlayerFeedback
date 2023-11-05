using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public Transform[] targetPoints;
    public float monsterSpeed, pointThreshold;

    private Rigidbody rb;
    private AudioSource monsterRunSound;
    private AudioSource monsterRoar;
    private Transform destination, playerCamera;
    private int pointProgress;
    private bool monsterActive, roar, gameOver;

    // Start is called before the first frame update
    void Start()
    {
        Collider[] collidersToClear;
        Collider thisCollider = transform.Find("Collider").GetComponent<Collider>();
        collidersToClear = GameObject.Find("MapGeometry").transform.GetComponentsInChildren<Collider>();
        foreach (Collider collider in collidersToClear)
        {
            Physics.IgnoreCollision(thisCollider, collider);
        }

        rb = GetComponent<Rigidbody>();
        monsterActive = false;
        roar = false;
        gameOver = false;
        pointProgress = 0;
        monsterRunSound = GetComponent<AudioSource>();
        monsterRoar = transform.Find("Roar").GetComponent<AudioSource>();
        playerCamera = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!monsterActive && !gameOver && GameManager.instance.monsterSpawn)
        {
            monsterActive = true;
            gameObject.transform.position = targetPoints[pointProgress].position;
            monsterRunSound.Play();
            pointProgress++;
        }

        if (monsterActive && pointProgress < targetPoints.Length)
        {
            destination = targetPoints[pointProgress];
        }

        if (monsterActive && pointProgress >= targetPoints.Length)
        {
            destination = playerCamera;
        }

        if (monsterActive && pointProgress < targetPoints.Length && (gameObject.transform.position - targetPoints[pointProgress].position).magnitude < pointThreshold)
        {
            pointProgress++;
        }

        if (!roar && monsterActive)
        {
            RoarCheck();
        }

        if (monsterActive)
        {
            transform.LookAt(destination.position);
            transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        }
    }

    private void RoarCheck()
    {
        Vector3 pDistance = playerCamera.transform.position - transform.position;
        Debug.DrawRay(transform.position + Vector3.up, pDistance);
        if (!Physics.Raycast(transform.position + Vector3.up, pDistance, pDistance.magnitude))
        {
            monsterRoar.Play();
            roar = true;
        }
    }

    private void FixedUpdate()
    {
        if (monsterActive)
        {
            rb.velocity = (transform.forward * monsterSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && monsterActive)
        {
            GameManager.instance.playerCaught = true;
            monsterRoar.Stop();
            monsterRunSound.Stop();
            gameObject.SetActive(false);
        }
    }
}
