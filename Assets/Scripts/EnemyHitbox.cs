using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable {
	public int damage = 1;
	public float pushForce = 2.0f;

	protected override void OnCollide(Collider2D collision) {
		if (collision.tag == "Fighter" && collision.name == "Player") {
			Damage dmg = new Damage {
				damageAmount = damage,
				origin = transform.position,
				pushForce = this.pushForce
			};

			collision.SendMessage("ReceiveDamage", dmg);
		}
	}
}
