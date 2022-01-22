using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover {
	private SpriteRenderer spriteRenderer;
	private bool isAlive = true;

	protected override void Start() {
		base.Start();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void ReceiveDamage(Damage dmg) {
		base.ReceiveDamage(dmg);
		GameManager.instance.OnHitpointChange();
	}

	private void FixedUpdate() {
		if (!isAlive) { return; }

		// Get Input
		float x = Input.GetAxisRaw("Horizontal");
		float y = Input.GetAxisRaw("Vertical");
		UpdateMotor(new Vector3(x, y, 0));
	}

	public void SwapSprite(int spriteID) {
		spriteRenderer.sprite = GameManager.instance.playerSprites[spriteID];
	}

	public void OnLevelUp() {
		maxHitpoint++;
		hitpoint = maxHitpoint;
	}

	public void SetLevel(int level) {
		for (int i = 0; i < level; i++) {
			OnLevelUp();
		}
	}

	public void Heal(int amount) {
		if (hitpoint == maxHitpoint) { return; }

		hitpoint += amount;
		if (hitpoint > maxHitpoint) {
			hitpoint = maxHitpoint;
		}

		GameManager.instance.ShowText(
			"+" + amount.ToString() + " HP",
			25,
			Color.green,
			transform.position,
			Vector3.up * 30,
			1.0f
		);

		GameManager.instance.OnHitpointChange();
	}

	protected override void Death() {
		isAlive = false;
		boxCollider.enabled = false;
		GameManager.instance.deathMenuAnim.SetTrigger("Show");
	}

	public void Respawn() {
		Heal(maxHitpoint);
		isAlive = true;
		lastImmune = Time.time;
		boxCollider.enabled = true;
		pushDirection = Vector3.zero;
	}
}
