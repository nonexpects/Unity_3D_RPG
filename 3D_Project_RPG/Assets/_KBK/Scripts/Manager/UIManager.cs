using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    PlayerController player;
    public Image hpBar;
    public Image expBar;
    public Text lvText;
    //public Text posText;

    bool playerDead;

    GameObject p;

    //플레이어 죽음
    public GameObject dieScene;
    Image dieImage;
    TextMeshProUGUI dietext;

    // Start is called before the first frame update
    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        dieImage = dieScene.GetComponent<Image>();
        dietext = dieScene.GetComponentInChildren<TextMeshProUGUI>();

        PlayerController.OnPlayerDead += PlayerDeadScene;

        dieScene.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = player.PlayerHp / player.PlayerMaxHp;
        expBar.fillAmount = player.PlayerExp / player.PlayerMaxExp;
        lvText.text = "LV " + player.PlayerLv;
        //posText.text = "x : " + p.transform.position.x + " y : " + p.transform.position.y + " z : " + p.transform.position.z;
    }

    public void PlayerDeadScene()
    {
        dieScene.SetActive(true);
        Invoke("UppingText", 5f);
        StartCoroutine(ChangeAlpha());
        StartCoroutine(ChangeText());
    }

    IEnumerator ChangeAlpha()
    {
        while (dieImage.color.a <= 1f)
        {
            Color alpha = dieImage.color;
            alpha.a += 0.1f;
            dieImage.color = alpha;

            dietext.fontSize++;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    IEnumerator ChangeText()
    {
        while (dietext.fontSize < 100f)
        {
            dietext.characterSpacing += 0.1f;
            dietext.fontSize += (10 * Time.deltaTime);
            yield return new WaitForSeconds(0.1f);
        }

    }

    void UppingText()
    {
        StartCoroutine(UpText());
    }

    IEnumerator UpText()
    {
        Vector3 wantedPos = new Vector3(dietext.transform.localPosition.x, 200, dietext.transform.localPosition.z);
        while (dietext.transform.localPosition.y < 200)
        {
            Vector3 pos = dietext.transform.localPosition;
            pos += Vector3.Lerp(pos, wantedPos, Time.deltaTime * 0.5f);
            dietext.transform.localPosition = pos;

            yield return null;
        }
    }
}
