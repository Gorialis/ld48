using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamInteractable : MonoBehaviour
{
    public Animator m_Animator;
    public bool active = false;

    public Animator m_targetAnimator;
    public string m_targetField;

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

        activation = Mathf.SmoothDamp(activation, active ? 1.0f : 0.0f, ref activationVelocity, 1.0f);
        m_targetAnimator.SetFloat(m_targetField, activation);
    }
}
