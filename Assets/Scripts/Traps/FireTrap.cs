using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Traps
{
	class FireTrap : Trap
	{
		[SerializeField] private float range = 50f;
		[SerializeField] private GameObject fireRay;
		[SerializeField] private GameObject point;

		private GameObject currentlyFireRay;

		RaycastHit2D raycast;
		

		private void Start()
		{
			StartCoroutine(Attack());
		}

		IEnumerator Attack()
		{
			while(true)
			{
				raycast = Physics2D.Raycast(transform.position, new Vector2(range * -1, 0));

				currentlyFireRay = Instantiate(fireRay, new Vector2(point.transform.position.x - fireRay.transform.localScale.x*5, point.transform.position.y), Quaternion.identity);
				Invoke("DeleteFireRay", 0.5f);

				if (raycast.collider != null)
				{
					if (raycast.collider.name == "Player")
					{
						player.GetComponent<Player>().PlayerDamaged(damage);
					}
				}

				yield return new WaitForSeconds(5f);
			}
		}

		void DeleteFireRay()
		{
			if (currentlyFireRay != null)
			Destroy(currentlyFireRay);
		}
	}
}
