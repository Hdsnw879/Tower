using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    Camera cam;
    public float senX,senY;
    float xRotation , yRotation;
    Rigidbody rb;
    public float speed;
    bool isLock;
    Transform camPosition;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camPosition = transform.Find("Cube");
        cam = GameObject.Find("CameraPlayer").GetComponent<Camera>();
        isLock = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SpeedLimit();
        FreeCursor();
        cam.transform.position = camPosition.transform.position;
        if(isLock)
        {
        Sensitive();
        PlayerMove();
        }
        
        
    }
    void FreeCursor()
    {
        
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isLock = false;
        }else if(!isLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isLock = true;
        }
    }
    void Sensitive()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90f,90f);
        cam.transform.rotation = Quaternion.Euler(xRotation,yRotation ,0);
        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
    }
    void PlayerMove()
    {
        
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        Vector3 direction = transform.forward * vertical + horizontal * transform.right;
        rb.AddForce(direction.normalized * speed * 10,ForceMode.Force);
    }
    void SpeedLimit()
    {
        Vector3 currentSpeed = new Vector3(rb.velocity.x,0,rb.velocity.z);
        if(currentSpeed.magnitude > speed)
        {
            Vector3 limitSpeed = currentSpeed.normalized * speed;
            rb.velocity = new Vector3(limitSpeed.x,rb.velocity.y,limitSpeed.z);
        }
    }
}
