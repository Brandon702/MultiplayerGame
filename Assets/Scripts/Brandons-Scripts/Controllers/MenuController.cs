using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    #region Unity Editor Values
    [Header("Panels")]
    public GameObject MainUI;
    public List<GameObject> panels = new List<GameObject>();
    private GameObject MainMenuPanel;
    private GameObject OptionsPanel;
    private GameObject CreditsPanel;
    private GameObject PausePanel;
    private GameObject InstructionsPanel;
    private GameObject GamePanel;
    private GameObject VideoPanel;

    [Header("Sub-Panels")]
    //Maybe?

    [Header("Other")]
    public GameController gameController;
    public AudioController audioController;
    public AudioMixer mixer;
    List<GameObject> gameObjects = new List<GameObject>();
    private int playing;

    #endregion

    #region Start/Awake/Enable

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        audioController = GameObject.Find("Controllers").GetComponent<AudioController>();
    }

    private void Start()
    {
        populatePanels();
        Disable();
        Time.timeScale = 0;
        GameController.Instance.state = eState.TITLE;
        if (GameController.Instance.state == eState.TITLE)
        {
            VideoPanel.SetActive(true);
            MainMenuPanel.SetActive(true);
        }
        menuTrackPlayer();
    }

    private void OnEnable()
    {
        
    }

    #endregion

    #region Audio
    private void menuTrackPlayer()
    {
        //int trackPlay = UnityEngine.Random.Range(0, 2);
        //if (trackPlay == 1)
        //{
        //    audioController.Play("Track10");
        //    Debug.Log("Track 10 played");
        //    playing = 10;
        //}
        //else
        //{
        //    audioController.Play("Track7");
        //    Debug.Log("Track 7 played");
        //    playing = 7;
        //}
        //audioController.Stop("Track1");

        audioController.Play("Track1");
    }

    private void sceneTrackPlayer()
    {
        //audioController.Stop("Track" + playing);
        //int trackPlay = UnityEngine.Random.Range(0, 2);
        //if (trackPlay == 1)
        //{
        //    audioController.Play("Track5");
        //    playing = 5;
        //    Debug.Log("Track 5 played");
        //}
        //else
        //{
        //    audioController.Play("Track9");
        //    playing = 9;
        //    Debug.Log("Track 9 played");
        //}
    }

    private void gameTrackPlayer()
    {
        //audioController.Stop("Track" + playing);
        //int trackPlay = UnityEngine.Random.Range(0, 4);
        //if (trackPlay == 1)
        //{
        //    audioController.Play("Track1");
        //    Debug.Log("Track 1 played");
        //    playing = 1;
        //}
        //else if (trackPlay == 2)
        //{
        //    audioController.Play("Track2");
        //    Debug.Log("Track 2 played");
        //    playing = 2;
        //}
        //else if (trackPlay == 3)
        //{
        //    audioController.Play("Track3");
        //    Debug.Log("Track 3 played");
        //    playing = 3;
        //}
        //else
        //{
        //    audioController.Play("Track4");
        //    Debug.Log("Track 4 played");
        //    playing = 4;
        //}
    }

    #endregion

    #region Panel Changing

    private void populatePanels()
    {
        MainUI.SetActive(true);
        RectTransform[] transforms = MainUI.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform child in transforms)
        {
            if (child.gameObject.CompareTag("UI"))
            {
                panels.Add(child.gameObject);
                Debug.Log(child.name);
                if (child.name == "MainMenuPanel") { MainMenuPanel = child.gameObject; }
                if (child.name == "OptionsPanel") { OptionsPanel = child.gameObject; }
                if (child.name == "CreditsPanel") { CreditsPanel = child.gameObject; }
                if (child.name == "PausePanel") { PausePanel = child.gameObject; }
                if (child.name == "InstructionsPanel") { InstructionsPanel = child.gameObject; }
                if (child.name == "GamePanel") { GamePanel = child.gameObject; }
                if (child.name == "VideoPanel") { VideoPanel = child.gameObject; }
            }
        }
    }

    public void Disable()
    {
        foreach (GameObject gameObject in panels)
        {
            gameObject.SetActive(false);
        }
    }
    public void StartGame()
    {
        Disable();
        gameTrackPlayer();
        Time.timeScale = 1;
        GameController.Instance.state = eState.GAME;
        GamePanel.SetActive(true);
        SceneManager.LoadScene("Launcher");
    }

    public void ResumeGame()
    {
        Disable();
        Time.timeScale = 1;
        GamePanel.SetActive(true);
        GameController.Instance.state = eState.GAME;
        Debug.Log("Resume Game");
    }

    public void Options()
    {
        Disable();
        OptionsPanel.SetActive(true);
        if (GameController.Instance.state == eState.TITLE)
        {
            VideoPanel.SetActive(true);
        }
        Debug.Log("Options menu");
    }

    public void Instructions()
    {
        Disable();
        InstructionsPanel.SetActive(true);
        if (GameController.Instance.state == eState.TITLE)
        {
            VideoPanel.SetActive(true);
        }
        //GameController.Instance.state = eState.INSTRUCTIONS;
    }

    public void Credits()
    {
        Disable();
        CreditsPanel.SetActive(true);
        if (GameController.Instance.state == eState.TITLE)
        {
            VideoPanel.SetActive(true);
        }
        Debug.Log("Credits menu");
    }
    public void Pause()
    {
        if (GameController.Instance.state == eState.GAME)
        {
            Time.timeScale = 0;
            Disable();
            PausePanel.SetActive(true);
            GameController.Instance.state = eState.PAUSE;
        }
    }

    #endregion

    #region Back
    public void Back()
    {
        Disable();

        if (GameController.Instance.state == eState.PAUSE)
        {
            BackToPause();
        }
        else
        {
            BackToMenu();
        }
    }

    //Back to main menu
    public void BackToMenu()
    {
        Disable();
        if (SceneManager.GetActiveScene().name != "Main")
        {
            SceneManager.LoadScene("Main");
        }
        GamePanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        GameController.Instance.state = eState.TITLE;
        if (GameController.Instance.state == eState.TITLE)
        {
            VideoPanel.SetActive(true);
        }
    }

    //Back to pause menu
    public void BackToPause()
    {
        Disable();
        PausePanel.SetActive(true);
        GameController.Instance.state = eState.PAUSE;
    }

    #endregion
    
    #region Set Audio Levels
    public void SetLevelMST(float sliderValue)
    {
        mixer.SetFloat("MST", Mathf.Log10(sliderValue) * 20);
    }

    public void SetLevelBGM(float sliderValue)
    {
        mixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
    }

    public void SetLevelSFX(float sliderValue)
    {
        mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }
    public void SetLevelAMB(float sliderValue)
    {
        mixer.SetFloat("AMB", Mathf.Log10(sliderValue) * 20);
    }
    public void SetLevelPitch(float sliderValue)
    {
        mixer.SetFloat("Pitch", sliderValue);
    }

    public void Mute(bool mute)
    {
        if (mute) mixer.SetFloat("MST", -80);
        else mixer.SetFloat("MST", 0);
    }

    #endregion

    #region Reset/Exit Game
    public void ResetApplication()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #endregion

}