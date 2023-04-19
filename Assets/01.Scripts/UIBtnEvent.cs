using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBtnEvent : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    //클릭 이벤트 -> SceneLoader의 _LoadScene함수를 이용하여, 다음 씬을 호출한다
    public void OnClick()
    {

        //SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
        //sceneLoader._LoadScene("IngameScene");

        SceneLoader._instance._LoadScene("IngameScene");
    }

    public void OnClick_HowToGame()
    {
        SceneLoader._instance._LoadScene("MenualScene");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
