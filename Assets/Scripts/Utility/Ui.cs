using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui : MonoBehaviour
{
    [SerializeField] private PauseMenuController pauseMenu;

    [SerializeField] private Transform mouseSprite;
    [SerializeField] private Image thrustImage;
    [SerializeField] private Image reverseImage;

    [SerializeField] private TextMeshProUGUI position;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI lapTime;

    [SerializeField] private TextMeshProUGUI checkpointTime;
    [SerializeField] private TextMeshProUGUI checkpointComparison;

    [SerializeField] private Image finalImage;
    [SerializeField] private TextMeshProUGUI finalPosition;
    [SerializeField] private TextMeshProUGUI finalTime;
    [SerializeField] private TextMeshProUGUI finalLapTime;

    private float timer = 0;
    private float lapTimer = 0;
    private float checkpointTimer = 0;
    private static float checkpointTimerMax = 2;
    private float positionTimer = 0;
    private float positionTimerPaused = 0;
    private static float positionTimerPausedDefault = 2;
    private float fastestLap = 0;

    private Vector3 finalPositionStart = Vector3.zero;
    private Vector3 finalPositionEnd = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Used to set the position of the player that the Ui is attached to
    public void SetPosition(int position)
    {

    }

    public void FinishRace()
    {

    }
}
