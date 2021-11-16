using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    public enum Direction { up, right, down, left };
    public Direction directionToExit;
    public Transform player;
    CameraEffects effects;
    void Start()
    {
        player = GameObject.Find("Player").transform;
        effects = Camera.main.GetComponent<CameraEffects>();
    }

    void Update()
    {
        if (PastExit())
        {
            if (!effects.transitioning)
            {
                effects.PlaySceneTransition(gameObject.name);
            }
        }
    }

    bool PastExit()
    {
        if(directionToExit == Direction.up && player.position.y > transform.position.y + 0.5f)
        {
            return true;
        }
        if(directionToExit == Direction.down && player.position.y < transform.position.y - 0.5f)
        {
            return true;
        }
        if(directionToExit == Direction.right && player.position.x > transform.position.x + 0.5f)
        {
            return true;
        }
        if(directionToExit == Direction.left && player.position.x < transform.position.x - 0.5f)
        {
            return true;
        }
        return false;
    }
}
