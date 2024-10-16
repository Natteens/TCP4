using UnityEngine;

public static class LoadingShaderEvent
{
    public static void FadeIn(Material dissolveMaterial, float duration, System.Action onComplete = null)
    {
      // dissolveMaterial.DOFloat(1200f, "_CutoffHeight", duration).OnComplete(() =>
      // {
      //     onComplete?.Invoke();
      // });
    }

    public static void FadeOut(Material dissolveMaterial, float duration, System.Action onComplete = null)
    {
      //  dissolveMaterial.DOFloat(-1200f, "_CutoffHeight", duration).OnComplete(() =>
      //  {
      //      onComplete?.Invoke();
     //   });
    }

    public static void StartLoading(Material dissolveMaterial, float fadeInDuration, float fadeOutDuration, System.Action onComplete = null)
    {
        FadeIn(dissolveMaterial, fadeInDuration, () =>
        {
            FadeOut(dissolveMaterial, fadeOutDuration, onComplete);
        });
    }
}
