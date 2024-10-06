using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpPowerUIScript : MonoBehaviour
{
    [SerializeField]
    private PlayerMoveScript playerMoveScript;
    [SerializeField]
    private Slider chargePowerSlider;
    [SerializeField]
    private Image sliderImage;
    [SerializeField]
    private Color color_1, color_2, color_3, color_4;

    private float chargePower;
    private float chargeRatio;

    private float maxCharge;

    void Update()
    {
        chargePower = playerMoveScript.GetChargeTime();
        maxCharge = playerMoveScript.GetMaxCharge();
        chargeRatio = chargePower / maxCharge;

        if (chargeRatio > 0.75f)
        {
            sliderImage.color = Color.Lerp(color_2, color_1, (chargeRatio - 0.75f) * 4f);
        }
        else if (chargeRatio > 0.25f)
        {
            sliderImage.color = Color.Lerp(color_3, color_2, (chargeRatio - 0.25f) * 4f);
        }
        else
        {
            sliderImage.color = Color.Lerp(color_4, color_3, chargePower * 4f);
        }
        chargePowerSlider.value = chargePower;
    }
}
