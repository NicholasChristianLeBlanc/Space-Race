using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject[] linkedFinishLines = null;

    private int percentDNF = 10;
    private int playersCount = 0;
    private int playersFinished = 0;
    private bool startCountdown = false;

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = true;

        playersCount = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform root = other.transform.root;

        if (root.CompareTag("Player"))
        {
            TrackProgress rootTracker = root.GetComponentInChildren<TrackProgress>();

            if (rootTracker.CheckCompleteProgress() == true)
            {
                if (rootTracker.GetLapsLeft() <= 0)
                {
                    root.GetComponentInChildren<HandlePlayerFinished>().SetPlayerFinished(true);

                    playersFinished++;

                    if (playersFinished / playersCount >= (percentDNF / 100))
                    {
                        startCountdown = true;
                    }

                    if (linkedFinishLines != null)
                    {
                        foreach(GameObject finishLine in linkedFinishLines)
                        {
                            FinishLine component = finishLine.GetComponent<FinishLine>();

                            component.SetStartCountdown(startCountdown);
                            component.SetPlayersFinished(playersFinished);
                        }
                    }
                }
                else
                {
                    rootTracker.NextLap();
                }
            }
            else
            {
                if (linkedFinishLines != null)
                {
                    foreach(GameObject finishLine in linkedFinishLines)
                    {
                        rootTracker.PassedCheckpoint(finishLine);
                    }
                }
                rootTracker.PassedCheckpoint(gameObject);
            }
        }
    }

    public void SetPercentDNF(int percent)
    {
        percentDNF = percent;
    }

    public void SetPlayersFinished(int finished)
    {
        playersFinished = finished;
    }

    public int GetPlayersFinished()
    {
        return playersFinished;
    }

    public void SetStartCountdown(bool newStartCountdown)
    {
        startCountdown = newStartCountdown;
    }

    public bool GetStartCountdown()
    {
        return startCountdown;
    }
}
