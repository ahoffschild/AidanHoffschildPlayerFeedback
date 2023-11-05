using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public float pipeDelay, spawnDelay, endDelay, lastDelay;
    public bool cameraCutscene, startBurst, pipeBurst, smokeSeen, preparation, monsterSpawn, playerCaught, gameEnding, finalPause;

    public GameObject[] lights;
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
        var childCount = GameObject.Find("LightParent").transform.childCount;
        lights = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            lights[i] = GameObject.Find("LightParent").transform.GetChild(i).gameObject;
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
            foreach (GameObject light in lights)
            {
                light.SetActive(false);
            }
            fleshCrunch.Play();
            progTimestamp = Time.time;
            gameEnding = true;
        }

        if (gameEnding && !finalPause && Time.time < progTimestamp + endDelay)
        {
            float alpha = Mathf.Lerp(0, 1, (Time.time - progTimestamp) / endDelay);
            PlayerUIManager.instance.BlankScreen(alpha);
        }

        if (gameEnding && !finalPause && Time.time > progTimestamp + endDelay)
        {
            PlayerUIManager.instance.BlankScreen(1);
            progTimestamp = Time.time;
            finalPause = true;
        }

        if (finalPause && Time.time > progTimestamp + lastDelay)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }
}
