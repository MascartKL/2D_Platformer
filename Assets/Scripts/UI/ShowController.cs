using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowController : MonoBehaviour
{
    public GameObject androidController;

    void Start()
    {
#if UNITY_EDITOR_WIN
        androidController.SetActive(false);
#endif

#if UNITY_ANDROID
        androidController.SetActive(true);
#endif

    }
}
