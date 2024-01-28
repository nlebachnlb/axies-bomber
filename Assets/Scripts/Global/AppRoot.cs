using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppRoot : MonoBehaviour
{
    public enum PlayMode
    {
        Normal,
        TestPlayground
    }
    
    public static AppRoot Instance { get; private set; }
    public TransitionController transitionController;
    public TransitionController fastTransitionController;
    public AppRootConfig Config { get => config; }
    public UserDataModel UserDataModel { get; private set; }
    public SoundManager SoundManager { get; private set; }
    
    public PlayMode Mode => mode;

    [SerializeField] private AppRootConfig config;
    [SerializeField] private PlayMode mode = PlayMode.Normal;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UserDataModel = GetComponent<UserDataModel>();
        SoundManager = GetComponentInChildren<SoundManager>();
    }

    private void Start()
    {
        if (mode == PlayMode.Normal)
        {
            TransitionToScene(config.startSceneName);
            SoundManager.PlayAudio(SoundManager.AudioType.MenuBGMType);
            return;
        }

        SceneManager.LoadSceneAsync(config.homeScene);
    }

    public void TransitionToScene(string sceneName, bool needLoading = false)
    {
        StartCoroutine(ProgressTransitionToScene(sceneName, needLoading));
    }

    private IEnumerator ProgressTransitionToScene(string sceneName, bool needLoading)
    {
        transitionController.TransitionOut();

        if (needLoading)
        {
            yield return new WaitForSeconds(0.75f);
            transitionController.ShowLoading(true);
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadSceneAsync(sceneName);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            transitionController.ShowLoading(false);
            transitionController.TransitionIn();
        };
    }
}
