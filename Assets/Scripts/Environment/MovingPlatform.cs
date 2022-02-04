using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2 start;
    [SerializeField] Vector2 end;
    [SerializeField] GameObject trackRef;
    GameObject track;
    LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        track = Instantiate(trackRef);
        lr = track.GetComponent<LineRenderer>();

        // Create squares at start and end points
        GameObject startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        startCube.transform.position = new Vector2(start.x, start.y);
        startCube.transform.localScale = new Vector2(0.1f, 0.1f);

        GameObject endCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        endCube.transform.position = new Vector2(end.x, end.y);
        endCube.transform.localScale = new Vector2(0.1f, 0.1f);

        // Make line along path
        lr.SetPosition(0, new Vector2(start.x, start.y));
        lr.SetPosition(1, new Vector2(end.x, end.y));

        transform.position = start;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
