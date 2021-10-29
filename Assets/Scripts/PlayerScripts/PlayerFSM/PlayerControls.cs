using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpUpState JumpUpState { get; private set; }
    public PlayerJumpSustainState JumpSustainState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerGrappleStartState GrappleState { get; private set; }
    

    #endregion

    #region Components
    public Animator Anim { get; private set; }
    [SerializeField] private PlayerData playerData;
    Rigidbody2D rb;
    public PlayerSceneManagement psm;
    //Slider staminaSlider;
    #endregion

    #region Other Variables
    public Vector2 MoveInput { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;
    public int DirectionFacing { get; private set; }
    public Transform groundCheckL { get; private set; }
    public Transform groundCheckR { get; private set; }
    //public float stamina { get; private set; }
    public LayerMask groundLayer;
    public Vector2 lastAfterImage;
    public bool canRechargeStamina { get; private set; }
    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        #region Create instances of states
        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpUpState = new PlayerJumpUpState(this, StateMachine, playerData, "jump up");
        JumpSustainState = new PlayerJumpSustainState(this, StateMachine, playerData, "jump sustain");
        FallState = new PlayerFallState(this, StateMachine, playerData, "fall");
        GrappleState = new PlayerGrappleStartState(this, StateMachine, playerData, "grapple");
        #endregion
    }
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        groundCheckL = GameObject.Find("GroundCheckL").transform;
        groundCheckR = GameObject.Find("GroundCheckR").transform;
        StateMachine.Initialize(IdleState);
        DirectionFacing = 1;
        psm = GetComponent<PlayerSceneManagement>();
        //staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        //staminaSlider.value = 100;
        //canRechargeStamina = true;
    }

    void Update()
    {
        /*if (CheckIfGrounded() && canRechargeStamina && stamina < playerData.maxStamina)
        {
            stamina += playerData.staminaRechargeRate;
        }*/
        //staminaSlider.value = Mathf.Lerp(staminaSlider.value, stamina, 0.95f);
        CurrentVelocity = rb.velocity;
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        StateMachine.CurrentState.LogicUpdate();        
    }
    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        /*if (!isDashing)
        {
            if (CurrentVelocity.y < playerData.maxFallSpeed)
            {
                SetVelocityY(playerData.maxFallSpeed);
            }
        }*/
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Instakill"))
        {
            psm.ChangeScene(psm.GetCurrentScene());
        }
        if(StateMachine.CurrentState == GrappleState)
        {
            StateMachine.ChangeState(JumpSustainState);
        }
    }
    #endregion

    #region Set Functions
    public void LerpVelocityX(float desiredVelocity, float lerpPercent, bool shouldClamp)
    {
        workspace = this.rb.velocity;
        workspace.Set(Mathf.Lerp(workspace.x, desiredVelocity, lerpPercent), CurrentVelocity.y);
        if (shouldClamp)
        {
            workspace.x = (workspace.x > -0.25f && workspace.x < 0.25f) ? 0 : workspace.x;
        }

        this.rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void LerpVelocityY(float desiredVelocity, float lerpPercent, bool shouldClamp)
    {
        workspace = this.rb.velocity;
        workspace.Set(CurrentVelocity.x, Mathf.Lerp(workspace.y, desiredVelocity, lerpPercent));
        if (shouldClamp)
        {
            workspace.y = (workspace.y > -0.25f && workspace.y < 0.25f) ? 0 : workspace.y;
        }

        this.rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityX(float desiredVelocity)
    {
        workspace = this.rb.velocity;
        workspace.Set(desiredVelocity, CurrentVelocity.y);
        this.rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetVelocityY(float desiredVelocity)
    {
        workspace = this.rb.velocity;
        workspace.Set(CurrentVelocity.x, desiredVelocity);
        this.rb.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public void SetGravity(float gravity)
    {
        rb.gravityScale = gravity;
    }
    /*public void SetStamina(float stam)
    {
        if (stam >= 0)
        {
            stamina = stam;
        }
        else
        {
            stamina = 0;
        }
    }
    public void SetStaminaRecharge(bool stam)
    {
        canRechargeStamina = stam;
    }*/
    #endregion

    #region Check Functions
    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput!= DirectionFacing)
        {
            Flip();
        }
    }
    public bool CheckIfGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheckL.transform.position, 0.1f, groundLayer) || 
            Physics2D.OverlapCircle(groundCheckR.transform.position, 0.1f, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public GameObject FindObjectStandingOn()
    {
        if (CheckIfGrounded())
        {
            RaycastHit2D leftHit = Physics2D.Raycast(groundCheckL.transform.position, Vector2.down);
            if(leftHit.collider != null)
            {
                //Debug.Log(leftHit.collider.gameObject);
                return leftHit.collider.gameObject;
            }
            else
            {
                RaycastHit2D rightHit = Physics2D.Raycast(groundCheckR.transform.position, Vector2.down);
                //Debug.Log(rightHit.collider.gameObject);
                return rightHit.collider.gameObject;
            }
        }
        else
        {
            //Debug.LogWarning("Not standing on object");
            return null;
        }
    }
    public Vector3 GetMouseDirection()
    {
        Camera cam = Camera.main;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDir = mousePos - this.transform.position;
        return mouseDir.normalized;
    }
    public Vector2 GetVelocityDirection()
    {
        Vector2 tempVector;
        if(CurrentVelocity.x > 0.05)
        {
            tempVector.x = 1;
        }
        else if(CurrentVelocity.x < -0.05)
        {
            tempVector.x = -1;
        }
        else
        {
            tempVector.x = 0;
        }

        if (CurrentVelocity.y > 0.05)
        {
            tempVector.y = 1;
        }
        else if (CurrentVelocity.y < -0.05)
        {
            tempVector.y = -1;
        }
        else
        {
            tempVector.y = 0;
        }
        return tempVector;
    }
    #endregion

    #region Other Functions
    private void Flip()
    {
        DirectionFacing *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    #endregion
}
