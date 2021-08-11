using UnityEngine;


public class Swordsmen : Mob
{

    private bool isAttack;
    private bool isCdAttack = false;


    [SerializeField] Transform attackPos;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float mobDamage;

    [SerializeField] private float groundLevel = 1.3f; //настраивается для каждого моба на сцене отдельно
    [SerializeField] private Canvas mobCanvas; //для флипа полоски хп при повороте

    [Header("Distances")]
    [SerializeField]
    private float rangeAgro = 10f;
    [SerializeField]
    private float rangeRage = 5f;
    private float distToPlayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        MobFlip();
        isLeftScale = true;
        player = GameObject.FindGameObjectWithTag("Player");
    } 


    
    void Update()
    {
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(gameObject.transform.position.y, groundLevel, groundLevel));
        distToPlayer = Vector2.Distance(player.transform.position, gameObject.transform.position);

        if (Mathf.Abs(gameObject.transform.position.y - player.transform.position.y) < 0.3f)
        { 
            if (distToPlayer < rangeAgro)
            {
                Persuit();
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

	protected override void Persuit()
	{
		if (player.transform.position.x > gameObject.transform.position.x && isLeftScale == true)
		{
			MobFlip();
			isLeftScale = false;
		}
		else if (player.transform.position.x < gameObject.transform.position.x && isLeftScale == false)
		{
			MobFlip();
			isLeftScale = true;
		}



		if (distToPlayer < rangeAgro)
		{
			if ((gameObject.transform.position.x > player.transform.position.x && speed > 0) || (gameObject.transform.position.x < player.transform.position.x && speed < 0))
			{
				speed *= -1; //в какую сторону идти
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
        Gizmos.color = Color.green;  //отрисовка радиуса атаки 
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

