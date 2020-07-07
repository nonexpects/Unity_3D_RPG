using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(this);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    Dictionary<string, AudioClip> bgmTable;
    
    AudioSource audioMain;
    AudioSource audioSub; 

    //어트리뷰트
    [Range(0, 1f)]
    public float masterVolume = 1f;
    float volumeMain = 0f;
    float volumeSub = 0f;
    float crossFameTime = 5f;

    void Start()
    {
        //BGM테이블 생성
        bgmTable = new Dictionary<string, AudioClip>();
        //오디오 소스 코드로 추가
        audioMain = gameObject.AddComponent<AudioSource>();
        audioSub = gameObject.AddComponent<AudioSource>();
        
        //오디오 소스 볼륨 0으로 초기화
        audioMain.volume = 0f;
        audioSub.volume = 0f;
    }

    private void Update()
    {
        if (audioMain.isPlaying)
        {
            if (volumeMain < 1f)
            {
                volumeMain += Time.deltaTime / crossFameTime;
                if (volumeMain >= 1f) volumeMain = 1f;
            }
            //서브오디오 볼륨 내리기
            if (volumeSub > 0f)
            {
                volumeSub -= Time.deltaTime / crossFameTime;
                if (volumeSub <= 0f)
                {
                    volumeSub = 0f;
                    //서브오디오 정지
                    audioSub.Stop();
                }
            }
        }

        //볼륨 조정
        audioMain.volume = volumeMain * masterVolume;
        audioSub.volume = volumeSub * masterVolume;
    }

    public void PlayBGM(string bgmName)
    {
        if (bgmTable.ContainsKey(bgmName) == false)
        {
            AudioClip bgm = Resources.Load("BGM/" + bgmName) as AudioClip;
            
            if (bgm == null) return;
            
            bgmTable.Add(bgmName, bgm);
        }
        
        audioMain.clip = bgmTable[bgmName];
        audioMain.Play();

        //볼륨값 세팅
        volumeMain = 1f;
        volumeSub = 0f;
    }

    //bgm 크로스페이드 플레이
    public void CrossFadeBGM(string bgmName, float cfTime = 1f)
    {
        if (bgmTable.ContainsKey(bgmName) == false)
        {
            AudioClip bgm = Resources.Load("BGM/" + bgmName) as AudioClip;
            
            if (bgm == null) return;
            
            bgmTable.Add(bgmName, bgm);
        }

        //크로스페이드 타임
        crossFameTime = cfTime;

        //메인오디오에서 플레이 되고 있는걸 서브오디오로 변경 (switching)
        AudioSource temp = audioMain;
        audioMain = audioSub;
        audioSub = temp;

        //볼륨값 스위칭
        float tempVolume = volumeMain;
        volumeMain = volumeSub;
        volumeSub = tempVolume;
        
        audioMain.clip = bgmTable[bgmName];
        audioMain.Play();
    }

    //일시정지
    public void PauseBGM()
    {
        audioMain.Pause();
    }
    //다시재생
    public void ResumeBGM()
    {
        audioMain.Play();
    }
}
