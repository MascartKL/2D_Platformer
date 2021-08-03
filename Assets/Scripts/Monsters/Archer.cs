using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Assets.Scripts.Monsters
{
	class Archer : DamageableObj
	{

		[Header("Archer parametrs")]
		[SerializeField]
		private float distanceAgro = 10f;
		[SerializeField]
		private float distanceAttack = 7f;
		[SerializeField]
		private float damage = 5f;
		[SerializeField]
		private float speed = 1.5f;

		[Header("Bow parametrs")]
		[SerializeField]
		private float speedArrow = 1.5f;
		[SerializeField]
		private float reloadArrow = 4f;
		private float lenghtArrow = 1f;
		private bool activArrow = false;

		public GameObject arrowPrefab;
		private GameObject currentlyArrow;


		private float distanceToPlayer;
		private Transform mobTransform;
		private Rigidbody2D rb;

		private GameObject player;
		private Transform playerTransform;
		[SerializeField] LayerMask playerLayer;

		void Start()
		{
			rb = GetComponent<Rigidbody2D>();
			player = GameObject.FindGameObjectWithTag("Player");
		}

		void FixedUpdate()
		{
			playerTransform = player.transform;
			mobTransform = gameObject.transform;

			distanceToPlayer = Vector2.Distance(mobTransform.position, playerTransform.position);

			if (distanceToPlayer < distanceAgro)
			{
				if(distanceToPlayer < distanceAttack)
				{
					ArcherAttack();
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

		void Persuit()
		{
			if(transform.position.x - playerTransform.position.x > 0)
				rb.velocity = new Vector2(speed * -1, 0);

			if (transform.position.x - playerTransform.position.x < 0)
				rb.velocity = new Vector2(speed, 0);
		}

		void ArcherAttack()
		{
			rb.velocity = new Vector2(0, 0);
			if(!activArrow)
			{
				float tempVelocity = transform.position.x - playerTransform.position.x;
				currentlyArrow = Instantiate(arrowPrefab,new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(0,0,0));

				if(tempVelocity > 0)
				currentlyArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedArrow * -200,speedArrow * 40));

				if (tempVelocity < 0)
				currentlyArrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(speedArrow * 200,speedArrow * 40));
				
				activArrow = true;

				Invoke("Reload", reloadArrow);
			}

			if (currentlyArrow != null)
			{
				if (Physics2D.OverlapCircle(currentlyArrow.transform.position, lenghtArrow, playerLayer) == true)
				{
					Destroy(currentlyArrow);
					player.GetComponent<Player>().PlayerDamaged(damage);
				}
			}
		}

		void Reload()
		{
			if (currentlyArrow != null)
			Destroy(currentlyArrow);

			activArrow = false;
		}

		
	}
}
