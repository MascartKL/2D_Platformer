using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    private float moveInput;
    private bool isFacingRight = true;
    private bool isGrounded;
    public Transform feetPos;
    private float checkRadius = 0.3f; // радиус для проверки isGround
    public LayerMask whatisGround;
    public Joystick joystick;
    private bool isInvulnerability;

    [Header("Main Settings")]
    [SerializeField]
    private float jumpForce = 10;
    [SerializeField]
    private float hpPlayer = 100;
    [SerializeField]
    private float maxhpPlayer = 100;
    [SerializeField]
    private float speed = 4;

    [Header("Attack Settings")]
    public Transform attackPos;
    public float attackDamage;
    public float attackRange;
    public LayerMask damageableLayer;

    [Header("Hp Bar Settings")]
    public Image hpBarPlayer;
    public Image hpBarPlayerEffect;
    private float hpSpeed = 0.002f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;     
    }

    private void Attack()
    {
        Debug.Log("attack");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos.position, attackRange, damageableLayer);

        if (enemies.Length != 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if(Random.Range(1,3) == 2)
				{      
                    enemies[i].GetComponent<Mob>().TakeDamage(attackDamage*2, true);
                }
				else
				{
                    enemies[i].GetComponent<Mob>().TakeDamage(attackDamage, false);
                }
            }
        }
    }
   
    public void PlayerDamaged(float damage)
    {
        if(!isInvulnerability)
		{
            hpPlayer -= damage;
            hpBarPlayer.fillAmount = hpPlayer * 0.01f;

            isInvulnerability = true;
            Invoke("Invulnerability", 0.3f);

           // Debug.Log(hpPlayer);
        }   
    }

    void Invulnerability()
	{
        isInvulnerability = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;  //отрисовка радиуса атаки 
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

        _rigidbody.velocity = new Vector2(moveInput * speed, _rigidbody.velocity.y);

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
            _rigidbody.velocity = Vector2.up * jumpForce;
        }


        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
#endif

        if(Input.GetKeyDown(KeyCode.J))
        {
            if(hpPlayer < maxhpPlayer)
            hpPlayer += 10;
            hpBarPlayer.fillAmount = hpPlayer * 0.01f;
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
            _rigidbody.velocity = Vector2.up * jumpForce;
        }
#endif
    }

    public void OnAttackButtonDown()
    {
#if UNITY_ANDROID
        Attack();
#endif
    }

    public void OnHealButtonDown()
    {
        hpPlayer += 10;
        hpBarPlayer.fillAmount = hpPlayer * 0.01f;

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
