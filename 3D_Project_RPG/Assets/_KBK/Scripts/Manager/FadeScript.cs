using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{

    public float fadeTime = 2f;

    Image fadeImg;
    float start = 0f;
    float end = 1f;
    float currTime;

    bool isPlaying;
    
    public void StartFadeInAnim(bool autoInOut = false)
    {
        if (isPlaying) return;
        fadeImg = GetComponentInChildren<Image>();
        StartCoroutine("FadeInAnim");
        if(autoInOut)
        {
            Invoke("StartFadeOutAnim", fadeTime);
        }
    }

    public void StartFadeOutAnim()
    {
        if (isPlaying) return;

        StartCoroutine("FadeOutAnim");
    }

    IEnumerator FadeInAnim()
    {
        isPlaying = true;

        Color fadeColor = fadeImg.color;
        currTime = 0f;

        while (fadeColor.a < 1f)
        {
            currTime += Time.deltaTime * fadeTime;

            fadeColor.a = Mathf.Lerp(start, end, currTime);
            fadeImg.color = fadeColor;

            yield return null;
        }

        isPlaying = false;
    }

    IEnumerator FadeOutAnim()
    {
        isPlaying = true;

        Color fadeColor = fadeImg.color;
        currTime = 0f;

        while (fadeColor.a >= 0f)
        {
            currTime += Time.deltaTime * fadeTime;

            fadeColor.a = Mathf.Lerp(end, start, currTime);
            fadeImg.color = fadeColor;

            yield return null;
        }

        isPlaying = false;
    }
}
