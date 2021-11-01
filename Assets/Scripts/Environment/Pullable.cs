using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    PlayerControls pc;
    PlayerGrapple grapple;
    public bool beingPulled = false;
    [SerializeField] Vector2 start;
    [SerializeField] Vector2 end;
    Vector2 workspace;
    Vector2 location;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        grapple = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
        pc = player.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        location.Set(transform.position.x, Mathf.Clamp(transform.position.y, start.y, end.y));
        location.Set(Mathf.Clamp(transform.position.x, start.x, end.x), location.y);

        transform.position = location;
        
        if (beingPulled)
        {
            if(pc.StateMachine.CurrentState == pc.PullState)
            {
                if (player.transform.position.x > this.transform.position.x)
                {
                    workspace.Set(Mathf.Lerp(rb.velocity.x, 5, 0.9f), rb.velocity.y);
                    rb.velocity = workspace;
                }
                else
                {
                    workspace.Set(Mathf.Lerp(rb.velocity.x, -5, 0.9f), rb.velocity.y);
                    rb.velocity = workspace;
                }
                if (player.transform.position.y > this.transform.position.y)
                {
                    workspace.Set(rb.velocity.x, Mathf.Lerp(rb.velocity.y, 5, 0.9f));
                    rb.velocity = workspace;

                }
                else
                {
                    workspace.Set(rb.velocity.x, Mathf.Lerp(rb.velocity.y, -5, 0.9f));
                    rb.velocity = workspace;
                }
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, start) < 0.2f || Vector2.Distance(transform.position, end) < 0.2f)
            {
                rb.velocity = Vector2.zero;
            }
            else
            {
                workspace.Set(Mathf.Lerp(rb.velocity.x, 0, 0.9f), Mathf.Lerp(rb.velocity.y, 0, 0.9f));
                rb.velocity = workspace;
            }
        }
    }
}
