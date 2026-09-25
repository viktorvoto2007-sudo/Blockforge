using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public int coins = 0;
    public int crystals = 0;
    public int pickaxeLevel = 1;
    public int ore = 0;
    public bool autoMine = false;
    public int bossHP = 500;
    public int bossMaxHP = 500;
    public int bossDefeats = 0;
    public Text coinsText, crystalsText, oreText, pickaxeText, autoText;
    public Text bossHPText, messageText;
    public Slider bossSlider;
    public Button oreButton, upgradeButton, autoButton, attackButton;
    float autoTimer, messageTimer;

    void Awake() { I = this; Load(); }
    void Start() { Refresh(); StartCoroutine(AutoSave()); ShowMessage("Добро пожаловать в Blockforge!"); }

    void Update()
    {
        if (autoMine) { autoTimer += Time.deltaTime; if (autoTimer >= 1f) { autoTimer = 0f; Mine(1 + pickaxeLevel / 3); } }
        if (messageTimer > 0) { messageTimer -= Time.deltaTime; if (messageTimer <= 0 && messageText) messageText.text = ""; }
    }

    public void Mine() { Mine(1); }
    void Mine(int amount) { ore += amount * pickaxeLevel; coins += amount * (2 + pickaxeLevel); Refresh(); }

    public void UpgradePickaxe()
    {
        int cost = 50 * pickaxeLevel;
        if (coins < cost) { ShowMessage("Нужно " + cost + " монет!"); return; }
        coins -= cost; pickaxeLevel++; ShowMessage("Кирка улучшена до " + pickaxeLevel + " уровня!"); Refresh(); Save();
    }

    public void ToggleAutoMine()
    {
        int cost = 250;
        if (!autoMine) {
            if (coins < cost) { ShowMessage("Автодобыча стоит " + cost + " монет."); return; }
            coins -= cost; autoMine = true; ShowMessage("Автодобыча включена!");
        } else { autoMine = false; ShowMessage("Автодобыча выключена."); }
        Refresh(); Save();
    }

    public void AttackBoss()
    {
        int damage = 10 + pickaxeLevel * 5;
        bossHP -= damage;
        if (bossHP <= 0) {
            bossHP = 0;
            int reward = 300 + pickaxeLevel * 50;
            coins += reward; crystals += 5; bossDefeats++;
            ShowMessage("ГОЛЕМ ПОБЕЖДЁН! +" + reward + " монет и +5 кристаллов!");
            Invoke(nameof(ResetBoss), 1.0f);
        } else ShowMessage("Удар! -" + damage + " HP");
        Refresh();
    }

    void ResetBoss() { bossMaxHP += 250; bossHP = bossMaxHP; Refresh(); }

    void ShowMessage(string s) { if (!messageText) return; messageText.text = s; messageTimer = 2.5f; }

    void Refresh()
    {
        if (coinsText) coinsText.text = coins.ToString("N0");
        if (crystalsText) crystalsText.text = crystals.ToString("N0");
        if (oreText) oreText.text = "Руда: " + ore.ToString("N0");
        if (pickaxeText) pickaxeText.text = "⛏ Кирка Lv." + pickaxeLevel + "  •  Сила: " + (10 + pickaxeLevel * 5);
        if (autoText) autoText.text = autoMine ? "⚡ АВТОДОБЫЧА: ВКЛ" : "⚡ АВТОДОБЫЧА • 250 🪙";
        if (bossHPText) bossHPText.text = "КАМЕННЫЙ ГОЛЕМ   " + bossHP + " / " + bossMaxHP;
        if (bossSlider) { bossSlider.maxValue = bossMaxHP; bossSlider.value = bossHP; }
        if (upgradeButton) upgradeButton.GetComponentInChildren<Text>().text = "УЛУЧШИТЬ КИРКУ
" + (50 * pickaxeLevel) + " 🪙";
        if (oreButton) oreButton.GetComponentInChildren<Text>().text = "⛏ ДОБЫВАТЬ РУДУ";
    }

    IEnumerator AutoSave() { while (true) { yield return new WaitForSeconds(5f); Save(); } }

    void Save()
    {
        PlayerPrefs.SetInt("coins", coins); PlayerPrefs.SetInt("crystals", crystals); PlayerPrefs.SetInt("pickaxe", pickaxeLevel);
        PlayerPrefs.SetInt("ore", ore); PlayerPrefs.SetInt("auto", autoMine ? 1 : 0); PlayerPrefs.SetInt("bossHP", bossHP);
        PlayerPrefs.SetInt("bossMaxHP", bossMaxHP); PlayerPrefs.SetInt("defeats", bossDefeats); PlayerPrefs.Save();
    }

    void Load()
    {
        coins = PlayerPrefs.GetInt("coins", 0); crystals = PlayerPrefs.GetInt("crystals", 0);
        pickaxeLevel = PlayerPrefs.GetInt("pickaxe", 1); ore = PlayerPrefs.GetInt("ore", 0);
        autoMine = PlayerPrefs.GetInt("auto", 0) == 1; bossHP = PlayerPrefs.GetInt("bossHP", 500);
        bossMaxHP = PlayerPrefs.GetInt("bossMaxHP", 500); bossDefeats = PlayerPrefs.GetInt("defeats", 0);
    }

    void OnApplicationPause(bool pause) { if (pause) Save(); }
    void OnApplicationQuit() { Save(); }
}
