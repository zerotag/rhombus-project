using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collidable {
	public int[] damagePoint = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
	public float[] pushForce = { 2.0f, 2.1f, 2.2f, 2.3f, 2.4f, 2.5f, 2.6f, 2.7f, 2.8f, 3.0f };

	// Upgrade
	public int weaponLevel = 0;
	private SpriteRenderer spriteRenderer;

	// Swing
	private Animator anim;
	private float cooldown = 0.5f;
	private float lastSwing;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void Start() {
		base.Start();
		anim = GetComponent<Animator>();
	}

	protected override void Update() {
		base.Update();
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (Time.time - lastSwing > cooldown) {
				lastSwing = Time.time;
				Swing();
			}
		}
	}

	protected override void OnCollide(Collider2D collision) {
		if (collision.tag == "Fighter") {
			if (collision.name != "Player") {
				// Create new damage object
				Damage dmg = new Damage() {
					damageAmount = damagePoint[weaponLevel],
					origin       = transform.position,
					pushForce    = pushForce[weaponLevel]
				};

				collision.SendMessage("ReceiveDamage", dmg);
			}
		}
	}

	private void Swing() {
		anim.SetTrigger("Swing");
	}

	public void UpgradeWeapon() {
		SetWeaponLevel(weaponLevel + 1);
	}

	public void SetWeaponLevel(int level) {
		weaponLevel = level;
		spriteRenderer.sprite = GameManager.instance.weaponSprites[weaponLevel];
	}
}
