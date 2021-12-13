using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackProgress : MonoBehaviour
{
    [SerializeField] private GameObject playerCharacter;

    private GameObject[] checkpoints;
    private bool[] activeCheckpoints;
    private int laps = 0;
    private int lapsLeft = 0;
    private Transform spawnpoint;

    private void Awake()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        activeCheckpoints = new bool[checkpoints.Length];

        spawnpoint = gameObject.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // change this to if gameObject goes out of bounds
        {
            if (playerCharacter != null)
            {
                playerCharacter.transform.position = spawnpoint.position - spawnpoint.forward * 10;
                playerCharacter.transform.rotation = spawnpoint.rotation;
                
                if (gameObject.TryGetComponent<ShipPlayerController>(out ShipPlayerController playerController))
                {
                    playerController.ResetSpeeds();
                }
                else if (gameObject.TryGetComponent<ShipAIController>(out ShipAIController aiController))
                {
                    aiController.ResetSpeeds();
                }
            }
        }
    }

    public bool CheckCompleteProgress()
    {
        bool trackCompleted = true;
        foreach (bool ac in activeCheckpoints)
        {
            if (ac == false)
            {
                trackCompleted = false;
                break;
            }
        }

        return trackCompleted;
    }

    public void PassedCheckpoint(GameObject other)
    {
        if (other.tag == "Checkpoint")
        {
            int index = 0;
            bool checkpointFound = false;
            foreach (GameObject go in checkpoints)
            {
                if (go.name == other.name)
                {
                    checkpointFound = true;
                    break;
                }
                index++;
            }

            if (checkpointFound)
            {
                //Debug.Log("Index: " + index + " | checkpoint length: " + activeCheckpoints.Length);

                if (activeCheckpoints[index] == true)
                {
                    Debug.Log("Checkpoint already passed");
                }
                else
                {
                    activeCheckpoints[index] = true;
                    spawnpoint = other.transform;
                }
            }
            else
            {
                Debug.Log("checkpoint not found");
            }
        }
    }

    public int GetPassedCheckpoints()
    {
        int i = 0;
        foreach(bool checkpointActive in activeCheckpoints)
        {
            if (checkpointActive == true)
            {
                i++;
            }
        }

        return i;
    }

    public float GetNearestCheckpointDistance()
    {
        int index = 0;
        float distance = float.MaxValue;

        foreach (GameObject checkpoint in checkpoints)
        {
            if (activeCheckpoints[index] != true)
            {
                float checkDistance = Vector3.Distance(checkpoint.transform.position, transform.position);

                if (checkDistance < distance)
                {
                    distance = checkDistance;
                }
            }

            index++;
        }

        return distance;
    }

    public void SetLaps(int newLaps)
    {
        laps = newLaps;

        if (laps >= 0)
        {
            lapsLeft = laps;
        }
        else
        {
            lapsLeft = 0;
        }
    }

    public void NextLap()
    {
        lapsLeft--;

        if (lapsLeft < 0)
        {
            lapsLeft = 0;
        }

        for (int i = 0; i < activeCheckpoints.Length; i++)
        {
            activeCheckpoints[i] = false;
        }

        if (gameObject.TryGetComponent<ShipPlayerController>(out ShipPlayerController playerController))
        {
            playerController.FinishLap();
        }
    }

    public int GetLapsLeft()
    {
        return lapsLeft;
    }
}
