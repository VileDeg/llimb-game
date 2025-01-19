using UnityEngine;

public class MedPack : MonoBehaviour
{
    [SerializeField]
    private float _healAmount = 10f;

    public float GetHealAmount()
    {
        return _healAmount;
    }
}