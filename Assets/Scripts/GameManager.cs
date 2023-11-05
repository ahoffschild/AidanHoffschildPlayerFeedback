using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public float pipeDelay, spawnDelay, endDelay;
    public bool cameraCutscene, startBurst, pipeBurst, smokeSeen, preparation, monsterSpawn, playerCaught, gameEnding;

    private AudioSource doorBreach, fleshCrunch;
    private float progTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<GameManager>();
        }
        else
        {
            Destroy(gameObject);
        }
        doorBreach = transform.Find("DoorSmash").GetComponent<AudioSource>();
        fleshCrunch = transform.Find("FleshCrunch").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraCutscene && !startBurst)
        {
            progTimestamp = Time.time;
            startBurst = true;
        }

        if (startBurst && !pipeBurst && Time.time > progTimestamp + pipeDelay)
        {
            pipeBurst = true;
        }

        if (smokeSeen && !preparation)
        {
            progTimestamp = Time.time;
            doorBreach.Play();
            preparation = true;
        }

        if (preparation && !monsterSpawn && Time.time > progTimestamp + spawnDelay)
        {
            monsterSpawn = true;
        }

        if (playerCaught && !gameEnding)
        {
            //PlayerUIManager.instance.BlankScreen();
            fleshCrunch.Play();
            progTimestamp = Time.time;
            gameEnding = true;
        }

        if (gameEnding && Time.time > progTimestamp + endDelay)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }
}
