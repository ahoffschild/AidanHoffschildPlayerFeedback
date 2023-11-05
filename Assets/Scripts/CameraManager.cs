using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    public float chance, delay, blockDuration, monsterSpeed, cutsceneDuration1, cutsceneDuration2;
    public GameObject blockImage, cutsceneMonster;
    public RawImage camImage;
    public TextMeshProUGUI camText;
    public Texture[] camFeeds;

    private int currentFeed;
    private AudioSource monsterRoar;
    private GameObject leftButton, rightButton;
    private float chanceTimestamp, blockTimestamp, cutsceneTimestamp;

    // Start is called before the first frame update
    void Start()
    {
        chanceTimestamp = Time.time;
        monsterRoar = GetComponent<AudioSource>();
        leftButton = transform.Find("ButtonL").gameObject;
        rightButton = transform.Find("ButtonR").gameObject;
    }

    public void ApplyControls(GameObject controlPressed)
    {
        if (controlPressed == leftButton)
        {
            ChangeFeed(true);
        }
        if (controlPressed == rightButton)
        {
            ChangeFeed(false);
        }
    }

    private void ChangeFeed(bool left)
    {
        if (left)
        {
            if (currentFeed - 1 >= 0)
            {
                currentFeed--;
            }
        }
        else
        {
            if (currentFeed + 1 < camFeeds.Length)
            {
                currentFeed++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (camImage.texture != camFeeds[currentFeed])
        {
            camImage.texture = camFeeds[currentFeed];
            camText.text = $"CAM {currentFeed + 1}";
        }

        if (Time.time > chanceTimestamp + delay)
        {
            if (Mathf.Round(Random.Range(1, chance)) == 1)
            {
                blockImage.SetActive(true);
                blockTimestamp = Time.time;
            }
            chanceTimestamp = Time.time;
        }

        if (Time.time > blockTimestamp + blockDuration && blockImage.activeSelf)
        {
            blockImage.SetActive(false);
        }

        if (currentFeed + 1 == camFeeds.Length && !cutsceneMonster.activeSelf  && !GameManager.instance.cameraCutscene)
        {
            cutsceneMonster.SetActive(true);
            cutsceneTimestamp = Time.time;
        }

        if (cutsceneMonster.activeSelf && Time.time > cutsceneTimestamp + cutsceneDuration1)
        {
            monsterRoar.Play();
            cutsceneDuration1 += cutsceneDuration2;
        }

        if (cutsceneMonster.activeSelf && Time.time > cutsceneTimestamp + cutsceneDuration2)
        {
            cutsceneMonster.SetActive(false);
            GameManager.instance.cameraCutscene = true;
        }
    }

    private void FixedUpdate()
    {
        if (cutsceneMonster.activeSelf)
        {
            cutsceneMonster.GetComponent<Rigidbody>().velocity = cutsceneMonster.transform.forward * monsterSpeed * Time.fixedDeltaTime;
        }
    }
}
