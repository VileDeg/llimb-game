using System.Collections;
using UnityEngine;

public class NoiseCircle : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public void Initialize(float radius, float fadeDuration)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the scale of the circle to match the radius
        transform.localScale = new Vector3(radius * 2, radius * 2, 1);

        // Set the initial transparency to 50% alpha
        Color initialColor = _spriteRenderer.color;
        _spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0.5f);

        // Start fading out
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float duration)
    {
        float elapsedTime = 0f;
        Color initialColor = _spriteRenderer.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Gradually decrease the alpha value
            float alpha = Mathf.Lerp(0.1f, 0f, elapsedTime / duration);
            _spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

            yield return null;
        }

        Destroy(gameObject); // Destroy the circle after fading out
    }
}
