using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePowerUp : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.GetInt("unlockedGrapple") == 1)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("unlockedGrapple", 1);
            Destroy(this.gameObject);
        }
    }
}
