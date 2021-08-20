using UnityEngine;

namespace Assets.Scripts.Monsters
{
	class FireElemental : Mob
	{
		[Header("Parametrs spell sphere")]
		[SerializeField] private float speedSkill = 200f;
		[SerializeField] private float reload = 4f;
		private readonly float radiusSphere = 1f;
		private bool activSphere = false;
		public GameObject spherePrefab;
		private GameObject currentlySphere;

		[Header("Parametrs Aoe spell")]
		[SerializeField] private float squareSpell = 200f;
		[SerializeField] private float damageSpell = 20f;
		[SerializeField] private float reloadAoe = 6f;
		[SerializeField] private float timeAoe = 0.2f;
		private bool activSpell = false;
		public GameObject aoeSpellPrefab;
		private GameObject currentlySpell;

		private bool startAttack = true;


		void Start()
		{
			aoeSpellPrefab.transform.localScale = new Vector3(squareSpell / 5, squareSpell / 10,0);
			anim = GetComponent<Animator>();
			rb = GetComponent<Rigidbody2D>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		protected override void  Update()
		{
			base.Update();
			if (isStan)
			return;

			Сonduct();
		}

		protected override void Сonduct()
		{
			distanceToPlayer = Vector2.Distance(gameObject.transform.position, player.transform.position);
			if (distanceToPlayer < distanceAgro)
			{
				if (distanceToPlayer < distanceAttack)
				{
					base.Attack();
					Debug.Log(startAttack);
					if (startAttack)
					{
						if (Random.Range(1, 3) == 1)
							Attack();
						else
							AoeAttack();
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
			if (currentlySpell || currentlySphere)
			{
				Damage();
			}
		}

		void Reload()
		{
			if (activSpell)
			{
				activSpell = false;
				Destroy(currentlySpell);
			}

			if(activSphere)
			{
				activSphere = false;			
			}

			startAttack = true;
		}

		void StartAttack()
		{
			if(!isStan)
			{
				if (activSphere)
				{
					float tempVelocity = transform.position.x - player.transform.position.x;
					currentlySphere = Instantiate(spherePrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);

					if (tempVelocity > 0)
						currentlySphere.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedSkill * -1, 0));

					if (tempVelocity < 0)
						currentlySphere.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedSkill, 0));

					_isAttack = false;
					Reload();
				}

				if (activSpell)
				{
					if (Mathf.Abs(player.transform.position.y - gameObject.transform.position.y) < 0.5f)
					{
						currentlySpell = Instantiate(aoeSpellPrefab,
						new Vector3(player.transform.position.x, player.transform.position.y - player.transform.localScale.y / 3, 0),
						Quaternion.identity);
					}
					else
					{
						activSpell = false;
					}

					_isAttack = false;
					Invoke("Reload", timeAoe);
				}
			}			
		}

		void AoeAttack()
		{
			startAttack = false;

			if (!activSpell)
			{
				activSpell = true;
				Invoke("StartAttack", reloadAoe);
			}
		}

		protected override void Attack()
		{
			startAttack = false;

			if (!activSphere)
			{
				activSphere = true;
				Invoke("StartAttack", reload);
			}

		}

		void Damage()
		{
			if (currentlySphere != null)
			{
				if (Physics2D.OverlapCircle(currentlySphere.transform.position, radiusSphere, playerLayer) == true)
				{
					Destroy(currentlySphere);
					player.GetComponent<Player>().PlayerDamaged(damage);
				}
			}
			if (currentlySpell != null)
			{
				if (Physics2D.OverlapCircle(currentlySpell.transform.position, squareSpell / 10, playerLayer) == true)
				{
					player.GetComponent<Player>().PlayerDamaged(damageSpell);
				}
			}
		}
	}
}
