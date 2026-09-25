using UnityEngine;
using UnityEngine.UI;

public class RuntimeUI : MonoBehaviour
{
    Font font;
    void Awake() { font = Resources.GetBuiltinResource<Font>("Arial.ttf"); Build(); }

    GameObject Panel(string name, Transform parent, Color c) {
        var go = new GameObject(name); go.transform.SetParent(parent, false); go.AddComponent<Image>().color = c; return go;
    }

    Text TextObj(string name, Transform parent, string value, int size, TextAnchor anchor = TextAnchor.MiddleCenter) {
        var go = new GameObject(name); go.transform.SetParent(parent, false);
        var t = go.AddComponent<Text>(); t.font = font; t.text = value; t.fontSize = size; t.alignment = anchor; t.color = Color.white;
        t.resizeTextForBestFit = true; t.resizeTextMinSize = 12; t.resizeTextMaxSize = size; return t;
    }

    void SetRect(GameObject go, Vector2 min, Vector2 max) {
        var r = go.GetComponent<RectTransform>(); r.anchorMin = min; r.anchorMax = max; r.offsetMin = Vector2.zero; r.offsetMax = Vector2.zero;
    }

    Button ButtonObj(string name, Transform parent, string label, Color color) {
        var go = Panel(name, parent, color); var b = go.AddComponent<Button>();
        var t = TextObj("Label", go.transform, label, 28); t.raycastTarget = false; return b;
    }

    void Build()
    {
        var canvasGO = new GameObject("Canvas"); var canvas = canvasGO.AddComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasGO.AddComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080, 1920); scaler.matchWidthOrHeight = 0.5f; canvasGO.AddComponent<GraphicRaycaster>();
        var root = canvasGO.transform;

        var bg = Panel("Background", root, new Color(0.035f,0.055f,0.08f,1)); SetRect(bg, Vector2.zero, Vector2.one);
        var header = Panel("Header", root, new Color(0.07f,0.10f,0.15f,1)); SetRect(header, new Vector2(0,0.86f), new Vector2(1,1));
        var title = TextObj("Title", header.transform, "BLOCKFORGE", 42); SetRect(title.gameObject, new Vector2(0.03f,0.48f), new Vector2(0.48f,0.95f));
        var coins = TextObj("Coins", header.transform, "0", 30); SetRect(coins.gameObject, new Vector2(0.50f,0.50f), new Vector2(0.72f,0.95f));
        var crystals = TextObj("Crystals", header.transform, "0", 30); SetRect(crystals.gameObject, new Vector2(0.73f,0.50f), new Vector2(0.98f,0.95f));
        var ore = TextObj("Ore", header.transform, "Руда: 0", 26); SetRect(ore.gameObject, new Vector2(0.03f,0.02f), new Vector2(0.50f,0.50f));
        var message = TextObj("Message", root, "", 25); SetRect(message.gameObject, new Vector2(0.05f,0.80f), new Vector2(0.95f,0.86f));

        var oreBtn = ButtonObj("OreButton", root, "⛏
ДОБЫВАТЬ РУДУ", new Color(0.12f,0.42f,0.32f,1));
        SetRect(oreBtn.gameObject, new Vector2(0.16f,0.43f), new Vector2(0.84f,0.72f));
        var pickaxe = TextObj("PickaxeInfo", root, "", 25); SetRect(pickaxe.gameObject, new Vector2(0.05f,0.37f), new Vector2(0.95f,0.43f));

        var bossPanel = Panel("BossPanel", root, new Color(0.16f,0.09f,0.11f,1)); SetRect(bossPanel, new Vector2(0.05f,0.20f), new Vector2(0.95f,0.36f));
        var bossText = TextObj("BossText", bossPanel.transform, "КАМЕННЫЙ ГОЛЕМ", 26); SetRect(bossText.gameObject, new Vector2(0.02f,0.60f), new Vector2(0.98f,1));

        var sliderGO = new GameObject("BossSlider"); sliderGO.transform.SetParent(bossPanel.transform, false);
        var slider = sliderGO.AddComponent<Slider>(); SetRect(sliderGO, new Vector2(0.05f,0.32f), new Vector2(0.95f,0.58f));
        var bgImg = new GameObject("Background").AddComponent<Image>(); bgImg.transform.SetParent(sliderGO.transform, false); bgImg.color = new Color(0.25f,0.25f,0.25f,1); SetRect(bgImg.gameObject, Vector2.zero, Vector2.one);
        var fill = new GameObject("Fill").AddComponent<Image>(); fill.transform.SetParent(sliderGO.transform, false); fill.color = new Color(0.8f,0.15f,0.12f,1); SetRect(fill.gameObject, Vector2.zero, Vector2.one);
        slider.fillRect = fill.GetComponent<RectTransform>(); slider.targetGraphic = fill;

        var attack = ButtonObj("Attack", bossPanel.transform, "⚔ АТАКОВАТЬ", new Color(0.55f,0.12f,0.10f,1)); SetRect(attack.gameObject, new Vector2(0.20f,0.02f), new Vector2(0.80f,0.30f));
        var upgrade = ButtonObj("Upgrade", root, "УЛУЧШИТЬ КИРКУ", new Color(0.12f,0.27f,0.50f,1)); SetRect(upgrade.gameObject, new Vector2(0.05f,0.08f), new Vector2(0.47f,0.17f));
        var auto = ButtonObj("Auto", root, "АВТОДОБЫЧА", new Color(0.45f,0.30f,0.08f,1)); SetRect(auto.gameObject, new Vector2(0.53f,0.08f), new Vector2(0.95f,0.17f));

        var gmGO = new GameObject("GameManager"); var gm = gmGO.AddComponent<GameManager>();
        gm.coinsText = coins; gm.crystalsText = crystals; gm.oreText = ore; gm.pickaxeText = pickaxe; gm.autoText = auto.GetComponentInChildren<Text>();
        gm.bossHPText = bossText; gm.bossSlider = slider; gm.messageText = message; gm.oreButton = oreBtn; gm.upgradeButton = upgrade; gm.autoButton = auto; gm.attackButton = attack;
        oreBtn.onClick.AddListener(gm.Mine); upgrade.onClick.AddListener(gm.UpgradePickaxe); auto.onClick.AddListener(gm.ToggleAutoMine); attack.onClick.AddListener(gm.AttackBoss);
    }
}
