using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

    public float speed = 1;
    public GameObject stick;

    private Rigidbody rb;
    private bool isAccelerometer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isAccelerometer = SystemInfo.supportsAccelerometer;
        if (isAccelerometer)
            stick.SetActive(false);
    }

    void FixedUpdate()
    {
        float moveHorizontal;
        float moveVertical;

        if (isAccelerometer)
        {
            moveHorizontal = Input.acceleration.x;
            moveVertical = Input.acceleration.y;

        }
        else
        {
            moveHorizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            moveVertical = CrossPlatformInputManager.GetAxis("Vertical");
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
    }
}
