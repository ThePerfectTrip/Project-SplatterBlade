using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour {

    PlayerControls player;

    [Header("UI Elements")]
    public Image healthBar;
    public Image energyGauge;
    public Text energyCount;
    public Image sawbladeReadyIndicator;
    
    Color sawbladeReadyIndicatorColor;
    Color sawbladeFiredIndicatorColor;

    float defaultEnergyGaugeWidth;
    float defaultEnergyGaugeHeight;
    float lusterPerGauge;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();

        defaultEnergyGaugeWidth = energyGauge.rectTransform.sizeDelta.x;
        defaultEnergyGaugeHeight = energyGauge.rectTransform.sizeDelta.y;
        lusterPerGauge = 100;

        sawbladeReadyIndicatorColor = sawbladeReadyIndicator.color;
        sawbladeFiredIndicatorColor = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
        // Divide player luster by luster per gauge to determine gauge count and gauge width.
        string[] playerEnergy = (player.luster / lusterPerGauge).ToString("0.00").Split('.');
        string energyGaugeCount = playerEnergy[0];
        float energyGaugeWidth = defaultEnergyGaugeWidth * float.Parse("0." + playerEnergy[1]);
        Vector2 energyGaugeSize = new Vector2(energyGaugeWidth, defaultEnergyGaugeHeight);

        // Determine sawblade indicator color.
        Color sawbladeCurrentIndicatorColor = player.hasSawblade ? sawbladeReadyIndicatorColor : sawbladeFiredIndicatorColor;
        
        // TODO: Health Code
        energyGauge.rectTransform.sizeDelta = energyGaugeSize;
        energyCount.text = energyGaugeCount;
        sawbladeReadyIndicator.color = sawbladeCurrentIndicatorColor;
	}
}
