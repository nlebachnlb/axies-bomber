using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppRoot : MonoBehaviour
{
    public static AppRoot Instance { get; private set; }
    public TransitionController transitionController;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        TransitionToScene("Room");
    }

    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(ProgressTransitionToScene(sceneName));
    }

    private IEnumerator ProgressTransitionToScene(string sceneName)
    {
        transitionController.TransitionOut();
        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(sceneName);
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            transitionController.TransitionIn();
        };
    }
}
