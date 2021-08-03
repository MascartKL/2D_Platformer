using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Swordsmen : DamageableObj
{
    private Transform playerTran;
    private Transform mobTran;
    private Animator anim;
    private Rigidbody2D rb;
    private float speed = 1.5f;

    private bool isAttack;
    private bool isCdAttack = false;
    private bool isHit;

    [SerializeField] Transform attackPos;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] LayerMask playerLayer;
    public GameObject player;
    [SerializeField] private float mobDamage;

    [SerializeField] private float groundLevel = 1.3f; //������������� ��� ������� ���� �� ����� ��������
    private bool isLeftScale;
    [SerializeField] private Canvas mobCanvas; //��� ����� ������� �� ��� ��������


    

    //

    [Header("Distances")]
    [SerializeField]
    private float rangeAgro = 10f;
    [SerializeField]
    private float rangeRage = 5f;
    private float distToPlayer;

    private float playerY;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mobTran = gameObject.transform;
        MobFlip();
        isLeftScale = true;
        player = GameObject.FindGameObjectWithTag("Player");
    } 


    
    void FixedUpdate()
    {
        playerTran = FindObjectOfType<Player>().transform;
        mobTran = gameObject.transform;
        playerY = playerTran.position.y;
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(mobTran.position.y, groundLevel, groundLevel));
        distToPlayer = Vector2.Distance(playerTran.position, mobTran.position);

        // ����������� �������� �� Y, �������� ����� ��� ���������� ������� � ���� ���������� ��� ������ �� ������, 
        //� ��� �������� � ������ �� ����� ������ �� ����� �����
        //trigger � box collider ��������� ����� ����� ���� ��������� ����� �����, ������ �����, � ����� ��� �� ��������� � ������� �� �����
        if (Mathf.Abs(mobTran.position.y - playerTran.position.y) < 0.3f) // ����� ��� �� ������� �� ������, ����� ��� ��������� 
        {                                                                 // ����� ������� ��� ������ ���������
            if (distToPlayer < rangeAgro)// && Vector2.Distance(playerTran.position, mobTran.position) > 2f)
            {
                Persuit();
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    void Persuit()
    {
        if (playerTran.position.x > mobTran.position.x && isLeftScale == true)
        {
            MobFlip();
            isLeftScale = false;
        }
        else if (playerTran.position.x < mobTran.position.x && isLeftScale == false)
        {
            MobFlip();
            isLeftScale = true;
        }



        if (distToPlayer < rangeAgro)
        {
            if ((mobTran.position.x > playerTran.position.x && speed > 0) || (mobTran.position.x < playerTran.position.x && speed < 0))
            {
                speed *= -1; //� ����� ������� ����
            }



            if (distToPlayer > 2f && isAttack == false)
            {               
                if (distToPlayer < rangeRage)
                {
                    rb.velocity = new Vector2(speed * 1.5f, 0);
                }
                else
                {
                    rb.velocity = new Vector2(speed, 0);
                }
            }
            else
            {
                rb.velocity = new Vector2(0, 0);

                if (isAttack == false)
                {
                    SwordsMenAttack();
                }
            }

            if (isCdAttack == true && distToPlayer < 5f)
            {
                rb.velocity = new Vector2(speed * -0.2f, 0);
            }
        }
        else 
        { 
            rb.velocity = new Vector2(0, 0);
        }
    }

    void SwordsMenAttack()
    {
        anim.SetBool("isAttack", true);
        isAttack = true;
        Invoke("SwordAttack", 0.85f);
        Invoke("StopAttack", 1f);
        Invoke("CdAttack", 5f);   
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;  //��������� ������� ����� 
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    void StopAttack()
    {
        anim.SetBool("isAttack", false);
        isCdAttack = true;
    }

    void CdAttack()
    {
        isAttack = false;
        isCdAttack = false;
    }
    void SwordAttack()
    {
        if (Physics2D.OverlapCircle(attackPos.position, attackRange, playerLayer) == true)
        {
            player.GetComponent<Player>().PlayerDamaged(mobDamage);
            //hitEvent.Invoke(mobDamage);
        }
    }

    void MobFlip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

        Vector3 CanvasScaler = mobCanvas.transform.localScale;
        CanvasScaler *= -1;
        mobCanvas.transform.localScale = CanvasScaler;
    }

    
}

