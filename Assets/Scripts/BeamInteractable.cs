using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamInteractable : MonoBehaviour
{
    public Animator m_Animator;

    public Rigidbody2D m_Rigidbody2D;
    public RigidbodyConstraints2D m_RigidbodyConstraints;
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

        if (!pullable)
        {
            activation = Mathf.SmoothDamp(activation, active ? 1.0f : 0.0f, ref activationVelocity, 1.0f);
            m_targetAnimator.SetFloat(m_targetField, activation);
        }
    }

    private void FixedUpdate()
    {
        if (pullable && active)
        {
            Vector3 distanceToTarget = (target - transform.position) * 4.0f;

            // Clamp magnitude
            if (distanceToTarget.magnitude > 50)
            {
                distanceToTarget *= (50.0f / distanceToTarget.magnitude);
            }

            // Use AddForce to ensure friction
            Vector3 newVelocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, distanceToTarget, ref movementVelocity, m_Smoothing);
            m_Rigidbody2D.AddForce((Vector2)newVelocity - m_Rigidbody2D.velocity, ForceMode2D.Impulse);
        }
    }
}
