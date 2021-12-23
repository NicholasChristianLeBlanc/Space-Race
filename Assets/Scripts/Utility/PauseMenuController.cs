using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    [Header("General Options")]
    [SerializeField] private bool showInEditor = true;

    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI pausedText;

    [SerializeField] private Button resume;

    [Header("Setting Options")]
    [Tooltip("Set if the Settings Menu is opened or not in the Editor")] [SerializeField] private bool settingsOpened = false;
    [SerializeField] private bool settingsEnabled = true;
    [SerializeField] private Button settings;
    [SerializeField] private Canvas settingsMenu;

    [Header("Quit Options")]
    [Tooltip("Set if the Quit Box is opened or not in the Editor")] [SerializeField] private bool quitOpened = false;
    [SerializeField] private Button quitButton;
    [SerializeField] private Canvas quitBox;
    [SerializeField] private Button quitYes;
    [SerializeField] private Button quitNo;

    private bool isPaused = false;
    
    private void OnValidate()
    {
        if (showInEditor)
        {
            background.enabled = true;

            if (settingsOpened)
            {
                settingsMenu.gameObject.SetActive(true);

                pausedText.enabled = false;
                resume.gameObject.SetActive(false);
                settings.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);

                quitBox.enabled = false;
            }
            else
            {
                pausedText.enabled = true;
                resume.gameObject.SetActive(true);

                settings.gameObject.SetActive(true);
                settingsMenu.gameObject.SetActive(false);

                quitButton.gameObject.SetActive(true);
                
                if (quitOpened)
                {
                    quitBox.enabled = true;
                }
                else
                {
                    quitBox.enabled = false;
                }
            }
        }
        else
        {
            background.enabled = false;
            pausedText.enabled = false;
            resume.gameObject.SetActive(false);
            settings.gameObject.SetActive(false);
            settingsMenu.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            quitBox.enabled = false;
        }
    }

    private void Start()
    {
        background.enabled = true;
        pausedText.enabled = false;
        resume.gameObject.SetActive(false);

        if (settingsEnabled)
        {
            settings.interactable = true;
        }
        else
        {
            settings.interactable = false;
        }

        settings.gameObject.SetActive(false);
        settingsMenu.enabled = false;
        settingsOpened = false;

        quitButton.gameObject.SetActive(false);
        quitBox.enabled = false;

        resume.onClick.AddListener(ManageOpenClose);
        settings.onClick.AddListener(ManageSettings);

        quitButton.onClick.AddListener(ManageQuitBox);
        quitYes.onClick.AddListener(ManageQuit);
        quitNo.onClick.AddListener(ManageQuitBox);
    }

    private void Update()
    {
        if (isPaused)
        {
            background.fillAmount += Time.unscaledDeltaTime * 3;

            if (background.fillAmount >= 1)
            {
                background.fillAmount = 1;

                if (!settingsOpened)
                {
                    pausedText.enabled = true;
                    resume.gameObject.SetActive(true);
                    settings.gameObject.SetActive(true);
                    quitButton.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            background.fillAmount -= Time.unscaledDeltaTime * 3;

            pausedText.enabled = false;
            resume.gameObject.SetActive(false);

            settings.gameObject.SetActive(false);
            settingsMenu.enabled = false;

            quitButton.gameObject.SetActive(false);
            quitBox.enabled = false;

            if (background.fillAmount <= 0)
            {
                background.fillAmount = 0;
            }
        }
    }

    public void ManageOpenClose()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            settingsOpened = false;

            Time.timeScale = 1;
        }
    }

    public void ManageSettings()
    {
        settingsOpened = !settingsOpened;

        if (settingsOpened)
        {
            pausedText.enabled = false;
            resume.gameObject.SetActive(false);
            settings.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);

            quitOpened = false;
            quitBox.enabled = false;
            
            settingsMenu.enabled = true;
        }
        else
        {
            background.enabled = true;
            pausedText.enabled = true;
            resume.gameObject.SetActive(true);

            settings.gameObject.SetActive(true);
            settingsMenu.enabled = false;

            quitButton.gameObject.SetActive(true);

            quitBox.enabled = false;
        }
    }

    private void ManageQuitBox()
    {
        quitOpened = !quitOpened;

        if (quitOpened)
        {
            quitBox.enabled = true;
        }
        else
        {
            quitBox.enabled = false;
        }
    }

    private void ManageQuit()
    {

    }
}
