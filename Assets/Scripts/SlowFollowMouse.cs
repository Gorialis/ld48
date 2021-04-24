using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFollowMouse : MonoBehaviour
{
    public Camera m_Camera;
    public float m_Smoothness;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate target position
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.localPosition.z);
        Vector3 target = m_Camera.ScreenToWorldPoint(mousePosition);

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, m_Smoothness);
    }
}
