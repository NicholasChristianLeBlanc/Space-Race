using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Transform m_chaseTarget;
    [SerializeField] private bool firstPerson = false;

    [SerializeField] private float m_distance = 7.0f;
    [SerializeField] private float m_height = 3.0f;

    private bool isFollowing = true;

    private Vector3 m_lookAtLocation;
    private Rigidbody m_rigidbody;

    private void Start()
    {
        if (m_chaseTarget.TryGetComponent(out Rigidbody rigidbody))
        {
            m_rigidbody = m_chaseTarget.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("Unable to find Rigidbody in Chase Target");
        }
        
    }

    private void Update()
    {
        if (isFollowing)
        {
            if (firstPerson)
            {
                // create a new position for the camera which shopuld be right in front of the chase target
                Vector3 worldPosition = m_chaseTarget.position + m_chaseTarget.transform.forward;
                // Move the camera
                transform.position = worldPosition;

                // Make sure the camera is looking at the chase target
                m_lookAtLocation = m_chaseTarget.transform.forward * 1000;
                transform.LookAt(m_lookAtLocation);
                // adjust the rotation of the camera on the z (forward) axis to match that of the chase target's
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_chaseTarget.eulerAngles.z);
            }
            else
            {
                Vector3 desiredPosition;

                bool axisFlip = false;
                if (m_rigidbody)
                {
                    Vector3 localVelocity = m_chaseTarget.InverseTransformDirection(m_rigidbody.velocity);

                    // Verify local z velocity is less than -0.5f
                    if (localVelocity.z < -0.5f)
                    {
                        axisFlip = true;
                    }
                }

                Vector3 worldPosition = Vector3.zero;
                if (!axisFlip)
                {
                    desiredPosition = m_chaseTarget.position + m_chaseTarget.transform.forward * -1;
                }
                else
                {
                    desiredPosition = m_chaseTarget.position + m_chaseTarget.transform.forward;
                }

                transform.position = desiredPosition;

                // Make sure the camera is looking at the chase target
                m_lookAtLocation = m_chaseTarget.position;
                transform.LookAt(m_lookAtLocation);

                // adjust the rotation of the camera on the z (forward) axis to match that of the chase target's
                if (!axisFlip)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, m_chaseTarget.eulerAngles.z);
                }
                else
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -m_chaseTarget.eulerAngles.z);
                }
                transform.Translate(0, m_height, -m_distance, Space.Self);
            }
        }
    }

    public void SetIsFollowing(bool following)
    {
        isFollowing = following;
    }
}
