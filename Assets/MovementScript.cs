using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 1f;

    float verticalLookRotation = 0f;

    Rigidbody rb;
    Transform cam;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        //Movement
        Vector3 vel = Vector3.zero;
        vel += transform.forward * Input.GetAxis("Vertical");
        vel += transform.right * Input.GetAxis("Horizontal");

        rb.velocity = vel.normalized * speed * (Input.GetKey(KeyCode.LeftShift) ? 2 : 1);
    }
}
