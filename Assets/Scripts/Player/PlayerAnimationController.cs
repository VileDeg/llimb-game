using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Player.OnMeleeStrikePressed += OnMeleeStrikePressedHandler;
    }

    private void OnDisable()
    {
        Player.OnMeleeStrikePressed -= OnMeleeStrikePressedHandler;
    }


    private void OnMeleeStrikePressedHandler()
    {
        LogUtil.Info("OnMeleeStrikePressedHandler");
        // Play animation and restart from the beginning.
        _animator.SetTrigger("MeleeStrike");
    }

}
