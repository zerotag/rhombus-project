using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable {
	public SceneAsset nextScene;

	protected override void OnCollide(Collider2D collision) {
		if (collision.name == "Player") {
			SceneManager.LoadScene(nextScene.name);
		}
	}
}
