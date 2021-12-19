using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCameraData", menuName = "Data/Camera Data")]
public class CameraData : ScriptableObject
{
    public bool camShake = false;
    public float camSize = 5;
}
