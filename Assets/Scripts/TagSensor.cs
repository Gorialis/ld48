using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagSensor : MonoBehaviour
{
    public Transform[] detecting;
    public float range = 5.0f;

    public Animator animator;
    public string field;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool enabled = false;

        foreach(Transform check in detecting)
        {
            if ((check.position - transform.position).magnitude <= range)
            {
                enabled = true;
                break;
            }
        }

        animator.SetBool(field, enabled);
    }
}
