using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class ShipController : MonoBehaviour
{
    // Forces
    [SerializeField] [Range(0, 100)] private float thrustForce = 10;
    [SerializeField] [Range(0, 100)] private float reverseThrustForce = 10;
    [SerializeField] [Range(0, 500)] private float pitchForce = 20;
    [SerializeField] [Range(0, 500)] private float yawForce = 20;
    [SerializeField] [Range(0, 100)] private float rollForce = 20;

    // Max Speeds
    [SerializeField] [Range(0, 5000)] protected float maxSpeed = 2500;
    [SerializeField] [Range(0, 5000)] protected float maxReverseSpeed = 1500;
    [SerializeField] [Range(0, 5000)] private float maxPitchSpeed = 200;
    [SerializeField] [Range(0, 5000)] private float maxYawSpeed = 200;
    [SerializeField] [Range(0, 5000)] private float maxRollSpeed = 200;

    protected Rigidbody rigidbody;
    protected float forwardSpeed, pitchSpeed, yawSpeed, rollSpeed;

    Vector3 angularVelocity = new Vector3(0, 0, 0);

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
        if (forwardSpeed > 0 || forwardSpeed < 0)
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
        rigidbody.velocity = transform.forward * forwardSpeed;
        
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
        if (rollSpeed < 0)
        {
            rollSpeed += Time.fixedDeltaTime * rollForce * 2;
        }
        else
        {
            rollSpeed += Time.fixedDeltaTime * rollForce;
        }

        if (rollSpeed > maxRollSpeed)
        {
            rollSpeed = maxRollSpeed;
        }
    }
    protected void RollRight()
    {
        if (rollSpeed > 0)
        {
            rollSpeed -= Time.fixedDeltaTime * rollForce * 2;
        }
        else
        {
            rollSpeed -= Time.fixedDeltaTime * rollForce;
        }

        if (rollSpeed < -maxRollSpeed)
        {
            rollSpeed = -maxRollSpeed;
        }
    }

    // controls the thrust being applied to to forward and backward momentum 
    protected void ForwardThrust()
    {
        if (forwardSpeed < 0)
        {
            forwardSpeed += Time.fixedDeltaTime * thrustForce * 2;
        }
        else
        {
            forwardSpeed += Time.fixedDeltaTime * thrustForce;
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
            forwardSpeed -= Time.fixedDeltaTime * reverseThrustForce * 2;
        }
        else
        {
            forwardSpeed -= Time.fixedDeltaTime * reverseThrustForce;
        }

        if (forwardSpeed < -maxReverseSpeed)
        {
            forwardSpeed = -maxReverseSpeed;
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
