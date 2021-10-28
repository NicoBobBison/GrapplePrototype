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
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        playerControls = target.GetComponent<PlayerControls>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPos = this.transform.position + offset;
        Vector3 desiredPos = Vector3.Lerp(currentPos, target.transform.position, lerpPercent);
        this.transform.position = desiredPos;
    }
    private void LateUpdate()
    {    
        SetBounds();
    }

    public IEnumerator camShake(float horizontal, float vertical)
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + (horizontal * camShakeDistance), this.gameObject.transform.position.y + (vertical * camShakeDistance), this.gameObject.transform.position.z);
        yield return new WaitForSeconds(camShakeTime);
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x - (horizontal * camShakeDistance), this.gameObject.transform.position.y - (vertical * camShakeDistance), this.gameObject.transform.position.z);
    }

    private void SetBounds()
    {
        
        if (SceneManager.GetActiveScene().name == "CaveEntrance")
        {
            boundPos.Set(
                Mathf.Clamp(this.gameObject.transform.position.x, 0f, 6.84f),
                Mathf.Clamp(this.gameObject.transform.position.y, -31.5f, 40),
                this.gameObject.transform.position.z);
        }
        /*if (SceneManager.GetActiveScene().name == "Cave1")
        {
            boundPos.Set(
                14.14f,
                //this.gameObject.transform.position.x,
                //Mathf.Clamp(this.gameObject.transform.position.y, 0.5f, 1.2f),
                2.01f,
                //this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
        }*/
        if(SceneManager.GetActiveScene().name == "Cave2")
        {
            boundPos.Set(
                Mathf.Clamp(this.gameObject.transform.position.x, 0f, 74.3f),
                Mathf.Clamp(this.gameObject.transform.position.y, -0.5f, 0f),
                //this.gameObject.transform.position.y,
                this.gameObject.transform.position.z);
        }

        //this.gameObject.transform.position = boundPos;
    }
}
