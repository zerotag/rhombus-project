using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> expTable;

    // References
    public Player player;
	public Weapon weapon;
	public FloatingTextManager floatingTextManager;
	public RectTransform hpBar;
	public Animator deathMenuAnim;

    // Data
    public int experience;
    public int gold;

	public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
		floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
	}

	// Upgrade Weapon
	public bool TryUpgradeWeapon() {
		// Check if max level
		if (weaponPrices.Count <= weapon.weaponLevel) { return false; }

		if (gold >= weaponPrices[weapon.weaponLevel]) {
			gold -= weaponPrices[weapon.weaponLevel];
			weapon.UpgradeWeapon();
			return true;
		}

		return false;
	}

	// Hitpoint Bar
	public void OnHitpointChange() {
		float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
		hpBar.localScale = new Vector3(1, ratio, 1);
	}

	// Experience
	public int GetCurrentLevel() {
		int r = 0;
		int add = 0;

		while (experience >= add) {
			add += expTable[r];
			r++;

			if (r == expTable.Count) { break; }
		}

		return r;
	}

	public int GetExpToLevel(int level) {
		int r = 0;
		int exp = 0;

		while (r < level) {
			exp += expTable[r];
			r++;
		}

		return exp;
	}

	public void GrantExp(int amount) {
		int currLevel = GetCurrentLevel();
		experience += amount;
		if (currLevel < GetCurrentLevel()) {
			OnLevelUp();
		}
	}

	public void OnLevelUp() {
		GameManager.instance.ShowText(
			"Level Up",
			40,
			Color.magenta,
			player.transform.position,
			Vector3.up * 40,
			1.0f
		);
		player.OnLevelUp();
		OnHitpointChange();
	}

    public void OnSceneLoaded(Scene s, LoadSceneMode mode) {
		OnHitpointChange();
		player.transform.position = GameObject.Find("SpawnPoint").transform.position;
	}

	public void Respawn() {
		deathMenuAnim.SetTrigger("Hide");
		UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
		player.Respawn();
	}

	// Game State
    /*
    ** INT preferedSkin
    ** INT gold
    ** INT experience
    ** INT weaponLevel
    */
    public void SaveState() {
        string s = "";

		// Set Data
        s += "0"                            + "|";
        s += gold.ToString()                + "|";
        s += experience.ToString()          + "|";
        s += weapon.weaponLevel.ToString();

        PlayerPrefs.SetString("SaveState", s);
    }

    public void LoadState(Scene s, LoadSceneMode mode) {
		SceneManager.sceneLoaded -= LoadState;

        if (!PlayerPrefs.HasKey("SaveState")) { return; }

        // Get Data
		string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Skin
        gold = int.Parse(data[1]);
        experience = int.Parse(data[2]);
		player.SetLevel(GetCurrentLevel());
        weapon.SetWeaponLevel(int.Parse(data[3]));
    }
}
