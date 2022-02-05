using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2 start;
    [SerializeField] Vector2 end;
    [SerializeField] GameObject trackRef;
    Vector2 endDir;
    Vector2 workspace;
    GameObject track;
    LineRenderer lr;
    [SerializeField] float movePercentage = 0.02f;
    [SerializeField] float maxMoveSpeed = 2;
    bool movingToEnd = true;

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

        endDir = (end - start).normalized;
        transform.position = start;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // TODO: Change velocity instead of changing position

        if (movingToEnd)
        {
            workspace.Set(Vector2.Lerp(transform.position, end, movePercentage).x, Vector2.Lerp(transform.position, end, movePercentage).y);
            Vector2.ClampMagnitude(workspace, maxMoveSpeed);
            transform.position = workspace;
        }
        else
        {
            workspace.Set(Vector2.Lerp(transform.position, start, movePercentage).x, Vector2.Lerp(transform.position, start, movePercentage).y);
            Vector2.ClampMagnitude(workspace, maxMoveSpeed);
            transform.position = workspace;
        }
        if(movingToEnd && Vector2.Distance(transform.position, end) < 0.03)
        {
            movingToEnd = !movingToEnd;
        }
        if (!movingToEnd && Vector2.Distance(transform.position, start) < 0.03)
        {
            movingToEnd = !movingToEnd;
        }
    }
}
