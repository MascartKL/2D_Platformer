using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rigibody;
    private float speed = 4;
    private float jumpForce = 10;
    private float moveInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    public Transform feetPos;
    private float checkRadius = 0.3f;
    public LayerMask whatisGround;
    public Joystick joystick;
    
    void Start()
    {
        rigibody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR_WIN
        moveInput = Input.GetAxis("Horizontal");
#endif

#if UNITY_ANDROID
        moveInput = joystick.Horizontal;
#endif

        rigibody.velocity = new Vector2(moveInput * speed, rigibody.velocity.y);

        if (isFacingRight == false && moveInput > 0)
        {
            Flip();
        }else if(isFacingRight == true && moveInput < 0)
        {
            Flip();
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
#if UNITY_EDITOR_WIN
        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            rigibody.velocity = Vector2.up * jumpForce;
        }
#endif
    }
    

    public void OnJumpButtonDown()
    {
#if UNITY_ANDROID
        if (isGrounded == true)
        {
            rigibody.velocity = Vector2.up * jumpForce;
        }
#endif
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
