using UnityEngine;

namespace Assets.Scripts.Traps
{
	class Knobstick : Trap
	{
		[SerializeField] private GameObject point;
		[SerializeField] private float amplitude = 180f;
		[SerializeField] private float speed = 5f;

		private void Update()
		{
			transform.RotateAround(point.transform.position, new Vector3(0, 0, amplitude), speed * Mathf.Sin(Time.time*2));
		}	
	}
}
