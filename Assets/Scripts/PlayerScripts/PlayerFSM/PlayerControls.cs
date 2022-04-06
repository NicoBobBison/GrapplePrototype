using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public PlayerGrapplePullState PullState { get; private set; }
    public PlayerSceneTransState SceneTransState { get; private set; }
    public PlayerGrappleSlowdownState GrappleSlowdownState { get; private set; }
    public PlayerGrappleAirState GrappleAirState { get; private set; }

    #endregion

    #region Components
    public Animator Anim { get; private set; }
    [SerializeField] public PlayerData playerData;
    Rigidbody2D rb;
    public SceneManagement psm;
    public ParticleSystem CollideDuringGrapplePS { get; private set; }
    public CameraMovement camMovement;
    public CameraEffects cameraEffects;
    //Slider staminaSlider;
    #endregion

    #region Other Variables
    public Vector2 MoveInput { get; private set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;
    public int DirectionFacing;
    public Transform groundCheckL { get; private set; }
    public Transform groundCheckR { get; private set; }
    //public float stamina { get; private set; }
    public LayerMask groundLayer;
    public Vector2 lastAfterImage;
    public bool canRechargeStamina { get; private set; }
    public bool slowingFromGrapple = false;
    PlayerGrapple playerGrapple;
    [SerializeField] PhysicsMaterial2D slippery;
    [SerializeField] PhysicsMaterial2D sticky;
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
        PullState = new PlayerGrapplePullState(this, StateMachine, playerData, "pull");
        SceneTransState = new PlayerSceneTransState(this, StateMachine, playerData, "scene trans");
        GrappleSlowdownState = new PlayerGrappleSlowdownState(this, StateMachine, playerData, "grapple slowdown");
        GrappleAirState = new PlayerGrappleAirState(this, StateMachine, playerData, "grapple air");
        #endregion
    }
    void Start()
    {
        cameraEffects = Camera.main.gameObject.GetComponent<CameraEffects>();
        GameObject spawn = SceneManagement.instance.FindSpawnPoint();
        if (spawn != null)
        {
            transform.position = SceneManagement.instance.FindSpawnPoint().transform.position;
            if (spawn.GetComponent<LevelExit>() != null && spawn.GetComponent<LevelExit>().directionToExit == LevelExit.Direction.right)
            {
                Flip();
                DirectionFacing = -1;
            }
            else
            {
                DirectionFacing = 1;
            }
        }
        else
        {
            DirectionFacing = 1;
        }
        camMovement = Camera.main.gameObject.GetComponent<CameraMovement>();
        CollideDuringGrapplePS = GetComponentInChildren<ParticleSystem>();
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        groundCheckL = GameObject.Find("GroundCheckL").transform;
        groundCheckR = GameObject.Find("GroundCheckR").transform;
        StateMachine.Initialize(IdleState);
        psm = GameObject.Find("SceneManager").GetComponent<SceneManagement>();
        playerGrapple = GameObject.Find("Grapple").GetComponent<PlayerGrapple>();
        //staminaSlider = GameObject.Find("Stamina").GetComponent<Slider>();
        //staminaSlider.value = 100;
        //canRechargeStamina = true;
    }

    void Update()
    {
        CurrentVelocity = rb.velocity;
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetButton("Jump"))
        {
            // Might not be optimized, lag?
            MoveInput = new Vector2(MoveInput.x, 1.0f);
        }
        StateMachine.CurrentState.LogicUpdate();
    }
    private void FixedUpdate()
    {
        // Check if on moving platform
        if (GetObjectStandingOn() != null && GetObjectStandingOn().layer == LayerMask.NameToLayer("MovingPlatform") && MoveInput.x == 0)
        {
            rb.sharedMaterial = sticky;
        }
        else
        {
            rb.sharedMaterial = slippery;
        }

        StateMachine.CurrentState.PhysicsUpdate();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Instakill"))
        {
            KillPlayer();
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

        rb.velocity = workspace;
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
        if (xInput != 0 && xInput != DirectionFacing)
        {
            Flip();
        }
    }
    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckL.transform.position, 0.1f, groundLayer) || Physics2D.OverlapCircle(groundCheckR.transform.position, 0.1f, groundLayer);
    }

    public GameObject GetObjectStandingOn()
    {
        if (CheckIfGrounded())
        {
            RaycastHit2D leftHit = Physics2D.Raycast(groundCheckL.transform.position, Vector2.down);
            if (leftHit.collider != null)
            {
                return leftHit.collider.gameObject;
            }
            else
            {
                RaycastHit2D rightHit = Physics2D.Raycast(groundCheckR.transform.position, Vector2.down);
                if(rightHit.collider != null)
                    return rightHit.collider.gameObject;
            }
        }
        //Debug.LogWarning("Not standing on object");
        return null;

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
        if (CurrentVelocity.x > 0.05)
        {
            tempVector.x = 1;
        }
        else if (CurrentVelocity.x < -0.05)
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
        if (!SceneManagement.gamePaused)
        {
            DirectionFacing *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
    public IEnumerator FullStop(float time, bool screenshake)
    {
        slowingFromGrapple = true;
        if (screenshake)
        {
            StartCoroutine(cameraEffects.camShake(GetMouseDirection().x, GetMouseDirection().y));
        }
        float timer = time;
        while(timer > 0)
        {
            SetVelocityX(0);
            SetVelocityY(0);
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        slowingFromGrapple = false;
    }
    public IEnumerator SlowToStop(float time, float multiplier, bool screenshake)
    {
        slowingFromGrapple = true;
        float timer = time;
        while (timer > 0)
        {
            SetVelocityX(Mathf.Lerp(CurrentVelocity.x, 0, multiplier));
            SetVelocityY(Mathf.Lerp(CurrentVelocity.y, 0, multiplier));
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        slowingFromGrapple = false;
        if (screenshake)
        {
            StartCoroutine(cameraEffects.camShake(GetMouseDirection().x, GetMouseDirection().y));
        }
    }
    public IEnumerator SlowToStop(float time, float multiplier, bool screenshake, bool endGrapple)
    {
        slowingFromGrapple = true;
        float timer = time;
        while (timer > 0)
        {
            SetVelocityX(Mathf.Lerp(CurrentVelocity.x, 0, multiplier));
            SetVelocityY(Mathf.Lerp(CurrentVelocity.y, 0, multiplier));
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        slowingFromGrapple = false;
        if (screenshake)
        {
            StartCoroutine(cameraEffects.camShake(GetMouseDirection().x, GetMouseDirection().y));
        }
        if (endGrapple)
        {
            playerGrapple.EndGrapple(JumpSustainState);
        }
    }
    public void AddForceTo(Vector2 direction, float amount)
    {
        Debug.Log("Direction: " + direction * amount);
        rb.AddForce(direction * amount, ForceMode2D.Impulse);
    }
    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }
    public void KillPlayer()
    {
        AudioManager.instance.Play("DeathSound");
        psm.ChangeScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}