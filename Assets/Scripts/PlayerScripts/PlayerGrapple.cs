using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    LineRenderer lr;
    Camera cam;
    GameObject player;
    Vector2 playerPos;
    Vector2 mousePos;
    public Vector2 grapplePoint;
    public Vector2 grappleDir;
    PlayerControls pc;
    [SerializeField] LayerMask grappleable;
    public RaycastHit2D lastHit { get; private set; }
    public Vector2 workspace;
    [SerializeField] float grappleLerpAmount = 0.5f;
    public enum GrapplingState { searching, pulling, unattached}
    public GrapplingState _state = GrapplingState.unattached;
    Color baseColor = Color.gray;
    Color warningColor = Color.red;

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
        ManageGrappleColor();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerPos = player.transform.position;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit2D hit = CastToMouse();
            if(hit.collider != null)
            {
                _state = GrapplingState.pulling;
                lastHit = hit;
                //Debug.Log("Hit transform: " + hit.point);
                grappleDir = (hit.point - playerPos).normalized;
            }
            else
            {
                _state = GrapplingState.searching;
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
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
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            {
                lr.enabled = true;
                if (Physics2D.OverlapCircle(grapplePoint, 0.05f, grappleable) && CastToMouse().collider != null)
                {
                    lastHit = CastToMouse();
                    grappleDir = (lastHit.point - playerPos).normalized;
                    _state = GrapplingState.pulling;
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
                if (lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                    lastHit.collider.gameObject.layer == LayerMask.NameToLayer("GrapplePoint") ||
                    lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
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
            //Debug.Log("Mouse pos: " + mousePos);
            //Debug.Log("Player pos: " + playerPos);
            if(Vector2.Distance(playerPos, mousePos) > pc.playerData.grappleMaxDistance)
            {
                Vector2 v = mousePos - playerPos;
                grapplePoint.Set(Vector2.Lerp(grapplePoint, v.normalized * pc.playerData.grappleMaxDistance + playerPos, grappleLerpAmount).x,
                    Vector2.Lerp(grapplePoint, v.normalized * pc.playerData.grappleMaxDistance + playerPos, grappleLerpAmount).y);
            }
            else
            {
                grapplePoint.Set(Vector2.Lerp(grapplePoint, mousePos, grappleLerpAmount).x,
                Vector2.Lerp(grapplePoint, mousePos, grappleLerpAmount).y);
            }
        }
        else
        {
            grapplePoint.Set(Vector2.Lerp(grapplePoint, playerPos, grappleLerpAmount).x,
                Vector2.Lerp(grapplePoint, playerPos, grappleLerpAmount).y);
        }
        lr.SetPosition(0, playerPos);
        lr.SetPosition(1, grapplePoint);
    }
    void ManageGrappleColor()
    {
        if(Vector2.Distance(playerPos, grapplePoint) > (pc.playerData.grappleMaxDistance / 5) * 4)
        {
            lr.startColor = warningColor;
            lr.endColor = warningColor;
        }
        else
        {
            lr.startColor = baseColor;
            lr.endColor = baseColor;
        }
    }
    RaycastHit2D CastToMouse()
    {
        Vector2 mouseDir = (playerPos - mousePos) * -1;
        RaycastHit2D hit = Physics2D.Raycast(playerPos, mouseDir, pc.playerData.grappleMaxDistance, grappleable);
        return hit;
    }
    public void SetGrappleState(GrapplingState state)
    {
        _state = state;
    }
    public void SlowThenEndGrapple()
    {
        pc.StartCoroutine(pc.SlowToStop(0.25f, 0.02f, false, true));
        
    }
    public void EndGrapple()
    {
        pc.StateMachine.ChangeState(pc.JumpSustainState);
        pc.CollideDuringGrapplePS.Play();
        SetGrappleState(GrapplingState.unattached);
    }
}
