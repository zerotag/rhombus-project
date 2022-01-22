using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
	public float fireSpeed = 2.5f;
	public float distance = 0.25f;
	public Transform fireball;

	private void Update() {
		Vector3 offset = new Vector3(
			-Mathf.Cos(Time.time * fireSpeed) * distance,
			Mathf.Sin(Time.time * fireSpeed) * distance,
			0
		);
		fireball.position = transform.position + offset;
	}
}
