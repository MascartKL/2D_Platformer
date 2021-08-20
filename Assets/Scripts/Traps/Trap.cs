using UnityEngine;

namespace Assets.Scripts.Traps
{
	public class Trap : MonoBehaviour
	{
		public GameObject player;

		[Header("Parametrs trap")]
		[SerializeField] protected float damage = 5f;

		protected virtual void OnTriggerStay2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Player")
			{
				player.GetComponent<Player>().PlayerDamaged(damage);
			}
		}
	}
}
