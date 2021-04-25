using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringDisplay : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public Transform from;
    public Vector3 fromOffset;
    public Transform to;
    public Vector3 toOffset;


    // Start is called before the first frame update
    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] positions = new Vector3[]
        {
            from.position + fromOffset,
            to.position + toOffset
        };

        lineRenderer.SetPositions(positions);
    }
}
