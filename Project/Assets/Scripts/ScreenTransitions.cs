using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransitions : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] Animator animator;
    [SerializeField] float transitionTime = 1f;

    public void ChangeScene(string nextScene)
    {
        StartCoroutine(FadeOut(nextScene));
    }

    public void PauseGame()
    {
        animator.SetTrigger("Pause");
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        animator.SetTrigger("Continue");
        Time.timeScale = 1;
    }

    public void WinGame()
    {
        animator.SetTrigger("Win");
    }

    public void LoseGame()
    {
        animator.SetTrigger("Lose");
    }

    private IEnumerator FadeOut(string nextScene)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nextScene);
    }
}
