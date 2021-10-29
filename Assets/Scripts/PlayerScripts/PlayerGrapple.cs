using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    LineRenderer lr;
    Vector3 player;
    Vector3 mousePos;
    Vector3 mouseDir;
    Vector3 grappleDir;
    Camera cam;
    [SerializeField]float currentLength = 0;
    public float grappleRetractSpeed = 0.05f;
    public Vector3 endpoint;
    PlayerControls playerControls;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        player = GameObject.Find("Player").transform.position;
        cam = Camera.main;
        playerControls = GameObject.Find("Player").GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.Find("Player").transform.position;
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = mousePos - player;
        mouseDir.z = 0;
        mouseDir = mouseDir.normalized;


        grappleDir = endpoint - player;
        grappleDir = grappleDir.normalized;

    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            player.z = 0;
            if(GrappleOnObject()){
                playerControls.StateMachine.ChangeState(playerControls.GrappleState);
            }
            else
            {
                currentLength += grappleRetractSpeed;
            }
        }
        else
        {
            if(currentLength > 0)
            {
                currentLength -= grappleRetractSpeed;
            }
        }
        endpoint = player + (mouseDir * currentLength);

        if (currentLength == 0)
        {
            //lr.enabled = false;
        }
        else
        {
            //lr.enabled = true;
        }
        currentLength = Mathf.Clamp(currentLength, 0, Vector2.Distance(player, mousePos));
        lr.SetPosition(0, player);
        lr.SetPosition(1, endpoint);
    }
   
    public bool GrappleOnObject()
    {
        return Physics2D.OverlapCircle(endpoint, 0.1f, playerControls.groundLayer);
    }
}
