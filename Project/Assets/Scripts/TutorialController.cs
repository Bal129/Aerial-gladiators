using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] RectTransform[] infoContents;

    private int currentContent;

    private void Start()
    {
        currentContent = 0;
        UpdateContents();
    }

    public void PreviousContent()
    {
        currentContent = Mathf.Clamp(currentContent - 1, 0, infoContents.Length - 1);
        UpdateContents();
    }

    public void NextContent()
    {
        currentContent = Mathf.Clamp(currentContent + 1, 0, infoContents.Length - 1);
        UpdateContents();
    }

    private void ResetContents()
    {
        foreach (RectTransform contents in infoContents)
        {
            contents.gameObject.SetActive(false);
        }
    }

    private void UpdateContents()
    {
        ResetContents();
        infoContents[currentContent].gameObject.SetActive(true);
    }
}
