using UnityEngine;

public class Kemal : Mob
{
	[Header("Kemal parametrs")]
	[SerializeField] private float speedRage = 6f;

	private bool isAttack = true;
	private bool isRoar;

	void Start()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		_player = player.GetComponent<Player>();
	}

	protected override void Update()
	{
		base.Update();
		Ñonduct();
	}

	protected override void Ñonduct()
	{
		distanceToPlayer = Vector2.Distance(gameObject.transform.position, player.transform.position);
		if (distanceToPlayer < distanceAgro)
		{
			if (distanceToPlayer < distanceAttack)
			{
				if (isAttack)
					Attack();
			}
			else
			{
				anim.SetBool("isRun", true);
				anim.SetBool("isAttack", false);
				Persuit();	
			}
		}
		else
		{
			anim.SetBool("isRun", false);
			rb.velocity = new Vector2(0, 0);
		}
	}

	protected override void Attack()
	{
		anim.SetBool("isRun", false);

		base.Attack();
		StanPlayer();
		StartAttack();

		_isAttack = true;
		isAttack = false;
	}

	protected override void Persuit()
	{	
		if(!isRoar)
		{
			anim.SetBool("isRoar", true);
			Invoke("WakeUp", 2f);
		}

		else
		{
			base.Attack();//Ïîâîðîò äëÿ àòàêè, à íå àòàêà.
				if (transform.position.x > player.transform.position.x)
				{
					rb.velocity = new Vector2(speedRage * -1, 0);
				}

				if (transform.position.x < player.transform.position.x)
				{
					rb.velocity = new Vector2(speedRage, 0);
				}
		}
	}

	void WakeUp()
	{
		isRoar = true;
		anim.SetBool("isRoar", false);
	}

	void Reload()
	{
		anim.SetBool("isAttack", false);
		isAttack = true;
		_isAttack = false;
	}

	void StartAttack()
	{
		anim.SetBool("isAttack", true);
		if (Physics2D.OverlapCircle(transform.position, distanceAttack, playerLayer))
		{
			player.GetComponent<Player>().PlayerDamaged(damage);
		}
		Invoke("Reload", 1);	
	}
}
