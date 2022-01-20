using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour {
    // TextFields
    public Text levelText,
                hitpointText,
                goldText,
                upgradeText,
                expText;

    private int currCharacterSelect = 0;
    public Image characterSelectSprite;
    public Image weaponSprite;
    public RectTransform expBar;

    // Character Selection
    public void OnArrowClick(bool right) {
        if (right) {
            currCharacterSelect++;
            if (currCharacterSelect == GameManager.instance.playerSprites.Count) {
                currCharacterSelect = 0;
            }
        } else {
            currCharacterSelect--;
            if (currCharacterSelect < 0) {
                currCharacterSelect = GameManager.instance.playerSprites.Count - 1;
            }
        }
        OnSelectionChanged();
    }

	private void OnSelectionChanged() {
        characterSelectSprite.sprite = GameManager.instance.playerSprites[currCharacterSelect];
		GameManager.instance.player.SwapSprite(currCharacterSelect);
	}

    public void OnUpgradeClick() {
        if (GameManager.instance.TryUpgradeWeapon()) {
			UpdateMenu();
		}
    }

    // Update Character Information
    public void UpdateMenu() {
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];

		if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count) {
			upgradeText.text = "MAX";
		} else {
			upgradeText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
		}

        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        goldText.text = GameManager.instance.gold.ToString();
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();

		int currLevel = GameManager.instance.GetCurrentLevel();
		if (currLevel == GameManager.instance.expTable.Count) {
			expText.text = "MAX";
			expBar.localScale = Vector3.one;
		} else {
			int prevLevelExp = GameManager.instance.GetExpToLevel(currLevel - 1);
			int currLevelExp = GameManager.instance.GetExpToLevel(currLevel);
			int diffLevelExp = currLevelExp - prevLevelExp;
			int currExpIntoLevel = GameManager.instance.experience - prevLevelExp;
			float ratio = (float)currExpIntoLevel / (float)diffLevelExp;
			expText.text = currExpIntoLevel.ToString() + " / " + diffLevelExp;
			expBar.localScale = new Vector3(ratio, 1, 1);
		}
    }
}
