using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbDestructable : ADestructable
{
    [SerializeField]
    private float _lifetimeSeconds = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DieAfterTime(_lifetimeSeconds));
    }

    private IEnumerator DieAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Die();
    }
}
