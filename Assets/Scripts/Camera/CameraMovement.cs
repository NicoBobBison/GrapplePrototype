using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float lerpPercent = 0.9f;
    Vector3 offset = new Vector3(0f, 0f, -10f);
    PlayerControls playerControls;
    [SerializeField] float camShakeDistance = 0.1f;
    [SerializeField] float camShakeTime = 0.05f;
    private Vector3 boundPos;
    [SerializeField] Vector2 lowerLeftBound;
    [SerializeField] Vector2 upperRightBound;
    [SerializeField] CameraData cameraData;
    public enum CamType { still, vertical, horizontal, freeCamera}
    public CamType CameraType = CamType.still;
    Vector2 startPos;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        playerControls = target.GetComponent<PlayerControls>();
        startPos.Set(transform.position.x, transform.position.y);
        Camera.main.orthographicSize = cameraData.camSize;
    }

    void FixedUpdate()
    {
        if (CameraType != CamType.still)
        {
            Vector3 currentPos = this.transform.position + offset;
            Vector3 desiredPos = Vector3.Lerp(currentPos, target.transform.position, lerpPercent);
            desiredPos.z = -10;
            transform.position = desiredPos;
        }
    }
    void LateUpdate()
    {
        SetBounds();
    }
    /*
    public IEnumerator camShake(float horizontal, float vertical)
    {
        if (cameraData.camShake)
        {
            Vector3 originalPos = transform.position;
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (2 * horizontal * camShakeDistance),
            this.gameObject.transform.position.y + (vertical * camShakeDistance),
            this.gameObject.transform.position.z);
            yield return new WaitForSeconds(camShakeTime);
            this.gameObject.transform.position = originalPos;
            yield return new WaitForSeconds(camShakeTime);
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (0.5f * horizontal * camShakeDistance),
                this.gameObject.transform.position.y + (vertical * camShakeDistance),
                this.gameObject.transform.position.z);
            yield return new WaitForSeconds(camShakeTime);
            this.gameObject.transform.position = originalPos;
        }
    }*/

    void SetBounds()
    {
        if(CameraType == CamType.freeCamera)
        {
            boundPos.Set(Mathf.Clamp(gameObject.transform.position.x, lowerLeftBound.x, upperRightBound.x),
            Mathf.Clamp(gameObject.transform.position.y, lowerLeftBound.y, upperRightBound.y), -10);
        }
        else if(CameraType == CamType.horizontal)
        {
            boundPos.Set(Mathf.Clamp(gameObject.transform.position.x, lowerLeftBound.x, upperRightBound.x),
            startPos.y, -10);
        }
        else if(CameraType == CamType.vertical)
        {
            boundPos.Set(startPos.x, Mathf.Clamp(gameObject.transform.position.y, lowerLeftBound.y, upperRightBound.y), -10);
        }
        else
        {
            boundPos.Set(transform.position.x, transform.position.y, -10);
        }
        gameObject.transform.position = boundPos;
    }
}
