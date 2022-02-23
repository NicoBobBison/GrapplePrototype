using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    LineRenderer lr;
    Camera cam;
    GameObject player;
    public Vector2 playerPos;
    Vector2 mousePos;
    public Vector2 grapplePoint;
    public Vector2 grappleDir;
    PlayerControls pc;
    public LayerMask grappleable;
    public LayerMask grappleDontSlow;
    public Vector2 lastHitPoint { get; private set; }
    public GameObject lastHitObject { get; private set; }
    public Vector2 workspace;
    [SerializeField] float grappleLerpAmount = 0.5f;
    public enum GrapplingState { searching, pulling, unattached}
    public GrapplingState _state = GrapplingState.unattached;
    Color baseColor;
    Color warningColor = Color.red;

    private void Start()
    {
        cam = Camera.main;
        lr = GetComponent<LineRenderer>();
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerControls>();
        playerPos = player.transform.position;
        grapplePoint = playerPos;
        baseColor = GetComponent<LineRenderer>().startColor;
    }
    private void Update()
    {
        ChangeBasedOnState();
        MoveGrappleEnd();
        ManageGrappleColor();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        playerPos = player.transform.position;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !SceneManagement.gamePaused)
        {
            RaycastHit2D hit = CastInDirection(pc.MoveInput);
            if(hit.collider != null)
            {
                _state = GrapplingState.pulling;
                lastHitPoint = hit.point;
                lastHitObject = hit.collider.gameObject;
                grappleDir = pc.MoveInput;
                if(Vector2.Distance(lastHitPoint, playerPos) < 0.2f && lastHitObject.layer == LayerMask.NameToLayer("Chains"))
                {
                    Vector2 inputToChain = pc.MoveInput * pc.playerData.chainGrabDistance;
                    inputToChain.Normalize();
                    Debug.DrawLine(playerPos, playerPos + inputToChain, Color.red, 2, false);

                    if (Physics2D.OverlapCircle(playerPos + (pc.MoveInput * pc.playerData.chainGrabDistance), 0.01f))
                    {
                        // Might not work if the grappled chain is a different object than the one the player is on
                        lastHitPoint = playerPos + (pc.MoveInput * pc.playerData.chainGrabDistance);
                    }
                    else
                    {
                        lastHitPoint = playerPos + (pc.MoveInput * 0.5f);
                    }
                }
            }
            else
            {
                _state = GrapplingState.searching;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
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
                if (CastInDirection(pc.MoveInput).collider != null)
                {
                    lastHitPoint = CastInDirection(pc.MoveInput).point;
                    lastHitObject = CastInDirection(pc.MoveInput).collider.gameObject;
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
                if(lastHitObject.layer == LayerMask.NameToLayer("Pullable"))
                {
                    pc.StateMachine.ChangeState(pc.PullState);
                }
            }
            if(_state == GrapplingState.pulling && pc.StateMachine.CurrentState != pc.GrappleState)
            {
                if (pc.IsInLayerMask(lastHitObject, grappleable))
                {
                    pc.StateMachine.ChangeState(pc.GrappleState);
                    if(Vector2.Distance(grapplePoint, playerPos) > pc.playerData.grappleMaxDistance)
                    {
                        // Grapple air state doesn't work as intended
                        EndGrapple(pc.JumpSustainState);
                    }
                }
            }
        }
    }

    void MoveGrappleEnd()
    {
        if (_state == GrapplingState.pulling)
        {
            grapplePoint.Set(Vector2.Lerp(grapplePoint, lastHitPoint, grappleLerpAmount).x,
                Vector2.Lerp(grapplePoint, lastHitPoint, grappleLerpAmount).y);
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
        if(direction != Vector2.zero && Physics2D.OverlapCircle(hit.point, 0.05f, 1 << LayerMask.NameToLayer("Instakill")))
        {
            Debug.Log("Die");
            pc.KillPlayer();
        }
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
        SetGrappleState(GrapplingState.unattached);
    }
}
