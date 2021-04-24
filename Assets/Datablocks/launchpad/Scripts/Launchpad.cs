using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launchpad : MonoBehaviour
{
    public Animator m_Animator;
    public UserInput m_Player;
    public AudioSource m_AudioSource;

    private bool triggered;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If this is the player, the level is over.
        if (!triggered && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_Animator.SetTrigger("Fling");
            m_AudioSource.Play();
            triggered = true;
            m_Player.OnLevelComplete();
        }
    }
}
