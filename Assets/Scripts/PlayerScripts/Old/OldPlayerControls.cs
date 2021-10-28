using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OldPlayerControls : MonoBehaviour
{
    /*
    public PlayerStateMachine StateMachine { get; private set;}

    public PlayerIdleState IdleState { get; private set;}
    public PlayerMoveState MoveState { get; private set;}
    [SerializeField] private PlayerData playerData;

    public Vector2 MoveInput { get; private set; }
    Rigidbody2D rb;
    public float maxSpeed = 4;
    public float jumpForce = 3;
    private bool isGrounded;
    private bool isJumping = false;
    public Animator animator { get; private set; }
    [SerializeField] Transform groundCheckL;
    [SerializeField] Transform groundCheckR;
    [SerializeField] float groundCheckRadius = 3f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float jumpsSinceCollision = 0;
    private float directionFacing = 0;
    bool groundCheckEnabled = true;
    Transform leftCheck;
    Transform rightCheck;
    float wallSlideTimer = 0f;

    [SerializeField] float dashTime;
    [SerializeField] float dashForce = 3;
    public bool isDashing;
    private bool canMove = true;
    //private float dashTimeLeft;
    private float horizontalDirection = 0;
    private float verticalDirection = 0;
    private float currentDashes = 0;
    private bool canDash = true;
    public float maxDashes = 1;
    [SerializeField] float dashSlowLengthPercentage = 0.2f;
    [SerializeField] float distanceBetweenAfterImages = 0.05f;
    private float lastImageXPos;
    private float lastImageYPos;

    [SerializeField] bool isImmune = false;
    
    public float immuneFlickerTime = 0.2f;
    public Color immuneColor1;
    public Color immuneColor2;
    ParticleSystem particle;
    
    Renderer playerRender;
    public float Xvelocity;
    public float Yvelocity;
    

    [SerializeField] float maxXSpeed = 4;
    [SerializeField] float maxYSpeed = 4;
    [SerializeField] float dashMaxSpeedIncrease = 1;
    [SerializeField] float dashDenom = 15f;

    GameObject mainCamera;
    CameraMovement cameraMovement;


    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsFacingRight", true);
        playerRender = GetComponent<Renderer>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraMovement = mainCamera.GetComponent<CameraMovement>();
        particle = GetComponentInChildren<ParticleSystem>();
        leftCheck = GameObject.FindGameObjectWithTag("LeftCheck").transform;
        rightCheck = GameObject.FindGameObjectWithTag("RightCheck").transform;

        StateMachine.Initialize(IdleState);
    }

    void Update()
    {
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxXSpeed, maxXSpeed), Mathf.Clamp(rb.velocity.y, -maxYSpeed, maxYSpeed));
        SetConditionsForScene();
        CheckForSceneChange();
        SetDirections();
        /*
        if (canMove)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * maxSpeed, rb.velocity.y);
        }
        if ((Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.Space))&& jumpsSinceCollision <1 && canMove && !isJumping)
        {
            StopCoroutine(WallSlide());
            animator.SetBool("IsJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsSinceCollision = 1;
            isJumping = true;
            StartCoroutine(groundCheckEnabler());
        }
        
        ManageGroundChecks();
        ManageDashes();
        CheckIfSliding();
    }

    void CheckIfSliding()
    {
        if(Physics2D.OverlapCircle(rightCheck.position, groundCheckRadius, groundLayer) && !Physics2D.OverlapCircle(groundCheckL.position, groundCheckRadius, groundLayer))
        {
            if (horizontalDirection == 1)
            {
                //WallSlide("Right");
                StartCoroutine(WallSlide());
                Debug.Log("Sliding right");
            }
        }
        if (Physics2D.OverlapCircle(leftCheck.position, groundCheckRadius, groundLayer) && !Physics2D.OverlapCircle(groundCheckR.position, groundCheckRadius, groundLayer))
        {
            if (horizontalDirection == -1)
            {
                //WallSlide("Left");
                StartCoroutine(WallSlide());
                Debug.Log("Sliding left");
            }
        }
    }
    IEnumerator WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0.5f);
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(1f);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    void WallSlide(string wallSliding)
    {
        //Just the code for the sliding, make jumping a different command.
        wallSlideTimer += Time.deltaTime;
        if(wallSlideTimer < 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            Debug.Log("Wall slide timer past 0");
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
        }
    }

    void ManageGroundChecks()
    {
        if (groundCheckEnabled && (Physics2D.OverlapCircle(groundCheckL.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheckR.position, groundCheckRadius, groundLayer)))
        {
            isGrounded = true;
            jumpsSinceCollision = 0;
            currentDashes = 0;
            animator.SetBool("IsJumping", false);
            isJumping = false;
            wallSlideTimer = 0;
        }
        else
        {
            isGrounded = false;
            isJumping = true;
        }
    }

    void ManageDashes()
    {
        if (Input.GetKeyDown(KeyCode.E) && canMove && canDash)
        {
            Debug.Log("Start dash");
            float tempHorizontal = horizontalDirection;
            float tempVertical = verticalDirection;
            if (!(tempHorizontal == 0 && tempVertical == -1 && isGrounded))
            {
                if (tempHorizontal == 0 && tempVertical == 0)
                {
                    tempHorizontal = directionFacing;
                }
                StartCoroutine(NewDash(tempHorizontal, tempVertical, dashTime));
                StartCoroutine(cameraMovement.camShake(tempHorizontal, tempVertical));
            }
            //dashTimeLeft = dashTime;
            currentDashes += 1;
        }
        if (currentDashes >= maxDashes)
        {
            canDash = false;
        }
        else
        {
            canDash = true;
        }
    }
    IEnumerator NewDash(float horizontal, float vertical, float dashTime)
    {
        isDashing = true;
        canMove = false;
        float buildUp = dashTime / dashDenom;
        float sustain = dashTime - (dashSlowLengthPercentage * dashTime);
        float dashTimeLeft = 0;
        float tempMaxX = maxXSpeed;
        float tempMaxY = maxYSpeed;
        maxYSpeed = 4;
        maxXSpeed += dashMaxSpeedIncrease;
        maxYSpeed += dashMaxSpeedIncrease;
        float startXTransform = this.transform.position.x;
        float startYTransform = this.transform.position.y + 0.01f;
        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
        lastImageYPos = transform.position.y;

        particle.Play();

        while (dashTimeLeft < dashTime)
        {
            
            if (dashTimeLeft < buildUp)
            {
                Debug.Log("Build dash");
                rb.velocity = new Vector2(0.5f * Mathf.Pow(dashTimeLeft, 2) * horizontal + rb.velocity.x, 0.5f * Mathf.Pow(dashTimeLeft, 2) * vertical + rb.velocity.y);
            }
            else if (dashTimeLeft > buildUp)
            {
                Debug.Log("Sustain dash");
                rb.velocity = new Vector2(dashForce * horizontal, dashForce * vertical);
            }
            /*else
            {
                //Shift graph of x^4 some amount to the right. (Add the conditional sustain > dashTimeLeft to above else if statement to reimplement this code
                Debug.Log("Slow dash");
                rb.velocity = new Vector2(0.5f * Mathf.Pow(dashTimeLeft - sustain + 1, 3) * horizontal + rb.velocity.x, Mathf.Pow(dashTimeLeft - sustain + 1, 3) * vertical + rb.velocity.y);
            }
            if (vertical == 0)
            {
                if (Mathf.Abs(transform.position.x - lastImageXPos) > distanceBetweenAfterImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXPos = transform.position.x;
                    lastImageYPos = transform.position.y;
                }
                rb.velocity = new Vector2(rb.velocity.x, 0);
                this.transform.position = new Vector2(this.transform.position.x, startYTransform);
            }
            if (horizontal == 0)
            {
                if (Mathf.Abs(transform.position.y - lastImageYPos) > distanceBetweenAfterImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXPos = transform.position.x;
                    lastImageYPos = transform.position.y;

                }
                rb.velocity = new Vector2(0, rb.velocity.y);
                this.transform.position = new Vector2(startXTransform, this.transform.position.y);

            }

            if(Mathf.Abs(transform.position.x -lastImageXPos) >distanceBetweenAfterImages && Mathf.Abs(transform.position.y - lastImageYPos) > distanceBetweenAfterImages)
            {
                PlayerAfterImagePool.Instance.GetFromPool();
                lastImageXPos = transform.position.x;
                lastImageYPos = transform.position.y;
            }
            yield return new WaitForEndOfFrame();
            dashTimeLeft += Time.deltaTime;
        }
        if (dashTimeLeft >= dashTime)
        {
            Debug.Log("End dash");
            maxXSpeed = tempMaxX;
            maxYSpeed = tempMaxY;
            isDashing = false;
            canMove = true;
            yield break;
            //End dash
        }

    }


    /*void Dash(float horizontal, float vertical)
    {
            if (dashTimeLeft > 0)
            {
                isDashing = true;
                canMove = false;
                rb.velocity = new Vector2(dashForce*horizontal, dashForce*vertical);
                dashTimeLeft -= Time.deltaTime;
                if (vertical == 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                }
        }
            else
            {
                isDashing = false;
                canMove = true;
                if (vertical == 1)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 3);
                }
        }
    
    void SetDirections()
    {
        horizontalDirection = Input.GetAxisRaw("Horizontal");
        verticalDirection = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat("Direction", Input.GetAxis("Horizontal"));
        animator.SetFloat("VerticalVelocity", rb.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                animator.SetBool("IsFacingRight", true);
                directionFacing = 1;
            } else {
                directionFacing = -1;
                animator.SetBool("IsFacingRight", false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (isDashing)
            {
                Destroy(collision.gameObject);
            }
            else{
                if (!isImmune)
                {
                    Debug.Log("Ouch!");
                    StartCoroutine(Immunity(1.5f));
                }
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Add collision to end dash early
        if (collision.gameObject.CompareTag("Instakill"))
        {
            Die();
        }
    }

    IEnumerator groundCheckEnabler()
    {
        groundCheckEnabled = false;
        yield return new WaitForSeconds(0.1f);
        groundCheckEnabled = true;
    }

    IEnumerator Immunity(float time)
    {
        isImmune = true;
        float tempTime = 0f;
        while (tempTime < time)
        {
            playerRender.material.SetColor("_Color", immuneColor1);
            yield return new WaitForSeconds(immuneFlickerTime);
            playerRender.material.SetColor("_Color", immuneColor2);
            yield return new WaitForSeconds(immuneFlickerTime);
            tempTime += immuneFlickerTime * 2;
        }
        playerRender.material.SetColor("_Color", Color.white);
        isImmune = false;
    }

    void CheckForSceneChange()
    {
        if(SceneManager.GetActiveScene().name == "CaveEntrance")
        {
            if (gameObject.transform.position.x >= 12)
            {
                SceneManager.LoadSceneAsync("Cave1");
            }
        }
    }

    void SetConditionsForScene()
    {
        if (SceneManager.GetActiveScene().name == "CaveEntrance")
        {
            canDash = false;
        }
    }

    void Die()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadSceneAsync(currentScene);
    }
    */
}