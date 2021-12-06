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
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            RaycastHit2D hit = CastInDirection(pc.MoveInput);
            if(hit.collider != null)
            {
                _state = GrapplingState.pulling;
                lastHit = hit;
                //Debug.Log("Hit transform: " + hit.point);
                grappleDir = pc.MoveInput;
            }
            else
            {
                _state = GrapplingState.searching;
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftShift))
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
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.LeftShift))
            {
                lr.enabled = true;
                if (Physics2D.OverlapCircle(grapplePoint, 0.05f, grappleable) && CastInDirection(pc.MoveInput).collider != null)
                {
                    lastHit = CastInDirection(pc.MoveInput);
                    grappleDir = pc.MoveInput.normalized;
                    _state = GrapplingState.pulling;
                    pc.StateMachine.ChangeState(pc.GrappleState);
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
                Debug.Log("Current state: " + pc.StateMachine.CurrentState);
                if (lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                    lastHit.collider.gameObject.layer == LayerMask.NameToLayer("GrapplePoint") ||
                    lastHit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
                {
                    pc.StateMachine.ChangeState(pc.GrappleState);
                    if(Vector2.Distance(grapplePoint, playerPos) > pc.playerData.grappleMaxDistance)
                    {
                        EndGrapple(pc.GrappleAirState);
                    }
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
            grapplePoint.Set(pc.MoveInput.x * pc.playerData.grappleMaxDistance + player.transform.position.x,
                pc.MoveInput.y * pc.playerData.grappleMaxDistance + player.transform.position.y);
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
    
    RaycastHit2D CastInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(playerPos, direction, pc.playerData.grappleMaxDistance, grappleable);
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
    public void EndGrapple(PlayerState nextState)
    {
        pc.StateMachine.ChangeState(pc.JumpSustainState);
        pc.CollideDuringGrapplePS.Play();
        SetGrappleState(GrapplingState.unattached);
    }
}
