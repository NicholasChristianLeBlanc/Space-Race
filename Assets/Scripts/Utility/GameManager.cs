using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int numberOfLaps = 0;
    [SerializeField] [Range(5, 100)] private int percentDNF = 10;

    // Start is called before the first frame update
    void Awake()
    {
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

        foreach (GameObject players in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.transform.root.GetComponentInChildren<TrackProgress>().SetLaps(numberOfLaps);
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
