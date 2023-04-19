using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ballCreate;
    [SerializeField] GameObject cannonBall;

    GameManager gameMng;

    bool isEnd;
    public int ballPower = 1;

    bool courutineFlag = false;

    void Start()
    {
        gameMng = GameManager.Instance;
    }

    void Update()
    {
        isEnd = gameMng.isEnding;
        if(isEnd)
        {
            Destroy(gameObject);
        }


        StartCoroutine(CannonFire());
    }



    public int BallPowerGetter()
    {
        return ballPower;
    }
    public void BallPowerSetter(int ballPower)
    {
        this.ballPower= ballPower;
    }

    IEnumerator CannonFire()
    {
        if(!courutineFlag)
        {
            courutineFlag=true;
            Instantiate(cannonBall, ballCreate.transform.position, ballCreate.transform.rotation);
            yield return new WaitForSeconds(0.2f);
            courutineFlag=false;
        }
    }

}
