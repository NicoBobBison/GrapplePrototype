using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

    public class PlayerName : MonoBehaviour
    {
        private string input;
        public InputField mainInputField;
 
        void Start()
        {
         
 		if (PlayerPrefs.GetString ("PlayerName")=="") {
 			PlayerPrefs.SetString ("LEACH", input);
 			}
            mainInputField.text = PlayerPrefs.GetString ("PlayerName");
             
 
        }
 
        public void SavePlayerName(string name)
        {
        	input = name;
        	Debug.Log(input);
         	Debug.Log("Saving Player Name");
       PlayerPrefs.SetString ("PlayerName", input);
        Debug.Log(PlayerPrefs.GetString ("PlayerName"));
 
        }
 }
 
