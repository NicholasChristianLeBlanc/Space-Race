using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject[] linkedCheckpoints = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            if (linkedCheckpoints != null)
            {
                foreach(GameObject checkpoint in linkedCheckpoints)
                {
                    other.transform.root.GetComponent<TrackProgress>().PassedCheckpoint(checkpoint);
                }
            }
            
            other.transform.root.GetComponent<TrackProgress>().PassedCheckpoint(gameObject);
        }
    }
}
