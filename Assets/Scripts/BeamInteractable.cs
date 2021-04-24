using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamInteractable : MonoBehaviour
{
    public Animator m_Animator;
    public Rigidbody2D m_Rigidbody2D;
    public bool pullable = false;

    public Vector3 target;
    public bool active = false;

    public Animator m_targetAnimator;
    public string m_targetField;

    public float m_Smoothing = 0.5f;
    private Vector3 movementVelocity;

    private float brightness = 0.0f;
    private float brightnessVelocity = 0.0f;

    private float activation = 0.0f;
    private float activationVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        brightness = Mathf.SmoothDamp(brightness, active ? 1.0f : 0.0f, ref brightnessVelocity, 0.25f);
        m_Animator.SetFloat("Brightness", brightness);

        if (pullable)
        {
            Vector3 distanceToTarget = target - transform.position;

            // Clamp magnitude
            if (distanceToTarget.magnitude > 10)
            {
                distanceToTarget *= (10.0f / distanceToTarget.magnitude);
            }

            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, distanceToTarget, ref movementVelocity, m_Smoothing);
        } else
        {
            activation = Mathf.SmoothDamp(activation, active ? 1.0f : 0.0f, ref activationVelocity, 1.0f);
            m_targetAnimator.SetFloat(m_targetField, activation);
        }
    }
}
