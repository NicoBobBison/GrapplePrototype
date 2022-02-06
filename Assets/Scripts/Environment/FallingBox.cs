using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBox : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] bool shouldFall = false;
    [SerializeField] float fallSpeed = 2;
    PlayerControls player;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<PlayerControls>();
    }
    private void Update()
    {
        if(player.GetObjectStandingOn() != null)
        {
            if (player.GetObjectStandingOn().name == this.name)
            {
                StartCoroutine(Fall());
                Debug.Log("Start box fall");
            }
        }
        
    }
    private void FixedUpdate()
    {
        if (shouldFall)
        {
            transform.Translate(Vector2.down * fallSpeed);
        }
    }

    public IEnumerator Fall()
    {
        yield return new WaitForSeconds(1);
        shouldFall = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
}
