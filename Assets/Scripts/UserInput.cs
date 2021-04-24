using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    public LevelHolder m_LevelHolder;
    public Camera m_Camera;
    public MusicController m_MusicController;

    public AudioSource m_EngineSound;
    public float m_EngineLoudness = 0.05f;
    public LineRenderer m_LineRenderer;
    public Transform m_BeamOrigin;
    public Transform m_BeamHandle;
    public Animator m_Animator;
    public Rigidbody2D m_Rigidbody2D;
    public float m_Speed;
    public float m_Smoothing;

    public bool m_FollowLevelDepth = true;

    private float targetFacingDirection = 0.0f;
    private float actualFacingDirection = 0.0f;

    // Currently held beam target
    private BeamInteractable currentInteractable = null;
    private float lightBrightness = 0.0f;

    // Velocities for interpolation
    private float facingDirectionVelocity = 0.0f;
    private Vector3 speedVelocity = Vector3.zero;
    private float lightBrightnessVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_LineRenderer.positionCount = 16;
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
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, new Vector2(leftRight * m_Speed, m_Rigidbody2D.velocity.y), ref speedVelocity, m_Smoothing);
        }

        actualFacingDirection = Mathf.SmoothDamp(actualFacingDirection, targetFacingDirection, ref facingDirectionVelocity, m_Smoothing);

        m_Animator.SetFloat("FacingDirection", actualFacingDirection);


        // Try to anchor the main object to the current level as best as possible
        transform.position = new Vector3(transform.position.x, transform.position.y, m_LevelHolder.GetLevelDepth() + (m_FollowLevelDepth ? 0.0f : -15.0f));

        if (Input.GetMouseButtonDown(0))
        {
            // See if we've hit an interactable or not
            BeamInteractable[] targets = m_LevelHolder.GetLevelInteractables();

            foreach (BeamInteractable target in targets)
            {
                // Cast the mouse position to this interactable
                Vector3 relativeToCamera = m_Camera.transform.worldToLocalMatrix.MultiplyPoint(target.transform.position);
                Vector3 clickWorldPosition = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, relativeToCamera.z));

                if ((target.transform.position - clickWorldPosition).magnitude < 5.0f)
                {
                    // Close enough to hit
                    if (currentInteractable != null)
                        currentInteractable.active = false;

                    currentInteractable = target;
                    target.active = true;
                    break;
                }
            }
        }

        if (!Input.GetMouseButton(0) && currentInteractable != null)
        {
            currentInteractable.active = false;
            currentInteractable = null;
        }


        lightBrightness = Mathf.SmoothDamp(lightBrightness, currentInteractable != null ? 1.0f : 0.0f, ref lightBrightnessVelocity, 0.25f); ;
        m_Animator.SetFloat("LightBrightness", lightBrightness);

        // Do beam
        if (currentInteractable == null)
        {
            m_LineRenderer.enabled = false;
        } else
        {
            m_LineRenderer.enabled = true;

            Vector3 interactableStop = currentInteractable.transform.position + (2 * Vector3.back);
            Vector3 interactableEntry = currentInteractable.transform.position + (10 * Vector3.back);

            Vector3[] positions = new Vector3[16];

            for (int i = 0; i < 16; i++)
            {
                positions[i] = BezierTerm((float)i / 15, m_BeamOrigin.position, m_BeamHandle.position, interactableEntry, interactableStop);
            }

            m_LineRenderer.SetPositions(positions);

            // Cast the mouse position to this interactable
            Vector3 relativeToCamera = m_Camera.transform.worldToLocalMatrix.MultiplyPoint(currentInteractable.transform.position);
            currentInteractable.target = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, relativeToCamera.z));
        }

        // Engine sound
        m_EngineSound.volume = m_EngineLoudness * (m_Rigidbody2D.velocity.magnitude + 5.0f);

    }

    Vector3 BezierTerm(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (
            (Mathf.Pow(1 - t, 3) * p0) +
            (3 * Mathf.Pow(1 - t, 2) * t * p1) +
            (3 * (1 - t) * Mathf.Pow(t, 2) * p2) +
            (Mathf.Pow(t, 3) * p3)
        );
    }

    public void DisableFollowLevelDepth()
    {
        m_FollowLevelDepth = false;
    }
    public void EnableFollowLevelDepth()
    {
        m_FollowLevelDepth = true;
    }

    public void OnLevelComplete()
    {
        m_Animator.SetTrigger("Fling");
        m_LevelHolder.OnLevelComplete();
        m_MusicController.phase++;
    }
}
