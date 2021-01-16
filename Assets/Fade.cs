using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image FadeImage;
    public float AlphaValue;
    public bool TestingFadeIn, TestingFadeOut, TestingInstantFadeIn, TestingInstantFadeOut;
    private void Update()
    {
        if (TestingFadeIn)
        {
            TestingFadeIn = false;
            FadeIn();
        }
        if (TestingFadeOut)
        {
            TestingFadeOut = false;
            FadeOut();
        }
        if (TestingInstantFadeIn)
        {
            TestingInstantFadeIn = false;
            InstantFadeIn();
        }
        if (TestingInstantFadeOut)
        {
            TestingInstantFadeOut = false;
            InstantFadeout();
        }
    }
    public void FadeOut() 
    {
        StartCoroutine(FadeOutTiming());
    }
    public void FadeIn() 
    {
        StartCoroutine(FadeInTiming());
    }
    public void InstantFadeIn() 
    {
        AlphaValue = 1;
        Color NewColor = new Color(0, 0, 0, 1);
        FadeImage.color = NewColor;
    }
    public void InstantFadeout()
    {
        AlphaValue = 0;
        Color NewColor = new Color(0, 0, 0, 0);
        FadeImage.color = NewColor;
    }
    IEnumerator FadeInTiming()
    {
        AlphaValue = AlphaValue + 0.1f;
        Color NewColor = new Color(0, 0, 0, AlphaValue);
        FadeImage.color = NewColor;
        yield return new WaitForSecondsRealtime(0.1f);
        if (AlphaValue < 1)
        {
            StartCoroutine(FadeInTiming());
        }
        else 
        {
            AlphaValue = 1;
        }
    }
    IEnumerator FadeOutTiming()
    {
        AlphaValue = AlphaValue - 0.1f;
        Color NewColor = new Color(0, 0, 0, AlphaValue);
        FadeImage.color = NewColor;
        yield return new WaitForSecondsRealtime(0.1f);
        if (AlphaValue > 0f)
        {
            StartCoroutine(FadeOutTiming());
        }
        else
        {
            AlphaValue = 0;
        }
    }
}
