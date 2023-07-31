using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;
    public bool isJumping = false;
    

    CharacterController cc;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    float gravity = -9.8f;
    float yVelocity = 0;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);

        dir = Camera.main.transform.TransformDirection(dir);


        yVelocity += gravity * Time.deltaTime / 1.3f;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 20;
        else
            moveSpeed = 11;

        if (cc.collisionFlags == CollisionFlags.Below)
        {
            yVelocity = 0;
            if(isJumping)
                isJumping = false;
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }
    }

    
}