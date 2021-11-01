using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    LineRenderer lr;
    Camera cam;
    GameObject player;
    Vector2 playerPos;
    public Vector2 grapplePoint;
    public Vector2 grappleDir;
    PlayerControls pc;
    [SerializeField] LayerMask grappleable;
    RaycastHit2D lastHit;

    private void Start()
    {
        cam = Camera.main;
        lr = GetComponent<LineRenderer>();
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControls>();
    }
    private void Update()
    {
        playerPos = player.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDir = (playerPos - mousePos) * -1;
            //Debug.Log("Mouse dir: " + mouseDir);
            RaycastHit2D hit = Physics2D.Raycast(playerPos, mouseDir, 20f, grappleable);
            if(hit.collider != null)
            {
                lastHit = hit;
                Debug.Log(hit.point);
                grapplePoint = hit.point;
                grappleDir = (grapplePoint - playerPos).normalized;
                if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    pc.StateMachine.ChangeState(pc.GrappleState);
                }
                else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Pullable"))
                {
                    Pullable pullScript = hit.transform.gameObject.GetComponent<Pullable>();
                    pc.StateMachine.ChangeState(pc.PullState);
                    pullScript.beingPulled = true;
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            lr.enabled = true;
            lr.SetPosition(0, playerPos);
            lr.SetPosition(1, grapplePoint);
        }
        else
        {
            lr.enabled = false;
            if (lastHit.collider != null)
            {
                if (lastHit.transform.gameObject.layer == LayerMask.NameToLayer("Pullable"))
                {
                    Pullable pullScript = lastHit.transform.gameObject.GetComponent<Pullable>();
                    pc.StateMachine.ChangeState(pc.PullState);
                    pullScript.beingPulled = false;
                }
            }
        }
        if(pc.StateMachine.CurrentState != pc.GrappleState && pc.StateMachine.CurrentState != pc.PullState || Vector2.Distance(playerPos, grapplePoint) < 1)
        {
            lr.enabled = false;
        }
    }
}
