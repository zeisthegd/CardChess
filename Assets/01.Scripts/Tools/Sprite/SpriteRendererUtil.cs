using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRendererUtil
{
    public static IEnumerator Flicker(SpriteRenderer spriteRenderer, Color color, float duration, float interval = 0.05F)
    {
        float _duration = 0;
        Color baseColor = spriteRenderer.color;
        while (_duration < duration)
        {
            _duration += Time.deltaTime + interval * 2;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(interval);
            spriteRenderer.color = baseColor;
            yield return new WaitForSeconds(interval);
        }
        spriteRenderer.color = baseColor;
    }

    /// <summary>
    /// Returns corners of a sprite in order [topRight, botRight, botLeft, topLeft]
    /// </summary>
    public static Vector3[] GetLocalSpriteCorners(SpriteRenderer renderer)
    {
        Vector3 topRight = renderer.sprite.bounds.max;
        Vector3 topLeft = new Vector3(renderer.sprite.bounds.max.x, renderer.sprite.bounds.min.y, 0);
        Vector3 botLeft = renderer.sprite.bounds.min;
        Vector3 botRight = new Vector3(renderer.sprite.bounds.min.x, renderer.sprite.bounds.max.y, 0);
        return new Vector3[] { topRight, botRight, botLeft, topLeft };
    }
}
