using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour
{
    enum state {Idle, JumpPrep, Jump, JumpCooldown}
    state currentState = state.JumpCooldown;
    public GameObject player;
    public float trackingDistance = 10;
    public float jumpCooldown;
    Rigidbody2D rb;
    bool inCooldown = false;
    [SerializeField] float jumpForceX = 9;
    [SerializeField] float jumpForceY = 12;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = state.Idle;
        player = GameObject.FindWithTag("Player");
    }
    void Update()
    {
        if(Vector2.Distance(player.transform.position, this.transform.position) > trackingDistance && currentState!=state.Jump && currentState !=state.JumpCooldown)
        {
            currentState = state.Idle;
        } else if(currentState == state.Idle || currentState == state.JumpPrep)
        {
            Jump();
            currentState = state.JumpCooldown;
        }
        if(currentState == state.JumpCooldown && !inCooldown)
        {
            StartCoroutine(_jumpCooldown());
        }
    }

    private void Jump()
    {
        float playerX = player.transform.position.x;
        Vector2 left = new Vector2(-jumpForceX, jumpForceY);
        Vector2 right = new Vector2(jumpForceX, jumpForceY);
        if (playerX < this.transform.position.x)
        {
            rb.AddForce(left, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(right, ForceMode2D.Impulse);
        }
    }
    IEnumerator _jumpCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(jumpCooldown);
        currentState = state.JumpPrep;
        inCooldown = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.velocity = new Vector2 (0, 0);
        }   
    }
}
