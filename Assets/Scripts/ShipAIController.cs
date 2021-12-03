using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipAIController : ShipController
{
    // taken from AeroplaneAiControl.cs in the standard asset package
    [SerializeField] private float m_LateralWanderDistance = 5;                     // The amount that the ship can wander by when heading for a target
    [SerializeField] private float m_LateralWanderSpeed = 0.11f;                    // The speed at which the ship will wander laterally
    [SerializeField] [Range(0, 1)] private float m_ThrustWanderAmount = 0.11f;      // The amount that the ships thrust/speed can wander when heading to the target
    [SerializeField] private float m_ThrustWanderSpeed = 0.11f;                     // The speed at which the ship will wander
    [SerializeField] private Transform m_Target;                                    // the target to fly towards

    private int thrustInput = 0;
    private int pitchInput = 0;
    private int yawInput = 0;
    private int rollInput = 0;

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (m_Target != null)
        {
            Vector3 relativePos = m_Target.position - transform.position;
            Quaternion lookAtRotation = Quaternion.LookRotation(relativePos, transform.up);
            Quaternion currentRotation = transform.rotation;

            //if (CalcShortestRot(currentRotation.y, lookAtRotation.y) > 0)
            //{
            //    yawInput = 1;
            //}
            //else if (CalcShortestRot(currentRotation.y, lookAtRotation.y) < 0)
            //{
            //    yawInput = -1;
            //}

            //Debug.Log(lookAtRotation.y);
            //Debug.Log(transform.rotation.x);

            //if (currentRotation.y < 0)
            //{
            //    int ammountFits = Mathf.CeilToInt(Mathf.Abs(currentRotation.y) / 360);
            //    int circleAngles = 360 * ammountFits;
            //    currentRotation.y += circleAngles;

            //    Debug.Log(currentRotation.y);
            //}


        }
        else
        {
            
        }
    }

    // Call CalcShortestRot and check its return value.
    // If CalcShortestRot returns a positive value, then this function
    // will return true for left. Else, false for right.
    bool CalcShortestRotDirection(float from, float to)
    {
        // If the value is positive, return true (left).
        if (CalcShortestRot(from, to) >= 0)
        {
            return true;
        }
        return false; // right
    }

    // If the return value is positive, then rotate to the left. Else,
    // rotate to the right.
    float CalcShortestRot(float from, float to)
    {
        // If from or to is a negative, we have to recalculate them.
        // For an example, if from = -45 then from(-45) + 360 = 315.
        if (from < 0)
        {
            from += 360;
        }

        if (to < 0)
        {
            to += 360;
        }

        // Do not rotate if from == to.
        if (from == to ||
           from == 0 && to == 360 ||
           from == 360 && to == 0)
        {
            return 0;
        }

        // Pre-calculate left and right.
        float left = (360 - from) + to;
        float right = from - to;

        // If from < to, re-calculate left and right.
        if (from < to)
        {
            if (to > 0)
            {
                left = to - from;
                right = (360 - to) + from;
            }
            else
            {
                left = (360 - to) + from;
                right = to - from;
            }
        }

        // Determine the shortest direction.
        return ((left <= right) ? left : (right * -1));
    }
}
