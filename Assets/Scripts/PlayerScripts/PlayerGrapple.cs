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
    [SerializeField] LayerMask groundLayer;

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
            RaycastHit2D hit = Physics2D.Raycast(playerPos, mouseDir, 20f, groundLayer);
            if(hit.collider != null)
            {
                Debug.Log(hit.point);
                grapplePoint = hit.point;
                grappleDir = (grapplePoint - playerPos).normalized;
                pc.StateMachine.ChangeState(pc.GrappleState);
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
        }
        if(pc.StateMachine.CurrentState != pc.GrappleState || Vector2.Distance(playerPos, grapplePoint) < 1)
        {
            lr.enabled = false;
        }
    }
}
