using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Monsters
{
	class Dragon : Mob
	{
		[Header("Fireball parametrs")]
		[SerializeField] private float speedFireball = 300f;
		[SerializeField] private float reloadFireball = 2f;
		private readonly float lenghtFireball = 1f;
		private bool activFireball = false;

		[Header("Dragon parametrs")]
		[SerializeField] private int fatigueMaxCount = 4;
		private int fatigueCount = 1;
		private bool isFatigue;

		public GameObject fireballPrefab;
		private GameObject currentlyFireball;

		void Start()
		{
			anim = GetComponent<Animator>();
			rb = GetComponent<Rigidbody2D>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		protected override void Update()
		{
			base.Update();
			base.Сonduct();
		}

		protected override void Persuit()
		{
			base.Attack();
			float tempVelocity_x =  player.transform.position.x - transform.position.x;

			rb.velocity = new Vector2
			(speed * (tempVelocity_x / Mathf.Abs(tempVelocity_x)),
			0f);		
		}

		protected override void Attack()
		{
			base.Attack();

			rb.Sleep();

			if (isFatigue)
			{
				gameObject.transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, 2f), Time.deltaTime);
				Invoke("Rested", 7);
			}

			if (!activFireball && !isFatigue)
			{
				activFireball = true;
				Invoke("StartAttack", reloadFireball);
			}

			if (currentlyFireball != null)
			{
				if (Physics2D.OverlapCircle(currentlyFireball.transform.position, lenghtFireball / 2, playerLayer) == true)
				{
					Destroy(currentlyFireball);
					player.GetComponent<Player>().PlayerDamaged(damage);
				}
			}
		}

		void Reload()
		{
			activFireball = false;
		}

		void StartAttack()
		{

			float tempVelocity_x = player.transform.position.x - transform.position.x;
			float tempVelocity_y = transform.position.y - player.transform.position.y;

			currentlyFireball = Instantiate
				(fireballPrefab, new Vector3(transform.position.x + transform.localScale.x/2, 
				transform.position.y + 0.5f, 0), 
				Quaternion.Euler(0,0,Mathf.Atan2(tempVelocity_y,Mathf.Abs(tempVelocity_x))*180/Mathf.PI));

			currentlyFireball.GetComponent<Rigidbody2D>().velocity = 
				new Vector2(speedFireball * tempVelocity_x,
				tempVelocity_y * -1);
			Fatigue();
			Reload();
		}

		void Rested()
		{
			gameObject.transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, 7f), Time.deltaTime);
			isFatigue = false;
		}

		void Fatigue()
		{
			if(fatigueCount++ == fatigueMaxCount)
			{
				fatigueCount = 0;
				isFatigue = true;
			}
		}
	}
}
