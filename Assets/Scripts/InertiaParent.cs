using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertiaParent : MonoBehaviour
{
    public Transform parent;

    private Vector2 change;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void FixedUpdate()
    {
        change = (Vector2)(parent.position - transform.position);
        transform.position = parent.position;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.otherCollider.tag == "Player")
            collision.rigidbody.position += change;
    }
}
