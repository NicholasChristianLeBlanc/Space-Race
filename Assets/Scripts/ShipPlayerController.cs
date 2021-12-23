using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShipPlayerController : ShipController
{
    [Header("Deadzones")]
    [SerializeField] [Range(0, 2000)] private int mouseHorizontalDeadzone = 10;
    [SerializeField] [Range(0, 2000)] private int mouseVerticalDeadzone = 10;

    [Header("UI Components")]
    [SerializeField] private PauseMenuController pauseMenu;
    [SerializeField] private Ui ui;

    [SerializeField] private Transform mouseSprite;

    private bool forward = true;
    private bool controlEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
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
            if (forwardSpeed > 0)
            {
                forward = true;
            }
            else if (forwardSpeed < 0)
            {
                forward = false;
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
            
        }
    }

    public void SetTrackFinished(bool finished)
    {
        ui.SetTrackFinished(finished);

        if (finished)
        {
            SetControlsEnabled(false);
        }
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlEnabled = enabled;
    }

    public float GetForwardSpeed()
    {
        return forwardSpeed;
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public float GetMaxReverseSpeed()
    {
        return maxReverseSpeed;
    }

    public void FinishLap()
    {
        ui.FinishLap();
    }
}
