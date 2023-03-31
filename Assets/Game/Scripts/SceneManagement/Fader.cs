using System.Collections;
using UnityEngine;


namespace RPG.SceneManagement 
{
    public class Fader : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;

        public IEnumerator DoSceneFade(float fadeDuration)
        {
            yield return FadeOut(fadeDuration);
            yield return FadeIn(fadeDuration);
        }


        public IEnumerator FadeOut(float duration)
        {
            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / duration;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float duration)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / duration;
                yield return null;
            }
        }
    }
}

