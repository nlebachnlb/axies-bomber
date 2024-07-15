using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressToContinue : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            AppRoot.Instance.TransitionToScene("Home", true);
            enabled = false;
        }
    }
}
