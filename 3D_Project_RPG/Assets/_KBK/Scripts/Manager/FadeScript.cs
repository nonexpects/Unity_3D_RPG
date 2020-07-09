using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    Image img;
    
    float start = 0f;
    float end = 1f;
    float currTime;

    bool isPlaying;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void FadeInOut(float time)
    {
        if (isPlaying) return;

        StartCoroutine("FadeInAnim");

        Invoke("StartFadeOutAnim", time);
    }
    

    public void FadeIn(float time)
    {
        if (isPlaying) return;

        StartCoroutine("FadeInAnim", time);
    }

    public void FadeOut(float time)
    {
        if (isPlaying) return;

        StartCoroutine("FadeOutAnim", time);
    }

    IEnumerator FadeInAnim(float time)
    {
        isPlaying = true;

        Color fadeColor = img.color;
        fadeColor.a = 0f;
        currTime = 0f;

        while (fadeColor.a < 1f)
        {
            currTime += Time.deltaTime * time;

            fadeColor.a = Mathf.Lerp(start, end, currTime);
            img.color = fadeColor;

            yield return null;
        }

        isPlaying = false;
    }

    IEnumerator FadeOutAnim(float time)
    {
        isPlaying = true;


        Color fadeColor = img.color;
        fadeColor.a = 1f;
        currTime = 0f;

        while (fadeColor.a > 0f)
        {
            currTime += Time.deltaTime * time;

            fadeColor.a = Mathf.Lerp(end, start, currTime);
            img.color = fadeColor;

            yield return null;
        }

        isPlaying = false;
    }
}
