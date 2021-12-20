using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]

[RequireComponent(typeof(HandlePlayerFinished))]
[RequireComponent(typeof(TrackProgress))]

public class ShipController : MonoBehaviour
{
    // Components
    [Header("Components")]
    [SerializeField] private BoxCollider undersideCollider;

    // Forces
    [Header("Forces")]
    [SerializeField] [Range(0, 50)] private float thrustForce = 25;
    [SerializeField] [Range(0, 50)] private float reverseThrustForce = 25;
    [SerializeField] [Range(0, 100)] private float brakeForce = 50;
    [SerializeField] [Range(0, 50)] private float horizontalThrustForce = 20;
    [SerializeField] [Range(0, 100)] private float pitchForce = 75;
    [SerializeField] [Range(0, 100)] private float yawForce = 75;
    [SerializeField] [Range(0, 100)] private float rollForce = 7;

    // Max Speeds
    [Header("Max Speeds")]
    [SerializeField] [Range(0, 200)] protected float maxSpeed = 45;
    [SerializeField] [Range(0, 200)] protected float maxReverseSpeed = 25;
    [SerializeField] [Range(0, 200)] protected float maxHorizontalSpeed = 25;
    [SerializeField] [Range(0, 200)] private float maxPitchSpeed = 50;
    [SerializeField] [Range(0, 200)] private float maxYawSpeed = 50;
    [SerializeField] [Range(0, 200)] private float maxRollSpeed = 15;

    [SerializeField] private bool forwardDrag = false;

    protected Rigidbody rigidbody;
    protected float forwardSpeed, horizontalSpeed, pitchSpeed, yawSpeed, rollSpeed;

    Vector3 angularVelocity = new Vector3(0, 0, 0);
    Vector3 velocity = new Vector3(0, 0, 0);

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        forwardSpeed = 0;
        pitchSpeed = 0;
        yawSpeed = 0;
        rollSpeed = 0;
    }

    private void LateUpdate()
    {
        if ((forwardSpeed > 0 || forwardSpeed < 0) && forwardDrag)
        {
            if (rigidbody.drag > 0)
            {
                forwardSpeed *= rigidbody.drag;
            }

            if (forwardSpeed <= 0.007 && forwardSpeed >= -0.007)
            {
                forwardSpeed = 0;
            }
        }

        if (horizontalSpeed > 0 || horizontalSpeed < 0)
        {
            if (rigidbody.drag > 0)
            {
                horizontalSpeed *= rigidbody.drag;
            }

            if (horizontalSpeed <= 0.007 && horizontalSpeed >= -0.007)
            {
                horizontalSpeed = 0;
            }
        }
        velocity.z = forwardSpeed;
        velocity.x = horizontalSpeed;
        rigidbody.velocity = transform.TransformDirection(velocity);
        
        if (rollSpeed > 0 || rollSpeed < 0)
        {
            if (rigidbody.angularDrag > 0)
            {
                rollSpeed *= rigidbody.angularDrag;
            }

            if (rollSpeed <= 0.007 && rollSpeed >= -0.007)
            {
                rollSpeed = 0;
            }
        }

        angularVelocity = new Vector3(-pitchSpeed, yawSpeed, rollSpeed);
        rigidbody.angularVelocity = transform.TransformDirection(angularVelocity);
    }

    private void Update()
    {
         
    }

    // controls the thrust being applied to to forward and backward momentum 
    protected void ForwardThrust()
    {
        if (forwardSpeed < 0)
        {
            forwardSpeed += Time.deltaTime * thrustForce * 2;
        }
        else
        {
            forwardSpeed += Time.deltaTime * thrustForce;
        }

        if (forwardSpeed > maxSpeed)
        {
            forwardSpeed = maxSpeed;
        }

        //rigidbody.AddForce(transform.forward * thrustForce);
    }
    protected void ReverseThrust()
    {
        if (forwardSpeed > 0)
        {
            forwardSpeed -= Time.deltaTime * reverseThrustForce * 2;
        }
        else
        {
            forwardSpeed -= Time.deltaTime * reverseThrustForce;
        }

        if (forwardSpeed < -maxReverseSpeed)
        {
            forwardSpeed = -maxReverseSpeed;
        }
    }
    protected void Brake()
    {
        if (forwardSpeed == 0)
        {
            forwardSpeed = 0;
        }
        else if (forwardSpeed > 0)
        {
            forwardSpeed -= brakeForce * Time.deltaTime;
        }
        else if (forwardSpeed < 0)
        {
            forwardSpeed += brakeForce * Time.deltaTime;
        }

        if (forwardSpeed <= 0.007 && forwardSpeed >= -0.007)
        {
            forwardSpeed = 0;
        }
    }

    // controlls sideways thrust
    protected void LeftThrust()
    {
        if (horizontalSpeed > 0)
        {
            horizontalSpeed -= Time.deltaTime * horizontalThrustForce * 2;
        }
        else
        {
            horizontalSpeed -= Time.deltaTime * horizontalThrustForce;
        }

        if (horizontalSpeed < -maxHorizontalSpeed)
        {
            horizontalSpeed = -maxHorizontalSpeed;
        }
    }
    protected void RightThrust()
    {
        if (horizontalSpeed < 0)
        {
            horizontalSpeed += Time.deltaTime * horizontalThrustForce * 2;
        }
        else
        {
            horizontalSpeed += Time.deltaTime * horizontalThrustForce;
        }

        if (horizontalSpeed > maxHorizontalSpeed)
        {
            horizontalSpeed = maxHorizontalSpeed;
        }
    }

    // controlls how fast the ship turns vertically
    protected void ManagePitch(float pitchPercent)
    {
        pitchSpeed = pitchForce * pitchPercent * Time.deltaTime;

        if (pitchSpeed > maxPitchSpeed)
        {
            pitchSpeed = maxPitchSpeed;
        }
        else if (pitchSpeed < -maxPitchSpeed)
        {
            pitchSpeed = -maxPitchSpeed;
        }
    }

    // controls the yaw, in other words where the ship is looking horizontally (left and right)
    protected void ManageYaw(float yawPercent)
    {
        yawSpeed = yawForce * yawPercent * Time.deltaTime;

        if (yawSpeed > maxYawSpeed)
        {
            yawSpeed = maxYawSpeed;
        }
        else if (yawSpeed < -maxYawSpeed)
        {
            yawSpeed = -maxYawSpeed;
        }
    }

    // controls the roll angle of the ship / where the wings are pointing vertically (left means left wing is pointing down, right means right wing is pointing down)
    protected void RollLeft()
    {
        rollSpeed += Time.deltaTime * rollForce;

        if (rollSpeed > maxRollSpeed)
        {
            rollSpeed = maxRollSpeed;
        }
    }
    protected void RollRight()
    {
        rollSpeed -= Time.deltaTime * rollForce;

        if (rollSpeed < -maxRollSpeed)
        {
            rollSpeed = -maxRollSpeed;
        }
    }

    public float GetRollSpeed()
    {
        return rollSpeed;
    }

    public void ResetSpeeds()
    {
        forwardSpeed = 0;
        pitchSpeed = 0;
        yawSpeed = 0;
        rollSpeed = 0;
    }
}
