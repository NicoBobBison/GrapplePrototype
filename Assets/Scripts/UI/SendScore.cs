using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SendScore : MonoBehaviour
{
    CameraEffects camEffects;
    TextMeshProUGUI fastestTime;
   
    public void SendTheScore()
    {
    	Debug.Log("SENDING SCORE...");
        HighScores.UploadScore(PlayerPrefs.GetString("PlayerName"), (int)PlayerPrefs.GetFloat("FastestTime"));
    }
}

