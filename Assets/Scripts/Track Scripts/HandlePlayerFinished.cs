using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandlePlayerFinished : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;

    private ShipPlayerController userControls = null;

    //private UnityStandardAssets.Vehicles.Car.CarUserControl userControls;
    //private UnityStandardAssets.Vehicles.Car.CarAIControl aiControls;

    private void Awake()
    {
        userControls = gameObject.transform.root.GetComponentInChildren<ShipPlayerController>();

        //userControls = gameObject.transform.root.GetComponentInChildren<UnityStandardAssets.Vehicles.Car.CarUserControl>();
        //aiControls = gameObject.transform.root.GetComponentInChildren<UnityStandardAssets.Vehicles.Car.CarAIControl>();
    }

    public void SetPlayerFinished(bool newBool)
    {
        if (newBool == true)
        {
            if (userControls != null)
            {
                playerCamera.GetComponent<ChaseCamera>().SetIsFollowing(false);
                userControls.SetTrackFinished(true);
            }
            //else if (aiControls != null)
            //{
                //aiControls.SetDriving(false);
            //}
            //else
            //{
                //Debug.Log("user and ai controls invalid");
            //}
        }
    }
}
