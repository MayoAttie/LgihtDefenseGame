using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    static SceneLoader _unique;
    public Text loadingText;
    [SerializeField] Image loadingBar;
    string nextScene;

    //캔버스 우선순위 조절
    public Canvas canvas;

    public enum eLoaddingState
    {
        None = 0,
        Start,
        ing,
        end
    }
    //AsyncOperation loadProc;
    eLoaddingState _nowLoadState = eLoaddingState.None;

    static public SceneLoader _instance
    {
        get { return _unique; }
    }


    // Start is called before the first frame update
    private void Awake()
    {
        _unique= this;
        DontDestroyOnLoad(this.gameObject);
        _LoadScene("UI_Scene");
    }
    private void Update()
    {
        switch(_nowLoadState)
        {
            case eLoaddingState.None:
                canvas.sortingOrder = 0;
                break;
            case eLoaddingState.Start:
                canvas.sortingOrder = 10;
                canvas.enabled= true;
                ShowLoadingText();
                break;
            case eLoaddingState.ing:
                canvas.enabled = true;
                canvas.sortingOrder = 10;
                ShowLoadingText();
                break;
            case eLoaddingState.end:
                canvas.sortingOrder = 0;
                canvas.enabled = false;
                ClearLoadingText();
                break;
        }
    }


    //문자열을 입력받아 씬을 넘겨주는 함수
    public void _LoadScene(string SceneName)
    {
        _nowLoadState = eLoaddingState.Start;
        nextScene = SceneName;

        StartCoroutine(LoadSceneProcess());


    }

    private void ShowLoadingText()
    {
        //로딩상태 표시
        loadingText.gameObject.SetActive(true);
    }

    private void ClearLoadingText()
    {
        //로딩상태 제거
        loadingText.gameObject.SetActive(false);
    }


    IEnumerator LoadSceneProcess()
    {
        _nowLoadState = eLoaddingState.ing;
        AsyncOperation obj = SceneManager.LoadSceneAsync(nextScene);


        //로딩이 99에서 멈춤
        obj.allowSceneActivation= false;

        float timer = 0f;
        while(!obj.isDone)
        {
            yield return null;


            if (obj.progress <0.9f)
            {   //실제 로딩
                //바 표기
                loadingBar.fillAmount = obj.progress;


                //텍스트표기
                int percent = Mathf.RoundToInt(obj.progress * 100);
                loadingText.text = $"{percent}%";

            }
            else
            {   //페이크 로딩
                timer += Time.unscaledDeltaTime;


                //텍스트표기
                float percent = Mathf.RoundToInt(Mathf.Lerp(0.9f, 1f, timer)*100);
                loadingText.text = $"{percent}%";


                loadingBar.fillAmount = Mathf.Lerp(0.9f,1f,timer);
                if(loadingBar.fillAmount >=1f)
                {
                    obj.allowSceneActivation= true;
                }
            }
        }

        _nowLoadState = eLoaddingState.end;

    }
}
