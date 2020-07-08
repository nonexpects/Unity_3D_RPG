using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    PlayerController player;
    BossController boss;
    public Image hpBar;
    Image bossHp;
    public Text hpText;
    Text bossHpText;
    public Image expBar;
    public Text goldText;
    public TextMeshProUGUI lvText;
    //public Text posText;

    public GameObject bossHpBar;
    GameObject[] allUI;

    bool playerDead;
    bool bossAppear;

    GameObject p;

    //플레이어 죽음
    public GameObject dieScene;
    Image dieImage;
    TextMeshProUGUI dietext;

    public GameObject bossName;

    // Start is called before the first frame update
    private void Awake()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
    }
    void Start()
    {
        p = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        dieImage = dieScene.GetComponent<Image>();
        dietext = dieScene.GetComponentInChildren<TextMeshProUGUI>();
        allUI = GameObject.FindGameObjectsWithTag("UI");
        bossName.SetActive(false);

        bossHp = bossHpBar.GetComponentsInChildren<Image>()[2];
        bossHpText = bossHpBar.GetComponentInChildren<Text>();

        PlayerController.OnPlayerDead += PlayerDeadScene;

        BossController.BossCamEvent += UIDisappear;

        BossController.BossAppearance += UIAppear;
        BossController.BossAppearance += BossHpBarAppear;

        BossController.BossDead += BossHpBarDisappear;
        
        dieScene.SetActive(false);
        bossHpBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = player.PlayerHp / player.PlayerMaxHp;
        expBar.fillAmount = player.PlayerExp / player.PlayerMaxExp;
        lvText.text = "LV " + player.PlayerLv;
        goldText.text = player.PlayerGold.ToString("000");
        hpText.text = "HP " + player.PlayerHp.ToString() + " / " + player.PlayerMaxHp.ToString();
        if(bossAppear)
        {
            bossHp.fillAmount = boss.currHp / boss.maxHp;
            bossHpText.text = "HP " + boss.currHp.ToString() + " / " + boss.maxHp.ToString();
        }

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

    public void BossHpBarAppear()
    {
        bossHpBar.SetActive(true);
        bossAppear = true;
    }

    public void BossHpBarDisappear()
    {
        bossHpBar.SetActive(false);
        bossAppear = false;
    }

    public void UIDisappear()
    {
        for (int i = 0; i < allUI.Length; i++)
        {
            allUI[i].SetActive(false);
        }

        bossName.SetActive(true);
    }

    public void UIAppear()
    {
        for (int i = 0; i < allUI.Length; i++)
        {
            allUI[i].SetActive(true);
        }

        bossName.SetActive(false);
    }
}
