using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    LineRenderer lr;
    Camera cam;
    GameObject player;
    Vector3 playerPos;
    Vector3 mousePos;
    public Vector2 grapplePoint;
    public Vector2 grappleDir;
    PlayerControls pc;
    [SerializeField] LayerMask grappleable;
    public RaycastHit2D lastHit;
    public Vector2 workspace;
    [SerializeField] float grappleLerpAmount = 0.5f;
    public enum GrapplingState { searching, pulling, unattached}
    GrapplingState _state = GrapplingState.unattached;

    private void Start()
    {
        cam = Camera.main;
        lr = GetComponent<LineRenderer>();
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControls>();
        playerPos = player.transform.position;
        grapplePoint = playerPos;
    }
    private void Update()
    {
        ChangeBasedOnState();
        MoveGrappleEnd();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerPos = player.transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = CastToMouse();
            if(hit.collider != null)
            {
                _state = GrapplingState.pulling;
                lastHit = hit;
                //Debug.Log("Hit transform: " + hit.point);
                grappleDir = (hit.point - (Vector2)playerPos).normalized;
            }
            else
            {
                _state = GrapplingState.searching;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _state = GrapplingState.unattached;
        }
    }
    void ChangeBasedOnState()
    {
        if(_state == GrapplingState.unattached)
        {
            if (Vector2.Distance(playerPos, grapplePoint) < 0.05f)
            {
                lr.enabled = false;
            }
            else
            {
                lr.enabled = true;
            }
        }
        else if(_state == GrapplingState.searching)
        {
            if (Input.GetMouseButton(0))
            {
                lr.enabled = true;
                if (Physics2D.OverlapCircle(grapplePoint, 0.05f, grappleable))
                {
                    if(CastToMouse().collider != null)
                    {
                        lastHit = CastToMouse();
                        grappleDir = (lastHit.point - (Vector2)playerPos).normalized;
                        _state = GrapplingState.pulling;
                    }
                }
            }
        }
        else
        {
            lr.enabled = true;
            if(_state == GrapplingState.pulling && pc.StateMachine.CurrentState != pc.PullState)
            {
                if(lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Pullable"))
                {
                    pc.StateMachine.ChangeState(pc.PullState);
                }
            }
            if(_state == GrapplingState.pulling && pc.StateMachine.CurrentState != pc.GrappleState)
            {
                if (lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    pc.StateMachine.ChangeState(pc.GrappleState);
                }
            }
        }
    }

    void MoveGrappleEnd()
    {
        if (_state == GrapplingState.pulling)
        {
            grapplePoint.Set(Vector2.Lerp(grapplePoint, lastHit.point, grappleLerpAmount).x,
                Vector2.Lerp(grapplePoint, lastHit.point, grappleLerpAmount).y);
        }else if (_state == GrapplingState.searching)
        {
            grapplePoint.Set(Vector2.Lerp(grapplePoint, mousePos, grappleLerpAmount).x,
                Vector2.Lerp(grapplePoint, mousePos, grappleLerpAmount).y);
        }
        else
        {
            grapplePoint.Set(Vector2.Lerp(grapplePoint, playerPos, grappleLerpAmount).x,
                Vector2.Lerp(grapplePoint, playerPos, grappleLerpAmount).y);
        }
        lr.SetPosition(0, playerPos);
        lr.SetPosition(1, grapplePoint);
    }
    RaycastHit2D CastToMouse()
    {
        Vector2 mouseDir = (playerPos - mousePos) * -1;
        RaycastHit2D hit = Physics2D.Raycast(playerPos, mouseDir, 10f, grappleable);
        return hit;
    }
    public void SetGrappleState(GrapplingState state)
    {
        if (state == GrapplingState.pulling)
        {
            _state = GrapplingState.pulling;
        }
        else if(state == GrapplingState.searching)
        {
            _state = GrapplingState.searching;
        }
        else
        {
            _state = GrapplingState.unattached;
        }
    }
}
