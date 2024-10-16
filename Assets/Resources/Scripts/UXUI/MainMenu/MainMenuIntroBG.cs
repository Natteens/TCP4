using UnityEngine;
//using DG.Tweening;

public class MainMenuIntroBG : MonoBehaviour
{
    #region Variaveis 

    // Variáveis para a animação do portão
    [Header("Camada do portão")]
    [Space(5f)]
    [SerializeField] private GameObject DoorBG;
    [SerializeField] private float doorInitialY = -10f;
    [SerializeField] private float doorTargetY = 10f;
    [SerializeField] private float doorDuration = 1f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private float shakeDuration = 0.5f;

    // Variáveis para a animação da camada de personagens
    [Header("Camada dos personagens")]
    [Space(5f)]
    [SerializeField] private GameObject LyskBG;
    [SerializeField] private GameObject EnemysBG;
    [SerializeField] private float charactersScale = 1.2f;
    [SerializeField] private float charactersDuration = 0.5f;
    [SerializeField] private float charactersStartDelay = 0.2f;

    // Variáveis para a animação do logo
    [Header("Camada da Logo")]
    [Space(5f)]
    [SerializeField] private GameObject LogoBG;
    [SerializeField] private float logoInitialY = 5f;
    [SerializeField] private float logoTargetY = 0f;
    [SerializeField] private float logoDuration = 0.5f;
    [SerializeField] private float logoStartDelay = 1f;

    // Variaveis para animação do texto para jogar
    [Header("Camada do texto animado")]
    [Space(5f)]
    [SerializeField] private GameObject TextForPlay;
    [SerializeField] private float scaleAmount = 1.2f;
    [SerializeField] private float animationDuration = 1f;
    #endregion

    [SerializeField] private SceneControllerManager sceneController;

    private void Start()
    {
        IntroMainMenu();
    }

    public void IntroMainMenu()
    {
        MoveLogoUp();
        ShakeDoor();
        AnimateDoor();
        AnimateCharacters();
        AnimateLogo();
    }

    private void MoveLogoUp()
    {
        LogoBG.transform.position = new Vector3(0, logoInitialY, 160);
    }

    private void ShakeDoor()
    {
     //   DoorBG.transform.DOShakePosition(shakeDuration, strength: shakeStrength, vibrato: 10, randomness: 90);
    }

    private void AnimateDoor()
    {
     //   DoorBG.transform.DOMoveY(doorTargetY, doorDuration).From(doorInitialY).SetEase(Ease.OutQuad);
    }

    private void AnimateCharacters()
    {
      //  LyskBG.transform.DOScale(Vector3.one, charactersDuration).From(Vector3.one * charactersScale).SetEase(Ease.OutBack).SetDelay(charactersStartDelay);
     //   EnemysBG.transform.DOScale(Vector3.one, charactersDuration).From(Vector3.one * charactersScale).SetEase(Ease.OutBack).SetDelay(charactersStartDelay);
    }

    private void AnimateLogo()
    {
     //   LogoBG.transform.DOMoveY(logoTargetY, logoDuration * 0.8f)
         //   .From(logoInitialY)
           // .SetEase(Ease.OutBack)
        //    .SetDelay(logoStartDelay)
        //    .OnComplete(() =>
       //     {
      //          PressPlayAnim();
      //      });
    }

    private void PressPlayAnim()
    {
     //   TextForPlay.SetActive(true);
     //   TextForPlay.transform.localScale = Vector3.one;
     //   TextForPlay.transform.DOScale(scaleAmount, animationDuration)
     //       .SetEase(Ease.InOutSine)
     //       .SetLoops(-1, LoopType.Yoyo);
     //
     //   GameEventsMenu.EnableInput();
    }

    private void OnPlayerPressToPlay()
    {
        TextForPlay.SetActive(false);
        sceneController.StartSceneLoad("MainMenu", SceneControllerManager.SceneLoadMode.Async, SceneControllerManager.SceneType.Normal);
    }


    private void OnEnable()
    {
       GameEventsMenu.OnClickForPlayGame += OnPlayerPressToPlay;
        
    }

    private void OnDisable()
    {
      GameEventsMenu.OnClickForPlayGame -= OnPlayerPressToPlay;
    }
}