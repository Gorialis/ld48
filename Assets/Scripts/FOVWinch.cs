using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class FOVWinch : MonoBehaviour
{
    public Camera m_Camera;
    public PostProcessVolume m_PPVolume;
    public float targetHeight = 10f;
    public float targetDistance = 25f;

    private DepthOfField dof;

    private float actualHeight = 10f;
    private float actualDistance = 25f;
    private float heightVelocity = 0.0f;
    private float distanceVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        actualHeight = targetHeight;
        actualDistance = targetDistance;

        m_PPVolume.profile.TryGetSettings(out dof);
    }

    // Update is called once per frame
    void Update()
    {
        actualHeight = Mathf.SmoothDamp(actualHeight, targetHeight, ref heightVelocity, 0.2f);
        actualDistance = Mathf.SmoothDamp(actualDistance, targetDistance, ref distanceVelocity, 1.0f);

        m_Camera.fieldOfView = 2 * Mathf.Rad2Deg * Mathf.Atan2(actualHeight, -transform.position.z);
        m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, -actualDistance);
        dof.focusDistance.value = actualDistance;
        dof.aperture.value = 125.0f / Mathf.Pow(actualDistance, 2);
    }
}
