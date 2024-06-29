using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Image imageSplash;
    [SerializeField] float flashDuration;
    [SerializeField] float fadeDuration;

    [Header("Flash Colors")]
    [SerializeField] Color characterHit;
    [SerializeField] Color enemyDied;

    public void FlashCharacterHit()
    {
        StartCoroutine(FlashScreenSequences(characterHit));
    }

    public void FlashEnemyDied()
    {
        StartCoroutine(FlashScreenSequences(enemyDied));
    }

    private IEnumerator FlashScreenSequences(Color color)
    {
        // sequences to flash screen
        // 1. change color of image depends on the situation
        // 2. activate image
        // 3. wait for a certain duration
        // 4. decreases the opacity overtime
        // 5. deactivate image

        imageSplash.color = new Color(color.r, color.g, color.b);
        imageSplash.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        imageSplash.enabled = false;
    }
}
