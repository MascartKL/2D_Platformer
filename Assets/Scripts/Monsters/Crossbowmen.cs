using UnityEngine;

namespace Assets.Scripts.Monsters
{
	class Crossbowmen : Mob
	{
		[Header("Crossbow parametrs")]
		[SerializeField] private float speedBolt = 600f;
		[SerializeField] private float reloadBolt = 4f;
		private readonly float lenghtBolt = 1f;
		private bool activBolt = false;

		public GameObject boltPrefab;
		private GameObject currentlyBolt;

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
					Attack();
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

			if (!activBolt)
			{
				//anim.SetBool("isAttack", true);
				activBolt = true;
				Invoke("StartAttack", reloadBolt);
			}

			if (currentlyBolt != null)
			{
				if (Physics2D.OverlapCircle(currentlyBolt.transform.position, lenghtBolt / 2, playerLayer) == true)
				{
					Destroy(currentlyBolt);
					player.GetComponent<Player>().PlayerDamaged(damage);
				}
			}
		}

		void Reload()
		{
			//anim.SetBool("isAttack", false);
			activBolt = false;
		}

		void StartAttack()
		{

			float tempVelocity = transform.position.x - player.transform.position.x;
			currentlyBolt = Instantiate(boltPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);

			if (tempVelocity > 0)
				currentlyBolt.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedBolt * -1, 0));

			if (tempVelocity < 0)
				currentlyBolt.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedBolt, 0));

			Reload();

		}
	}
}
