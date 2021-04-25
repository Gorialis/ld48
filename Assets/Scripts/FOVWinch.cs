using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVWinch : MonoBehaviour
{
    public Camera m_Camera;
    public float targetHeight = 10f;

    private float actualHeight = 10f;
    private float heightVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        actualHeight = targetHeight;
    }

    // Update is called once per frame
    void Update()
    {
        actualHeight = Mathf.SmoothDamp(actualHeight, targetHeight, ref heightVelocity, 0.2f);

        m_Camera.fieldOfView = 2 * Mathf.Rad2Deg * Mathf.Atan2(actualHeight, -transform.position.z);
    }
}
