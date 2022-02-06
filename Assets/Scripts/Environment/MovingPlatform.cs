using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Vector2 start;
    [SerializeField] Vector2 end;
    Vector2 dirToEnd;

    [SerializeField] GameObject trackRef;
    GameObject track;
    LineRenderer lr;
    Rigidbody2D rb;
    [SerializeField] float movePercentage = 0.02f;
    [SerializeField] float maxMoveSpeed = 2;
    [SerializeField] float minMoveSpeed = 0.03f;
    [SerializeField] float divisor = 2;
    bool movingToEnd = true;

    // Start is called before the first frame update
    void Start()
    {
        track = Instantiate(trackRef);
        lr = track.GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

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
        dirToEnd = (end - start).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed;
        // TODO: Change velocity instead of changing position

        if (movingToEnd)
        {
            speed = Vector2.Distance(transform.position, end)/ divisor;
            float toStart = Vector2.Distance(transform.position, start);
            if (toStart/divisor < speed)
            {
                speed = toStart/ divisor;
            }
            speed = Mathf.Clamp(speed, -maxMoveSpeed, maxMoveSpeed);

            if(speed < minMoveSpeed)
            {
                speed = minMoveSpeed;
            }

            rb.velocity = dirToEnd * speed;
        }
        else
        {
            speed = Vector2.Distance(transform.position, end);
            float toStart = Vector2.Distance(transform.position, start)/ divisor;
            if (toStart/ divisor < speed)
            {
                speed = toStart/ divisor;
            }
            speed = Mathf.Clamp(speed, -maxMoveSpeed, maxMoveSpeed);

            if (speed < minMoveSpeed)
            {
                speed = minMoveSpeed;
            }

            rb.velocity = dirToEnd * speed * -1;
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
