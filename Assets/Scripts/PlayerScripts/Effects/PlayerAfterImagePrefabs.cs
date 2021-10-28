using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePrefabs : MonoBehaviour
{
    private float lifetime = 0.15f;
    private float startTime;
    private float alpha;
    private float alphaMultiplier = 0.99f;
    private float startingAlpha = 0.7f;
    private Transform playerTransform;
    private SpriteRenderer SR;
    private SpriteRenderer playerSR;
    private Color color;
    [SerializeField] float stallTime = 0.5f;
    Vector3 workspace;
    private float tempStall;

    private void OnEnable()
    {
        tempStall = stallTime;
        SR = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = playerTransform.GetComponent<SpriteRenderer>();
        alpha = startingAlpha;
        SR.sprite = playerSR.sprite;
        transform.position = playerTransform.transform.position;
        startTime = Time.time;
        PlayerControls pc = playerTransform.GetComponent<PlayerControls>();
        if(pc.DirectionFacing == -1)
        {
            workspace.Set(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        } else
        {
            workspace.Set(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        transform.eulerAngles = workspace;
    }

    private void Update()
    {
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;
        tempStall -= Time.deltaTime;
        if (tempStall < 0)
        {
            alpha *= alphaMultiplier;
            
            if (Time.time >= (startTime + lifetime))
            {
                PlayerAfterImagePool.Instance.AddToPool(gameObject);
            }
        }
    }
}
