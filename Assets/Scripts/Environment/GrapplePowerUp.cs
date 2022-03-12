using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePowerUp : MonoBehaviour
{
    [SerializeField] PlayerData data;
    private void Start()
    {
        if (data.unlockedGrapple)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            data.unlockedGrapple = true;
            Destroy(this.gameObject);
        }
    }
}
