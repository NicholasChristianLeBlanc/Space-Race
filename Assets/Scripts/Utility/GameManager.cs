using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberOfLaps = 0;
    [SerializeField] [Range(5, 100)] private int percentDNF = 10;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject players in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.GetComponent<TrackProgress>().SetLaps(numberOfLaps);
        }

        foreach (GameObject checkpoint in GameObject.FindGameObjectsWithTag("Checkpoint"))
        {
            if (checkpoint.TryGetComponent<FinishLine>(out FinishLine component))
            {
                component.SetPercentDNF(percentDNF);
            }
        }
    }

    private void Update()
    {

    }
}
