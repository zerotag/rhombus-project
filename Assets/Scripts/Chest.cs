using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable {
	public Sprite emptyChest;
	public int goldAmount = 10;

	protected override void OnCollect() {
		if (!collected) {
			base.OnCollect();
			GetComponent<SpriteRenderer>().sprite = emptyChest;
			GameManager.instance.gold += goldAmount;
			GameManager.instance.ShowText(
				"+" + goldAmount + "g",
				60,
				Color.yellow,
				transform.position,
				Vector3.up * 20,
				2.0f
			);
		}
	}
}
