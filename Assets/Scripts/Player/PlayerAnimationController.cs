using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator _animator;

    //public static event Action OnPlayerMeleeStrikeAnimationComplete;

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

    //void Start()
    //{
    //    StartCoroutine(WaitForAnimationEnd());
    //}

    // Wait for the animation to end before starting the game
    //private IEnumerator WaitForAnimationEnd()
    //{
    //    // Get the length of the animation clip from the Animator
    //    AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
    //    float animationDuration = stateInfo.length;

    //    // Wait for the animation to finish
    //    yield return new WaitForSeconds(animationDuration);

    //    OnPlayerMeleeStrikeAnimationComplete?.Invoke();
    //}


    private void OnMeleeStrikePressedHandler()
    {
        LogUtil.Info("OnMeleeStrikePressedHandler");
        // Play animation and restart from the beginning.
        _animator.SetTrigger("MeleeStrike");
    }

}
