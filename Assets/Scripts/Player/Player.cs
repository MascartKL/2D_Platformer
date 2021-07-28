using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D rigidbody;
    static private float hpPlayer = 100;
    private float speed = 4;
    private float jumpForce = 10;
    private float moveInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    public Transform feetPos;
    private float checkRadius = 0.3f;
    public LayerMask whatisGround;
    public Joystick joystick;

    public Transform attackPos;
    public float attackDamage;
    public float attackRange;
    public LayerMask damageableLayer;

    public Image hpBarPlayer;
    public Image hpBarPlayerEffect;
    private float hpSpeed = 0.002f;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.freezeRotation = true;
    }

    private void Attack()
    {
        Debug.Log("attack");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, damageableLayer);

        if (enemies.Length != 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<DamageableObj>().TakeDamage(attackDamage);
            }
        }
    }
   
    public void PlayerDamaged(float damage)
    {
        hpPlayer -= damage;
        hpBarPlayer.fillAmount = hpPlayer * 0.01f;
        Debug.Log(hpPlayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR_WIN
        moveInput = Input.GetAxis("Horizontal");
#endif

#if UNITY_ANDROID
        moveInput = joystick.Horizontal;
#endif

        rigidbody.velocity = new Vector2(moveInput * speed, rigidbody.velocity.y);

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
            rigidbody.velocity = Vector2.up * jumpForce;
        }
#endif

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }

        if (hpBarPlayer.fillAmount < hpBarPlayerEffect.fillAmount)
            hpBarPlayerEffect.fillAmount -= hpSpeed;
        else
            hpBarPlayerEffect.fillAmount = hpBarPlayer.fillAmount;
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
