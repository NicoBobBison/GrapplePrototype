using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    string scene;
    Vector2 position;
    public static List<Coin> collectedCoins = new List<Coin>();
    private void Start()
    {
        scene = SceneManagement.instance.GetScene().name;
        position = transform.position;
        if (collectedCoins.Contains(this))
        {
            Destroy(this.gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
            CoinCounter.CollectCoin();
            collectedCoins.Add(this);
            Destroy(gameObject);
        }
    }
    public override bool Equals(object other)
    {
        Coin coin = other as Coin;
        return scene == SceneManagement.instance.GetScene().name && position == coin.position;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
