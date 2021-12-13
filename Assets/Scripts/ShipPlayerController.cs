using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipPlayerController : ShipController
{
    [SerializeField] [Range(0, 2000)] private int mouseHorizontalDeadzone = 10;
    [SerializeField] [Range(0, 2000)] private int mouseVerticalDeadzone = 10;

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

    private bool forward = true;
    private bool positionEnabled = true;
    private bool controlEnabled = true;
    private bool lapEnabled = true;
    private bool trackFinished = false;

    private int currentPosition = 1;

    private TrackProgress trackProgress;
    private GameObject[] otherPlayers = null;

    private float timer = 0;
    private float lapTimer = 0;
    private float checkpointTimer = 0;
    private static float checkpointTimerMax = 2;
    private float positionTimer = 0;
    private float positionTimerPaused = 0;
    private static float positionTimerPausedDefault = 2;
    private float fastestLap = 0;

    private Vector3 positionStart = Vector3.zero;
    private Vector3 positionEnd = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        finalImage.enabled = false;
        finalPosition.enabled = false;
        finalTime.enabled = false;
        finalLapTime.enabled = false;

        trackProgress = transform.root.GetComponentInChildren<TrackProgress>();

        if (trackProgress.GetLapsLeft() <= 0)
        {
            lapTime.enabled = false;
            lapEnabled = false;
        }
        else
        {
            lapTime.enabled = true;
            lapEnabled = true;
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length <= 1)
        {
            positionEnabled = false;

            position.enabled = false;
        }
        else
        {
            otherPlayers = GameObject.FindGameObjectsWithTag("Player");

            positionEnabled = true;
            position.enabled = true;

            positionStart = new Vector3(Screen.width / 2, finalPosition.transform.position.y, finalPosition.transform.position.z);
            positionEnd = finalPosition.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.ManageOpenClose();
        }

        if (controlEnabled)
        {
            if (positionEnabled)
            {
                currentPosition = 1;
                foreach (GameObject player in otherPlayers)
                {
                    TrackProgress otherPlayerTracker = player.GetComponent<TrackProgress>();

                    if (transform.root.name == player.transform.root.name)
                    {
                        continue;
                    }
                    else if (otherPlayerTracker.GetPassedCheckpoints() > trackProgress.GetPassedCheckpoints() || otherPlayerTracker.GetLapsLeft() < trackProgress.GetLapsLeft())
                    {
                        currentPosition++;
                    }
                    else if (otherPlayerTracker.GetPassedCheckpoints() == trackProgress.GetPassedCheckpoints() && otherPlayerTracker.GetLapsLeft() == trackProgress.GetLapsLeft())
                    {
                        if (otherPlayerTracker.GetNearestCheckpointDistance() < trackProgress.GetNearestCheckpointDistance())
                        {
                            currentPosition++;
                        }
                    }
                }

                position.text = currentPosition + "/" + otherPlayers.Length;
            }

            if (checkpointTimer > 0)
            {
                checkpointTime.enabled = true;
                checkpointComparison.enabled = true;

                checkpointTimer -= Time.deltaTime;
            }
            else
            {
                checkpointTime.enabled = false;
                checkpointComparison.enabled = false;
            }

            timer += Time.deltaTime;
            lapTimer += Time.deltaTime;

            var timeSpan = System.TimeSpan.FromSeconds(timer);
            time.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + "." + timeSpan.Milliseconds;

            if (forwardSpeed > 0)
            {
                thrustImage.fillAmount = forwardSpeed / maxSpeed;
                thrustImage.enabled = true;
                reverseImage.enabled = false;

                forward = true;
            }
            else if (forwardSpeed < 0)
            {
                reverseImage.fillAmount = forwardSpeed / -maxReverseSpeed;
                reverseImage.enabled = true;
                thrustImage.enabled = false;

                forward = false;
            }
            else
            {
                thrustImage.fillAmount = 0;
                thrustImage.enabled = false;

                reverseImage.fillAmount = 0;
                reverseImage.enabled = false;

            }

            Vector3 mousePos = Input.mousePosition;
            mousePos.x -= Screen.width / 2;
            mousePos.y -= Screen.height / 2;

            // cap the mouse in the horizontal (x) axis
            if (Input.mousePosition.x > Screen.width)
            {
                mousePos.x = Screen.width / 2;

                mouseSprite.position = new Vector3(Screen.width, mouseSprite.position.y, mouseSprite.position.z);
            }
            else if (Input.mousePosition.x < 0)
            {
                mousePos.x = -Screen.width / 2;

                mouseSprite.position = new Vector3(0, mouseSprite.position.y, mouseSprite.position.z);
            }
            else
            {
                mouseSprite.position = new Vector3(Input.mousePosition.x, mouseSprite.position.y, mouseSprite.position.z); 
            }

            // cap the mouse in the vertical (y) axis
            if (Input.mousePosition.y > Screen.height)
            {
                mousePos.y = Screen.height / 2;

                mouseSprite.position = new Vector3(mouseSprite.position.x, Screen.height, mouseSprite.position.z);
            }
            else if (Input.mousePosition.y < 0)
            {
                mousePos.y = -Screen.height / 2;

                mouseSprite.position = new Vector3(mouseSprite.position.x, 0, mouseSprite.position.z);
            }
            else
            {
                mouseSprite.position = new Vector3(mouseSprite.position.x, Input.mousePosition.y, mouseSprite.position.z);
            }

            // set the mouse origin to the center of the screen
            if (mousePos.x < mouseHorizontalDeadzone && mousePos.x > -mouseHorizontalDeadzone)
            {
                mousePos.x = 0;
            }
            if (mousePos.y < mouseVerticalDeadzone && mousePos.y > -mouseVerticalDeadzone)
            {
                mousePos.y = 0;
            }

            if (forward)
            {
                ManagePitch(mousePos.y / 300);
            }
            else
            {
                ManagePitch(-mousePos.y / 300);
            }
            ManageYaw(mousePos.x / 300);

            // Boost forward/backward/brake
            if (Input.GetKey(KeyCode.W))
            {
                ForwardThrust();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                ReverseThrust();
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                Brake();

                if (forwardSpeed == 0)
                {
                    forward = true;
                }
            }

            // Boost left/right
            if (Input.GetKey(KeyCode.A))
            {
                LeftThrust();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                RightThrust();
            }

            // Roll
            if (Input.GetKey(KeyCode.Q)) // roll left
            {
                if (forward)
                {
                    RollLeft();
                }
                else
                {
                    RollRight();
                }

            }
            else if (Input.GetKey(KeyCode.E)) // roll right
            {
                if (forward)
                {
                    RollRight();
                }
                else
                {
                    RollLeft();
                }

            }

            //Debug.Log("X: " + mousePos.normalized.x + "Y: " + mousePos.normalized.y);
        }
        else
        {
            if (trackFinished)
            {
                if (checkpointTimer > 0)
                {
                    checkpointTime.enabled = true;
                    checkpointComparison.enabled = true;

                    checkpointTimer -= Time.deltaTime;
                }
                else
                {
                    checkpointTime.enabled = false;
                    checkpointComparison.enabled = false;

                    finalImage.enabled = true;
                    finalImage.fillAmount += Time.deltaTime;

                    if (finalImage.fillAmount >= 1)
                    {
                        if (positionEnabled)
                        {
                            positionTimerPaused -= Time.deltaTime;

                            if (positionTimerPaused <= 0)
                            {
                                positionTimer += Time.deltaTime;
                            }

                            finalPosition.enabled = true;
                            finalPosition.transform.position = Vector3.Lerp(positionStart, positionEnd, positionTimer);

                            if (positionTimer >= 1)
                            {
                                positionTimer = 1;

                                finalTime.enabled = true;

                                if (lapEnabled)
                                {
                                    finalLapTime.enabled = true;
                                }
                            }
                        }
                        else
                        {
                            finalTime.transform.position = finalImage.transform.position;
                            finalTime.enabled = true;

                            if (lapEnabled)
                            {
                                finalLapTime.transform.position = new Vector3(finalTime.transform.position.x, finalLapTime.transform.position.y, finalLapTime.transform.position.z);
                                finalLapTime.enabled = true;
                            }
                        }
                    }
                }
            }
        }
    }

    public void SetTrackFinished(bool finished)
    {
        trackFinished = finished;

        if (trackFinished)
        {
            time.enabled = false;

            if (lapEnabled)
            {
                var lapTimeSpan = System.TimeSpan.FromSeconds(fastestLap);
                finalLapTime.text = lapTimeSpan.Hours.ToString("00") + ":" + lapTimeSpan.Minutes.ToString("00") + ":" + lapTimeSpan.Seconds.ToString("00") + "." + lapTimeSpan.Milliseconds;

                lapTime.enabled = false;
            }

            if (positionEnabled)
            {
                position.enabled = false;

                positionTimer = 0;
                positionTimerPaused = positionTimerPausedDefault;

                finalPosition.text = currentPosition.ToString();
            }

            var timeSpan = System.TimeSpan.FromSeconds(timer);
            finalTime.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + "." + timeSpan.Milliseconds;

            if (timer < PlayerPrefs.GetFloat("Fastest Track Time" + gameObject.scene.name))
            {
                PlayerPrefs.SetFloat("Fastest Track Time" + gameObject.scene.name, timer);
            }
            
            SetControlsEnabled(false);
            FinishLap();
        }
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlEnabled = enabled;
    }

    public void FinishLap()
    {
        if (lapTimer > fastestLap)
        {
            fastestLap = lapTimer;

            var timeSpan = System.TimeSpan.FromSeconds(fastestLap);
            lapTime.text = timeSpan.Hours.ToString("00") + ":" + timeSpan.Minutes.ToString("00") + ":" + timeSpan.Seconds.ToString("00") + "." + timeSpan.Milliseconds;
        }

        var lapSpan = System.TimeSpan.FromSeconds(lapTimer);
        checkpointTime.text = lapSpan.Hours.ToString("00") + ":" + lapSpan.Minutes.ToString("00") + ":" + lapSpan.Seconds.ToString("00") + "." + lapSpan.Milliseconds;

        var lapComparison = System.TimeSpan.FromSeconds(lapTimer - PlayerPrefs.GetFloat("Fastest Lap Time" + gameObject.scene.name));
        if (lapTimer - PlayerPrefs.GetFloat("Fastest Lap Time" + gameObject.scene.name) > 0)
        {
            checkpointComparison.color = Color.red; // red
            checkpointComparison.text = "+" + lapComparison.Hours.ToString("00") + ":" + lapComparison.Minutes.ToString("00") + ":" + lapComparison.Seconds.ToString("00") + "." + lapComparison.Milliseconds / 10;
        }
        else
        {
            lapComparison *= -1;

            checkpointComparison.color = Color.blue; // blue
            checkpointComparison.text = "-" + lapComparison.Hours.ToString("00") + ":" + lapComparison.Minutes.ToString("00") + ":" + lapComparison.Seconds.ToString("00") + "." + lapComparison.Milliseconds / 10;


            PlayerPrefs.SetFloat("Fastest Lap Time" + gameObject.scene.name, lapTimer);
            PlayerPrefs.Save();
        }
        
        lapTimer = 0;
        checkpointTimer = checkpointTimerMax;
    }
}
