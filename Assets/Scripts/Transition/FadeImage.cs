using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeImage : MonoBehaviour
{

    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    public IEnumerator FadeOutIn(System.Action onMidFade) // Coroutina function. Can wait for another frame and then continue
                                                          // System.Action - Callback. "After the loop is complet, make this ... "
    { 
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null; // Make function wait for another frame.
        }

        onMidFade?.Invoke(); // Function for teleport

        timer = fadeDuration;
        while (timer > 0)
        {   
            timer -= Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
    }
}
