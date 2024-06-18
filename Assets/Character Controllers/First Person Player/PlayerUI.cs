using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public PlayerController player;

    public bool showStatBars;
    public GameObject statBarParent;

    public Slider healthBar;
    public float oldHealth;
    public Slider staminaBar;
    public Slider oxygenBar;
    public Slider batteryBar;

    public TextPopUp popUp;

    public TextMeshProUGUI interactionText;

    public float vignetteIntensity = 0.4f;

    //public Volume postProcessingVolume;
    public float damageEffectDecreaseRate = 0.4f;
    public Volume damagePostProcessing;
    public Volume drowningPostProcessing;


    private void Start()
    {
        player = GetComponent<PlayerController>();

        batteryBar.gameObject.SetActive(false);
        oxygenBar.gameObject.SetActive(false);

        healthBar.maxValue = player.maxHealth;
        staminaBar.maxValue = player.maxStamina;
        oxygenBar.maxValue = player.maxOxygen;

        if (!showStatBars)

            statBarParent.SetActive(false);

        else
            statBarParent.SetActive(true);
    }

    private void Update()
    {
        healthBar.value = player.currentHealth;
        staminaBar.value = player.stamina;

        if (player.isTakingDamage)
        {
            //StartCoroutine(TakeDamageUI());
            damagePostProcessing.weight = 1f;
            Debug.Log("Damage Effect Active");
        }

        if (damagePostProcessing.weight > 0f)
        {
            damagePostProcessing.weight -= Time.deltaTime * damageEffectDecreaseRate;

        }
        else player.isTakingDamage = false;

        if (player.isSwimming || player.currentOxygen < player.maxOxygen)
        {
            if (showStatBars)
                oxygenBar.gameObject.SetActive(true);

            oxygenBar.value = player.currentOxygen;

            drowningPostProcessing.weight = Mathf.Lerp(1, 0, oxygenBar.value * 0.1f);

            if (oxygenBar.value <= oxygenBar.maxValue * 0.4f)
            {
                //popUp.SetAndDisplayPopUp("Warning: Low Oxygen");
                //StartCoroutine(DrowningUI());
            }

            if (oxygenBar.value <= 0f)
            {
                popUp.SetAndDisplayPopUp("Out of Oxygen");

                //StartCoroutine(DrowningUI());
            }
        }
        else oxygenBar.gameObject.SetActive(false);

        if (player.canClimb && !player.isClimbing)
        {

        }
    }

    public void InitialiseBatteryBar(InventoryItem item)
    {
        batteryBar.maxValue = item.item.maxBatteryCharge;
        batteryBar.value = item.batteryCharge;
    }

}
