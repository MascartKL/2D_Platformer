﻿using UnityEngine;

namespace Assets.Scripts.Monsters
{
	class Archer : Mob
	{
		[Header("Bow parametrs")]
		[SerializeField] private float speedArrow = 300f;
		[SerializeField] private float reloadArrow = 1f;
		private readonly float lenghtArrow = 1f;
		private bool activArrow = false;

		public GameObject arrowPrefab;
		private GameObject currentlyArrow;

		void Start()
		{
			anim = GetComponent<Animator>();
			rb = GetComponent<Rigidbody2D>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		protected override void Update()
		{
			base.Update();
			if (isStan)
				return;

			base.Сonduct();
		}

		protected override void Attack()
		{
			base.Attack();

			if (!activArrow)
			{	
				anim.SetBool("isAttack", true);
				activArrow = true;
				Invoke("StartAttack", reloadArrow);
			}

			if (currentlyArrow != null)
			{
				if (Physics2D.OverlapCircle(currentlyArrow.transform.position, lenghtArrow/2, playerLayer) == true)
				{
					Destroy(currentlyArrow);
					player.GetComponent<Player>().PlayerDamaged(damage);
				}
			}
		}

		void Reload()
		{
			anim.SetBool("isAttack", false);
			activArrow = false;
		}

		void StartAttack()
		{
			if (!isStan)
			{
				float tempVelocity = transform.position.x - player.transform.position.x;
				currentlyArrow = Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.Euler(0, 0, 0));

				if (tempVelocity > 0)
					currentlyArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedArrow * -1, speedArrow / 5));

				if (tempVelocity < 0)
					currentlyArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedArrow, speedArrow / 5));
				_isAttack = false;
				Reload();
			}
		}
	}
}
