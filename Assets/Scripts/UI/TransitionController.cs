using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour
{
    private readonly int _circleSizeId = Shader.PropertyToID("_Circle_Size");

    private Image _image;
    private Animator animator;
    public float circleSize = 0;

    public CanvasGroup loadingFigure;

    private void Awake()
    {
        _image = gameObject.GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _image.materialForRendering.SetFloat(_circleSizeId, circleSize);
    }

    public void TransitionIn()
    {
        animator.SetTrigger("In");
    }

    public void TransitionOut()
    {
        animator.SetTrigger("Out");
    }

    public void ShowLoading(bool show)
    {
        StartCoroutine(ShowLoadingCoroutine(show));
    }

    private IEnumerator ShowLoadingCoroutine(bool show)
    {
        yield return new WaitForEndOfFrame();
        loadingFigure.gameObject.SetActive(show);
    }
}
