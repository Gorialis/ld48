using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public Animator m_Animator;
    public Rigidbody2D m_Rigidbody2D;
    public float m_Speed;
    public float m_Smoothing;

    private float targetFacingDirection = 0.0f;
    private float actualFacingDirection = 0.0f;

    // Dummy values for refs
    private float dummyFloat = 0.0f;
    private Vector3 dummyVector3 = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Moving left-right
        float leftRight = Input.GetAxis("Horizontal");

        if (Mathf.Abs(leftRight) > 0.1f)
        {
            // Target facing value should be close to either -1 or 1
            // Cube root the value to keep its sign and bring it closer to a magnitude of 1
            targetFacingDirection = Mathf.Sign(leftRight) * Mathf.Pow(Mathf.Abs(leftRight), 1.0f / 3.0f);

            // Apply the actual genuine value to the velocity of the rigidbody
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, new Vector2(leftRight * m_Speed, m_Rigidbody2D.velocity.y), ref dummyVector3, m_Smoothing);
        }

        actualFacingDirection = Mathf.SmoothDamp(actualFacingDirection, targetFacingDirection, ref dummyFloat, m_Smoothing);

        m_Animator.SetFloat("FacingDirection", actualFacingDirection);
    }
}
