using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerName : MonoBehaviour
{
    private string input;
    public InputField ifield = null;

    void Start()
    {
        if (PlayerPrefs.GetString("PlayerName") == null)
        {
            PlayerPrefs.SetString("LEACH", input);
        }
        // THIS DOES NOT WORK
        //  FIX IT!
        // ifield.text = PlayerPrefs.GetString("PlayerName"); 


    }

    public void SavePlayerName(string name)
    {
        input = name;
        PlayerPrefs.SetString("PlayerName", input);
        
    }
}

