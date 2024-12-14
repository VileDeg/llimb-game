using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public Slider slider;
    public Image fillImage; // Reference to the Fill Image of the slider

    public void SetCharge(float charge)
    {
        slider.value = charge;
    }

    public void SetMaxCharge(float maxCharge)
    {
        slider.maxValue = maxCharge;
        slider.value = 0;
    }

    public void SetFillColor(Color color)
    {
        if (fillImage != null)
        {
            fillImage.color = color;
        }
    }
}
