using UnityEngine;

namespace Assets.Scripts.Monsters
{
	class Shieldmen : Mob
	{
		[Header("Shieldmen parametrs")]
		[SerializeField] private float rangeRage = 5f;
		[SerializeField] private float reloadShield = 5f;
		[SerializeField] private float speedRage = 6f;

		private bool isAttack = true;
		private bool isDefense = true;

		void Start()
		{
			anim = GetComponent<Animator>();
			rb = GetComponent<Rigidbody2D>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		void Update()
		{
			HpBarChange();

			distanceToPlayer = Vector2.Distance(gameObject.transform.position, player.transform.position);
			if (distanceToPlayer < distanceAgro)
			{
				if (distanceToPlayer < distanceAttack)
				{
					if(isAttack)
					Attack();
					if(hp < 50 && isDefense)
					{
						Defense();
					}
				}
				else
				{
					Persuit();
				}
			}
			else
			{
				rb.velocity = new Vector2(0, 0);
			}


		}

		protected override void Attack()
		{
			base.Attack();
			rb.velocity = new Vector2(0, 0);
			StartAttack();
			isAttack = false;
		}

		protected override void Persuit()
		{
			if (Mathf.Abs(transform.position.x - player.transform.position.x) < rangeRage)
			{
				base.Attack();//Поворот для атаки, а не атака.
				if (transform.position.x > player.transform.position.x)
				{
					rb.velocity = new Vector2(speedRage * -1, 0);
				}

				if (transform.position.x < player.transform.position.x)
				{
					rb.velocity = new Vector2(speedRage, 0);
				}
			}
			else
			{
				base.Persuit();
			}		
		}

		void Reload()
		{
			isAttack = true;
		}

		void Defense()
		{
			isDefense = false;
			physRes = 90;
			Invoke("AnDefense", 2);
		}

		void AnDefense()
		{
			physRes = 10;
			Invoke("RefreshDef", reloadShield);
		}

		void RefreshDef()
		{
			isDefense = true;
		}

		void StartAttack()
		{
			if(Physics2D.OverlapCircle(transform.position,distanceAttack, playerLayer))
			{
				player.GetComponent<Player>().PlayerDamaged(damage);
			}

			Invoke("Reload", 2);
		}
	}
}
