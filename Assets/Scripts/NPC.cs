using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Collidable {

    [TextArea(3, 3)]
    public string message = "Generic message";
    public float duration = 4.0f;
    private float cooldown;
    private float lastShown;
	private Vector3 offset = new Vector3(0, 0.16f, 0);

	protected override void Start() {
		base.Start();
        cooldown = duration + 1.0f;
		lastShown = -cooldown;
	}

	protected override void OnCollide(Collider2D collision) {
		if (Time.time - lastShown > cooldown) {
            lastShown = Time.time;
            GameManager.instance.ShowText(
                message,
                30,
                Color.white,
                transform.position + offset,
                Vector3.zero,
                duration
            );
        }
	}
}
