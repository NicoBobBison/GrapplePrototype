using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    TextMeshProUGUI text;
    static float timeSinceLastCoin;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.SetText(PlayerPrefs.GetInt("coins").ToString());
        timeSinceLastCoin += Time.deltaTime;
        if(timeSinceLastCoin > 2)
        {
            Color temp = text.color;
            temp.a -= Time.deltaTime;
            text.color = temp;
        }
        else
        {
            if(text.color.a < 1)
            {
                Color temp = text.color;
                // Multiply deltaTime by speed to increase transparency value by
                temp.a += 10 * Time.deltaTime;
                text.color = temp;
            }
        }
    }
    public static void CollectCoin()
    {
        timeSinceLastCoin = 0;
    }
}
