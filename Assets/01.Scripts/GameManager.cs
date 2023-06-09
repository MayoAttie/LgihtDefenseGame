﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum eSTATE
    {
        READY = 0,
        START,
        PLAY,
        END,
        RESULT
    }

    enum eSTAGE
    {
        NONE = 0,
        LEVEL1,
        LEVEL2,
        LEVEL3, LEVEL4, LEVEL5,
        EndLevel
    }

    [SerializeField] eSTATE _eState;
    [SerializeField] eSTAGE eStage;

    private bool isCoroutineFlag = false;
    public bool isPlaying = false;
    //몬스터 생산 제어 플래그
    private bool MonstercontrolFleg = false;
    private static GameManager _instance = null;
    public bool isEnding = false;

    //스테이지 텍스트 관리 플래그
    private bool StageBgrFlag = false;

    [SerializeField] private Image set_Text_Bgr;
    [SerializeField] private Text set_StateText;
    [SerializeField] private GameObject player;
    //몬스터 생성 필드
    [SerializeField] Transform monster_CreateField;

    //골드 표시
    [SerializeField] private Image img_Gold_Bag;
    [SerializeField] private Text txt_Gold_Text;
    //몬스터 숫자
    [SerializeField] private Image img_MonsterNumber;
    [SerializeField] private Text txt_MonsterNumber;

    //엔드 메시지
    [SerializeField] private Image img_EndBag;
    [SerializeField] private Text txt_EndBag;

    //아이템샵버튼
    [SerializeField] private Button Shopbutton;


    //몬스터 카운트용
    private int nMonsterTotalCount = 0;
    private int nMonsterCount = 0;

    //골드 저장 변수
    private int nGold=0;

    //타이머
    float timer = 0;

    //승리 체크
    [SerializeField]bool isVictory =false;
    [SerializeField]bool isFaild = false;

    //커서체크
    public bool isCursor = true;


    private void Awake()
    {
        _eState = eSTATE.READY;
        eStage= eSTAGE.NONE;

        if (_instance == null)
        {
            //이 클래스 인스턴스가 탄생했을 때, 전역변수 _instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            _instance = this;
            //씬이 전환되어도 파괴되지 않도록 한다.
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //씬 이동이 되었는데, 그 씬에도 GameManager가 존재하는 경우에는, 기존 인스턴스를 사용하도록 한다.
            Destroy(this.gameObject);
        }
    }

    //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티, static이므로 다른 클래스에서 호출할 수 있다.
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isEnding = isGameOver();
        if(isCursor && isPlaying)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            isCursor= false;
        }
        

        switch (_eState)
        {
            case eSTATE.READY:
                GameReady();
                break;
            case eSTATE.START:
                GameStart();
                break;
            case eSTATE.PLAY:
                GamePlay();
                break;
            case eSTATE.END:
                if(!isVictory) GameEnd();
                else GameVictory();
                break;
            case eSTATE.RESULT:
                EndScreenInput();
                break;
        }
        ExitInput();
    }

    // State와 관련된 함수.
    void GameReady()
    {
        _eState = eSTATE.START;
        img_Gold_Bag.gameObject.SetActive(false);
        txt_Gold_Text.gameObject.SetActive(false);
        img_MonsterNumber.gameObject.SetActive(false);
        txt_MonsterNumber.gameObject.SetActive(false);
        img_EndBag.gameObject.SetActive(false);
        txt_EndBag.gameObject.SetActive(false);
    }
    void GameStart()
    {

        StartCoroutine(CountDown());
        
    }

    void GamePlay()
    {
        if(!StageBgrFlag)
        {
            set_StateText.gameObject.SetActive(false);
            set_Text_Bgr.gameObject.SetActive(false);

        }


        img_Gold_Bag.gameObject.SetActive(true);
        txt_Gold_Text.gameObject.SetActive(true);
        img_MonsterNumber.gameObject.SetActive(true);
        txt_MonsterNumber.gameObject.SetActive(true);

        //몬스터 숫자 카운트
        MonsterNumberPrint();


        txt_Gold_Text.text = "Gold : " + nGold;

        isPlaying = true;

        if (isEnding)   //엔딩 확인
        {
            _eState = eSTATE.END;
        }

        switch (eStage)
        {
            case eSTAGE.NONE:
                eStage = eSTAGE.LEVEL1;
                break;
            case eSTAGE.LEVEL1:
                Stage_1();
                break;
            case eSTAGE.LEVEL2:
                Stage_2();
                break;
            case eSTAGE.LEVEL3:
                Stage_3();
                break;
            case eSTAGE.LEVEL4:
                Stage_4();
                break;
            case eSTAGE.LEVEL5:
                Stage_5();
                break;
            case eSTAGE.EndLevel:
                End_Level();
                break;
        }

    }
    void GameEnd()
    {
        set_Text_Bgr.gameObject.SetActive(true);
        set_StateText.gameObject.SetActive(true);
        set_StateText.fontSize = 130;
        set_StateText.color = Color.red;
        set_StateText.text = "G A M E  O V E R!";
        
        txt_Gold_Text.gameObject.SetActive(false) ;
        img_Gold_Bag.gameObject .SetActive(false);
        txt_MonsterNumber.gameObject.SetActive(false);
        img_MonsterNumber.gameObject.SetActive(false);
        Shopbutton.gameObject.SetActive(false);

        isPlaying = false;


        Invoke("GameResult", 3);

    }

    void GameVictory()
    {
        set_Text_Bgr.gameObject.SetActive(true);
        set_StateText.gameObject.SetActive(true);
        set_StateText.fontSize= 130;
        set_StateText.color = new Color(0.529f, 1f, 0.706f);
        set_StateText.text = "G A M E  C L E A R!";

        txt_Gold_Text.gameObject.SetActive(false);
        img_Gold_Bag.gameObject.SetActive(false);
        txt_MonsterNumber.gameObject.SetActive(false);
        img_MonsterNumber.gameObject.SetActive(false);
        Shopbutton.gameObject.SetActive(false);


        isPlaying = false;


        Invoke("GameResult", 3);
    }

    void GameResult()
    {
        _eState = eSTATE.RESULT;
        int score = 0;
        score = nMonsterTotalCount - nMonsterCount;
        set_StateText.fontSize = 130;
        set_StateText.color = Color.blue;
        set_StateText.text = "파괴한 몬스터 수 : " + score;

        img_EndBag.gameObject.SetActive(true);
        txt_EndBag.gameObject.SetActive(true);

    }

#if UNITY_EDITOR

    //Stage와 관련된 함수.
    void Stage_1()
    {


        //GameObject[] enemys = GameObject.FindGameObjectsWithTag("level1_Mon");
        //StartCoroutine(MonsterTransPosition(enemys));
        StartCoroutine(MonsterCreate("level1_Mon", eSTAGE.LEVEL2, 2, 1f, false));


    }
    void Stage_2()
    {
        StartCoroutine(MonsterCreate("level2_Mon", eSTAGE.LEVEL3, 4, 1f, false));
    }
    void Stage_3()
    {
        StartCoroutine(MonsterCreate("level3_Mon", eSTAGE.LEVEL4, 4, 1f, false));

    }
    void Stage_4()
    {
        StartCoroutine(MonsterCreate("level4_Mon", eSTAGE.LEVEL5, 4, 1f, false));

    }
    void Stage_5()
    {
        StartCoroutine(MonsterCreate("level5_Mon", eSTAGE.EndLevel, 4, 1f, true));

    }
    void End_Level()
    {
        if(nMonsterCount == 0)
        {
            Invoke("FinalBossStage", 5f);

            if(isVictory)
            {
                isGameOver();
            }
        }
    }


#else
void Stage_1()
    {


        //GameObject[] enemys = GameObject.FindGameObjectsWithTag("level1_Mon");
        //StartCoroutine(MonsterTransPosition(enemys));
        StartCoroutine(MonsterCreate("level1_Mon", eSTAGE.LEVEL2, 30, 1.5f, false));


    }
    void Stage_2()
    {
        StartCoroutine(MonsterCreate("level2_Mon", eSTAGE.LEVEL3, 30, 1.7f, false));
    }
    void Stage_3()
    {
        StartCoroutine(MonsterCreate("level3_Mon", eSTAGE.LEVEL4, 30, 1.8f, false));

    }
    void Stage_4()
    {
        StartCoroutine(MonsterCreate("level4_Mon", eSTAGE.LEVEL5, 30, 1.7f, false));

    }
    void Stage_5()
    {
        StartCoroutine(MonsterCreate("level5_Mon", eSTAGE.EndLevel, 30, 1.8f, true));

    }
    void End_Level()
    {
        if(nMonsterCount == 0)
        {
            Invoke("FinalBossStage", 5f);

            if(isVictory)
            {
                isGameOver();
            }
        }
    }


#endif


    public void setVictory(bool victory)
    {
        isVictory = victory;
    }

    public void setFailed(bool faile)
    {
        isFaild= faile;
    }


    IEnumerator MonsterCreate(string enemyTag, eSTAGE nextLevel, int MonsterNumber, float CreateTimer, bool isBoss)
    {

        if(!MonstercontrolFleg)
        {
            //몬스터 생성
            for(int i =0; i < MonsterNumber; i++)
            {

                MonstercontrolFleg = true;

                GameObject enemy = Instantiate(GameObject.FindGameObjectWithTag(enemyTag));
                enemy.transform.position = monster_CreateField.position;
                nMonsterCount++;
                nMonsterTotalCount++;
                yield return new WaitForSecondsRealtime(CreateTimer);

                MonstercontrolFleg = false;

            }

            //일정 시간 이후, 다음 스테이지 부르기.
            if (!MonstercontrolFleg)
            {
                MonstercontrolFleg = true;

#if UNITY_EDITOR    //에디터 환경 nextLvel Stage 시간초 5, 다른 플랫폼, 30초

                if(!isBoss)
                {
                    StageBgrFlag = true;
                    set_Text_Bgr.gameObject.SetActive(true);
                    set_StateText.gameObject.SetActive(true);
                    set_StateText.fontSize = 130;
                    set_StateText.color = new Color(0.529f, 2f, 0.706f);
                    set_StateText.text = "1.5초 후 다음 스테이지";
                    Invoke("StageBgrSetFlag", 1.5f);
                    yield return new WaitForSecondsRealtime(5);
                }
                else
                {
                    StageBgrFlag = true;
                    set_Text_Bgr.gameObject.SetActive(true);
                    set_StateText.gameObject.SetActive(true);
                    set_StateText.fontSize = 45;
                    set_StateText.color = Color.red;
                    set_StateText.text = "모든 몬스터 파괴 시, 5초 후 보스 스테이지.\n";
                    set_StateText.text += "보스몹은 한 바퀴 내에 잡아야 합니다.";
                    Invoke("StageBgrSetFlag", 2.5f);
                }


#else
                if(!isBoss)
                {
                    StageBgrFlag = true;
                    set_Text_Bgr.gameObject.SetActive(true);
                    set_StateText.gameObject.SetActive(true);
                    set_StateText.text = "12초 후 다음 스테이지";
                    Invoke("StageBgrSetFlag", 1.5f);
                    yield return new WaitForSecondsRealtime(12);
                }
                else
                {
                    StageBgrFlag = true;
                    set_Text_Bgr.gameObject.SetActive(true);
                    set_StateText.gameObject.SetActive(true);
                    set_StateText.fontSize = 45;
                    set_StateText.color = Color.red;
                    set_StateText.text = "모든 몬스터 파괴 시, 5초 후 보스 스테이지.\n";
                    set_StateText.text += "보스몹은 한 바퀴 내에 잡아야 합니다.";
                    Invoke("StageBgrSetFlag", 2.5f);
                }
#endif
                eStage = nextLevel;
                MonstercontrolFleg = false;
                yield break;

            }

        }
    }

    private void FinalBossStage()
    {
        //보스몬스터는 한 개체만 나와야 하기 때문에, 플래그 true
        if(!MonstercontrolFleg)
        {
            MonstercontrolFleg= true;

            GameObject enemy = Instantiate(GameObject.FindGameObjectWithTag("Boss"));
            enemy.transform.position = monster_CreateField.position;
            nMonsterCount++;
            nMonsterTotalCount++;
            
        }
    }

 
    void StageBgrSetFlag()
    {
        StageBgrFlag = false;
    }


    IEnumerator CountDown()
    {
        if(!isCoroutineFlag)
        {
            isCoroutineFlag= true;

#if UNITY_EDITOR
            for (int i = 1; i >= 1; i--)
            {
                set_StateText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1.0f);

            }
            isCoroutineFlag = false;
            set_StateText.color = new Color(0.529f, 1f, 0.706f);
            set_StateText.text = " Game Start!";
            _eState = eSTATE.PLAY;
            yield break;

#else
            for(int i=3; i>=1; i--)
            {
                set_StateText.text = i.ToString();
                yield return new WaitForSecondsRealtime(1.0f);

            }
            isCoroutineFlag = false;
            set_StateText.text = " Game Start!";
            _eState = eSTATE.PLAY;
            yield break;

#endif


        }
    }

    void ExitInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader._instance._LoadScene("UI_Scene");
        }
    }

    //골드 setter
    public void IncomeGold(int gold)
    {
        nGold+=gold;
    }
    public void setGold(int gold)
    {
        nGold=gold;
    }
    public int getGold()
    {
        return nGold;
    }
    void MonsterNumberPrint()
    {

        txt_MonsterNumber.text = "Monster : " + nMonsterCount.ToString();
    }

    public void setMonsterCount(int nMonsterCount)
    {
        this.nMonsterCount -= nMonsterCount;
    }


    bool isGameOver()
    {
        if(isVictory)
        {
            return true;
        }
        if(isFaild)
        {
            return true;
        }
        
        if (nMonsterCount >= 15)
        {
            return true;
        }
        else
            return false;
    }

    void EndScreenInput()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Application.Quit();
        }
        else if(Input.GetKeyDown(KeyCode.Return)) 
        {
            SceneLoader._instance._LoadScene("UI_Scene");
        }


    }

}
