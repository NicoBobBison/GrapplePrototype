using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularPlatforms : MonoBehaviour
{
    List<Transform> playerGroundChecks = new List<Transform>();
    GameObject player;
    private bool shouldIgnoreCollisions;
    // Start is called before the first frame update
    void Start()
    {
        /*
        GameObject[] checks = GameObject.FindGameObjectsWithTag("PlayerGroundChecks");
        foreach (GameObject go in checks)
        {
            playerGroundChecks.Add(go.transform);
        }
        player = GameObject.FindGameObjectWithTag("Player");
        */
    }

    // Update is called once per frame
    private void Update()
    {
        /*Just use the platform effector in the editor
         * 
         * int groundCheckCounter = 0;
        foreach (Transform tr in playerGroundChecks)
        {
            if(player.transform.position.y < this.transform.position.y)
            {
                groundCheckCounter++;
            }
        }
        if(groundCheckCounter != 0)
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
        }
        else
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
        }*/
    }

}
