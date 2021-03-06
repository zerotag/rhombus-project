using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter {
	private Vector3 originalSize;
	protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
	public float xSpeed = 1.0f;
	public float ySpeed = 0.75f;

    protected virtual void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
		originalSize = transform.localScale;
    }

	protected virtual void UpdateMotor(Vector3 input) {
		// Reset MoveDelta
        moveDelta = new Vector3(
			input.x * xSpeed,
			input.y * ySpeed,
			0
		);

        // Swap sprite direction
        if (moveDelta.x > 0) {
            transform.localScale = originalSize;
        } else if (moveDelta.x < 0) {
            transform.localScale = new Vector3(
				originalSize.x * -1,
				originalSize.y,
				originalSize.z
			);
        }

		// Add push and reduce every frame
		moveDelta += pushDirection;
		pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        // Check Collision Y
        hit = Physics2D.BoxCast(
            transform.position,
            boxCollider.size,
            0,
            new Vector2(0, moveDelta.y),
            Mathf.Abs(moveDelta.y * Time.deltaTime),
            LayerMask.GetMask("Entity", "Blocking")
        );

        if (hit.collider == null) {
            // Make it move
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        // Check Collision X
        hit = Physics2D.BoxCast(
            transform.position,
            boxCollider.size,
            0,
            new Vector2(moveDelta.x, 0),
            Mathf.Abs(moveDelta.x * Time.deltaTime),
            LayerMask.GetMask("Entity", "Blocking")
        );

        if (hit.collider == null) {
            // Make it move
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
	}
}
