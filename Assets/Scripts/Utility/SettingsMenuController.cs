using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private Canvas parentMenu;

    [SerializeField] private bool visibleInEditor = false;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private bool backgroundVisible = true;

    [Header("Delete Box Options")]
    [SerializeField] private bool deleteOpened = false;
    [SerializeField] private Button deleteAllButton;
    [SerializeField] private Canvas deleteAllMenu;
    [SerializeField] private Button deleteAllYes;
    [SerializeField] private Button deleteAllNo;

    [SerializeField] private Button returnButton;

    private void OnValidate()
    {
        if (visibleInEditor)
        {
            if (backgroundVisible)
            {
                backgroundImage.enabled = true;
            }
            else
            {
                backgroundImage.enabled = false;
            }

            if (deleteOpened)
            {
                deleteAllMenu.enabled = true;
            }
            else
            {
                deleteAllMenu.enabled = false;
            }
        }
        else
        {
            backgroundImage.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        deleteAllButton.onClick.AddListener(ManageDeleteBox);
        deleteAllYes.onClick.AddListener(ManageDeleteAll);
        deleteAllNo.onClick.AddListener(ManageDeleteBox);

        returnButton.onClick.AddListener(ManageReturn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBackgroundVisible(bool visible)
    {
        backgroundVisible = visible;
    }

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            if (backgroundVisible)
            {
                backgroundImage.enabled = true;
            }
        }
        else
        {
            backgroundImage.enabled = false;
        }
    }

    private void ManageDeleteBox()
    {
        deleteAllMenu.enabled = !deleteAllMenu.enabled;
    }

    private void ManageDeleteAll()
    {
        PlayerPrefs.DeleteAll();

        if (PlayerPrefs.GetFloat("Fastest Track Time" + gameObject.scene.name) <= 0)
        {
            PlayerPrefs.SetFloat("Fastest Track Time" + gameObject.scene.name, 99999999999);

            PlayerPrefs.Save();
        }
        if (PlayerPrefs.GetFloat("Fastest Lap Time" + gameObject.scene.name) <= 0)
        {
            PlayerPrefs.SetFloat("Fastest Lap Time" + gameObject.scene.name, 99999999999);

            PlayerPrefs.Save();
        }
    }

    private void ManageReturn()
    {
        if (parentMenu.TryGetComponent<PauseMenuController>(out PauseMenuController menuController))
        {
            menuController.ManageSettings();
        }

    }
}
