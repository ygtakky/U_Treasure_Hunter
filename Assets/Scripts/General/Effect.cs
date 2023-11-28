using System;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
