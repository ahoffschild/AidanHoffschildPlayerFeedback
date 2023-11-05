using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;

public class PlayerUIManager : MonoBehaviour
{
    static public PlayerUIManager instance;
    private GameObject pauseUI, endUI, crosshair;
    private Image endImage;
    private GameObject pausedPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<PlayerUIManager>();
        }
        else
        {
            Destroy(gameObject);
        }
        pauseUI = transform.Find("PauseMenu").gameObject;
        endUI = transform.Find("GameEnd").gameObject;
        endImage = transform.Find("GameEnd").gameObject.GetComponent<Image>();
        crosshair = transform.Find("Crosshair").gameObject;
    }

    public void PauseGame(GameObject player)
    {
        Cursor.lockState = CursorLockMode.None;
        pausedPlayer = player;
        pausedPlayer.GetComponent<PlayerControl>().OnDisable();
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
        crosshair.SetActive(false);
    }

    public void UnpauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pausedPlayer.GetComponent<PlayerControl>().OnEnable();
        pausedPlayer = null;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
        crosshair.SetActive(true);
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BlankScreen(float alpha)
    {
        if (!endUI.activeSelf)
        {
            endUI.SetActive(true);
        }
        endImage.color = new Color(0.1f, 0, 0, alpha);  
        //endUI.GetComponent<Image>().color
    }
}
