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
    public enum CamType { still, verticallyBound, horizontallyBound, freeCamera}
    public CamType CameraType = CamType.still;
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        playerControls = target.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
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
        if (CameraType != CamType.still)
        {
            SetBounds();
        }
    }

    public IEnumerator camShake(float horizontal, float vertical)
    {
        Vector3 original = transform.position;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (2 * horizontal * camShakeDistance),
            this.gameObject.transform.position.y + (vertical * camShakeDistance),
            this.gameObject.transform.position.z);
        yield return new WaitForSeconds(camShakeTime);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - (2 * horizontal * camShakeDistance),
            this.gameObject.transform.position.y - (vertical * camShakeDistance),
            this.gameObject.transform.position.z);
        yield return new WaitForSeconds(camShakeTime);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (0.5f * horizontal * camShakeDistance),
            this.gameObject.transform.position.y + (vertical * camShakeDistance),
            this.gameObject.transform.position.z);
        yield return new WaitForSeconds(camShakeTime);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - (0.5f * horizontal * camShakeDistance),
            this.gameObject.transform.position.y - (0.5f * vertical * camShakeDistance),
            this.gameObject.transform.position.z);
        transform.position = original;
    }

    void SetBounds()
    {
        if (SceneManager.GetActiveScene().name == "CaveEntrance")
        {
            boundPos.Set(
                Mathf.Clamp(this.gameObject.transform.position.x, 0f, 6.84f),
                Mathf.Clamp(this.gameObject.transform.position.y, -31.5f, 40),
                this.gameObject.transform.position.z);
        }
        if (SceneManager.GetActiveScene().name == "Cave1")
        {
            boundPos.Set(
                Mathf.Clamp(this.gameObject.transform.position.x, 41.59f, 1000f),
                Mathf.Clamp(this.gameObject.transform.position.y, 2.75f, 1000f),
                //this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
        }
        if (SceneManager.GetActiveScene().name == "Cave2")
        {
            boundPos.Set(
                Mathf.Clamp(this.gameObject.transform.position.x, 0f, 74.3f),
                Mathf.Clamp(this.gameObject.transform.position.y, -0.5f, 0f),
                //this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
        }
        if(SceneManager.GetActiveScene().name == "CaveRoom2")
        {
            boundPos.Set(
                Mathf.Clamp(this.gameObject.transform.position.x, 0f, 100),
                Mathf.Clamp(this.gameObject.transform.position.y, -0.5f, 100),
                this.gameObject.transform.position.z);
        }

        this.gameObject.transform.position = boundPos;
    }
}
