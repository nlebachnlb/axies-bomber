using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppRoot : MonoBehaviour
{
    public static AppRoot Instance { get; private set; }
    public TransitionController transitionController;
    public AppRootConfig Config { get => config; }
    public UserDataModel UserDataModel { get; private set; }

    [SerializeField] private AppRootConfig config;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        UserDataModel = GetComponent<UserDataModel>();
    }

    private void Start()
    {
        TransitionToScene(config.startSceneName);
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
