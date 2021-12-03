using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipPlayerController : ShipController
{
    [SerializeField] private Transform mouseSprite;
    [SerializeField] private Image thrustImage;
    [SerializeField] private Image reverseImage;

    [SerializeField] [Range(0, 2000)] private int mouseHorizontalDeadzone = 10;
    [SerializeField] [Range(0, 2000)] private int mouseVerticalDeadzone = 10;

    private bool forward = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
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

        mouseSprite.position = Input.mousePosition;

        Vector3 mousePos = Input.mousePosition;
        mousePos.x -= Screen.width / 2;
        mousePos.y -= Screen.height / 2;

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

        if (Input.GetKey(KeyCode.W))
        {
            ForwardThrust();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ReverseThrust();
        }

        if (Input.GetKey(KeyCode.A)) // roll left
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
        else if (Input.GetKey(KeyCode.D)) // roll right
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
}
