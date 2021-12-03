using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour
{
    [SerializeField] private LineRenderer line;

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = Input.mousePosition;
        screenPoint.z = 10;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(screenPoint);

        line.SetPosition(0,Camera.main.ViewportToWorldPoint(new Vector3(0.5f,0.5f,10.0f)));
        line.SetPosition(1,worldPoint);
    }
}
