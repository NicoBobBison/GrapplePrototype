using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    PlayerControls pc;
    PlayerGrapple grapple;
    public bool beingPulled;
    [SerializeField] Vector2 start;
    [SerializeField] Vector2 end;
    Vector2 workspace;
    Vector2 location;
    PlayerData data;
    [SerializeField] GameObject trackRef;
    GameObject track;
    LineRenderer lr;
    public bool atOriginalPosition = true;
    bool transitioning = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        grapple = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
        pc = player.GetComponent<PlayerControls>();
        data = player.GetComponent<PlayerControls>().playerData;
        track = Instantiate(trackRef);
        lr = track.GetComponent<LineRenderer>();

        // Create squares at start and end points
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector2(start.x, start.y);
        cube.transform.localScale = new Vector2(0.1f, 0.1f);

        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube2.transform.position = new Vector2(end.x, end.y);
        cube2.transform.localScale = new Vector2(0.1f, 0.1f);

        // Make line along path
        lr.SetPosition(0, new Vector2(start.x, start.y));
        lr.SetPosition(1, new Vector2(end.x, end.y));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!transitioning)
        {
            rb.velocity = Vector2.zero;
            if(Vector2.Distance(transform.position, start) > Vector2.Distance(transform.position, end))
            {
                transform.position = end;
            }
            else
            {
                transform.position = start;
            }
        }
        #region Clamp position
        if (start.y < end.y)
        {
            location.Set(transform.position.x, Mathf.Clamp(transform.position.y, start.y, end.y));
        }
        else
        {
            location.Set(transform.position.x, Mathf.Clamp(transform.position.y, end.y, start.y));
        }
        if(start.x < end.x)
        {
            location.Set(Mathf.Clamp(transform.position.x, start.x, end.x), location.y);
        }
        else
        {
            location.Set(Mathf.Clamp(transform.position.x, end.x, start.x), location.y);
        }
        transform.position = location;
        #endregion

        /*
        if (beingPulled)
        {
            travelling = true;
            if(pc.StateMachine.CurrentState == pc.PullState)
            {
                if (player.transform.position.x > this.transform.position.x)
                {
                    workspace.Set(Mathf.Lerp(rb.velocity.x, data.playerPullSpeed, 0.9f), rb.velocity.y);
                    rb.velocity = workspace;
                }
                else
                {
                    workspace.Set(Mathf.Lerp(rb.velocity.x, -data.playerPullSpeed, 0.9f), rb.velocity.y);
                    rb.velocity = workspace;
                }
                if (player.transform.position.y > this.transform.position.y)
                {
                    workspace.Set(rb.velocity.x, Mathf.Lerp(rb.velocity.y, data.playerPullSpeed, 0.9f));
                    rb.velocity = workspace;

                }
                else
                {
                    workspace.Set(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -data.playerPullSpeed, 0.9f));
                    rb.velocity = workspace;
                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, start) < 0.2f || Vector2.Distance(transform.position, end) < 0.2f)
            {
                rb.velocity = Vector2.zero;
                if(Vector2.Distance(transform.position, start) < 0.2f)
                {
                    workspace.Set(start.x, start.y);
                    transform.position = workspace;
                }
                else
                {
                    workspace.Set(end.x, end.y);
                    transform.position = workspace;   
                }
            }
            else
            {
                //workspace.Set(Mathf.Lerp(rb.velocity.x, 0, 0.9f), Mathf.Lerp(rb.velocity.y, 0, 0.9f));
                //rb.velocity = workspace;
            }
        }
        */
    }

    public IEnumerator Transition()
    {
        transitioning = true;
        Vector2 direction = Vector2.zero;
        if (player.transform.position.x > this.transform.position.x)
        {
            direction.Set(1, 0);
        }
        else
        {
            direction.Set(-1, 0);
        }
        if (player.transform.position.y > this.transform.position.y)
        {
            direction.Set(direction.x, 1);
        }
        else
        {
            direction.Set(direction.x, -1);
        }
        if (atOriginalPosition)
        {
            // Moves vertically
            if(start.x == end.x)
            {
                bool endAboveStart = end.y > start.y;
                // Conditional in while loop checks if position is in between start and end points
                while ((endAboveStart && transform.position.y < end.y && direction.y > 0) || (!endAboveStart && transform.position.y > end.y && direction.y < 0))
                {
                    workspace.Set(0, data.playerPullSpeed * direction.y);
                    rb.velocity = workspace;
                    yield return new WaitForEndOfFrame();
                }
            }
            // Moves horizontally
            else if (start.y == end.y)
            {
                bool endRightOfStart = end.x > start.x;
                while ((endRightOfStart && transform.position.x < end.x && direction.x > 0) || (!endRightOfStart && transform.position.x > end.x && direction.x < 0))
                {
                    workspace.Set(data.playerPullSpeed * direction.x, 0);
                    rb.velocity = workspace;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                Debug.LogWarning("Block moves diagonally");
            }
        }
        else
        {
            if(start.x == end.x)
            {
                bool endAboveStart = end.y > start.y;
                // Conditional in while loop checks if position is in between start and end points
                while ((endAboveStart && transform.position.y > start.y && direction.y < 0) || (!endAboveStart && transform.position.y < start.y && direction.y > 0))
                {
                    workspace.Set(0, data.playerPullSpeed * direction.y);
                    rb.velocity = workspace;
                    yield return new WaitForEndOfFrame();
                }
            }
            // Moves horizontally
            else if (start.y == end.y)
            {
                bool endRightOfStart = end.x > start.x;
                while ((endRightOfStart && transform.position.x > start.x && direction.x < 0) || (!endRightOfStart && transform.position.x < start.x && direction.x > 0))
                {
                    workspace.Set(data.playerPullSpeed * direction.x, 0);
                    rb.velocity = workspace;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                Debug.LogWarning("Block moves diagonally");
            }
        }        
        rb.velocity = Vector2.zero;
        atOriginalPosition = !atOriginalPosition;
        transitioning = false;
    }
}
