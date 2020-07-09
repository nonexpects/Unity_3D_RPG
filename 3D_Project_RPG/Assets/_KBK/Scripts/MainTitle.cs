using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    public GameObject trigger;
    public GameObject titleMenu;
    public FadeScript fade;

    float start = 1;
    float end = 0;
    float currTime;

    bool isPlaying;
    
    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            trigger.SetActive(false);
            titleMenu.SetActive(true);
        }
    }

    IEnumerator FadeIn()
    {
        isPlaying = true;

        Color alpha = trigger.GetComponent<Text>().color;

        while (alpha.a > 0f)
        {
            currTime += Time.deltaTime;
            
            alpha.a = Mathf.Lerp(start, end, currTime * 3f);
            trigger.GetComponent<Text>().color = alpha;

            yield return null;

        }

        isPlaying = false;
        currTime = 0f;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        isPlaying = true;

        Color alpha = trigger.GetComponent<Text>().color;

        while (alpha.a < 1f)
        {
            currTime += Time.deltaTime;

            alpha.a = Mathf.Lerp(end, start, currTime * 3f);
            trigger.GetComponent<Text>().color = alpha;

            yield return null;
        }

        isPlaying = false;
        currTime = 0f;

        StartCoroutine(FadeIn());
    }

    public void GameStart()
    {
        fade.FadeIn(5f);
        SceneManager.LoadScene("Level1");
        SceneManager.LoadScene("Play", LoadSceneMode.Additive);
    }
        
}
