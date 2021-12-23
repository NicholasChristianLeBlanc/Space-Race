using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui : MonoBehaviour
{
    [SerializeField] private ShipPlayerController playerController;

    [Header("Ui Components")]
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

    private bool positionEnabled = true;
    private bool lapEnabled = true;
    private bool trackFinished = false;

    private int currentPosition = 1;

    private float forwardSpeed = 0;
    private float maxSpeed = 0;
    private float maxReverseSpeed = 0;

    private float timer = 0;
    private float lapTimer = 0;
    private float checkpointTimer = 0;
    private static float checkpointTimerMax = 2;
    private float positionTimer = 0;
    private float positionTimerPaused = 0;
    private static float positionTimerPausedDefault = 2;
    private float fastestLap = float.MaxValue;

    private TrackProgress trackProgress;
    private GameObject[] allPlayers = null;

    private Vector3 finalPositionStart = Vector3.zero;
    private Vector3 finalPositionEnd = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        finalImage.enabled = false;
        finalPosition.enabled = false;
        finalTime.enabled = false;
        finalLapTime.enabled = false;

        if (playerController == null)
        {
            playerController = transform.root.GetComponentInChildren<ShipPlayerController>();
        }

        maxSpeed = playerController.GetMaxSpeed();
        maxReverseSpeed = playerController.GetMaxReverseSpeed();

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
            Debug.Log("triggered");
            positionEnabled = false;
            position.enabled = false;
        }
        else
        {
            allPlayers = GameObject.FindGameObjectsWithTag("Player");

            positionEnabled = true;
            position.enabled = true;

            finalPositionStart = new Vector3(Screen.width / 2, finalPosition.transform.position.y, finalPosition.transform.position.z);
            finalPositionEnd = finalPosition.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
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
                        finalPosition.transform.position = Vector3.Lerp(finalPositionStart, finalPositionEnd, positionTimer);

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
        else
        {
            if (positionEnabled)
            {
                currentPosition = 1;
                foreach (GameObject player in allPlayers)
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

                position.text = currentPosition + "/" + allPlayers.Length;
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


            forwardSpeed = playerController.GetForwardSpeed();

            if (forwardSpeed > 0)
            {
                thrustImage.fillAmount = forwardSpeed / maxSpeed;
                thrustImage.enabled = true;
                reverseImage.enabled = false;
            }
            else if (forwardSpeed < 0)
            {
                reverseImage.fillAmount = forwardSpeed / -maxReverseSpeed;
                reverseImage.enabled = true;
                thrustImage.enabled = false;
            }
            else
            {
                thrustImage.fillAmount = 0;
                thrustImage.enabled = false;

                reverseImage.fillAmount = 0;
                reverseImage.enabled = false;
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

            FinishLap();
        }
    }

    public void FinishLap()
    {
        if (lapTimer < fastestLap)
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

        if (!trackFinished)
        {
            lapTimer = 0;
        }
        checkpointTimer = checkpointTimerMax;
    }
}
