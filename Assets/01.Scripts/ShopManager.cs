using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;



public class ShopManager : MonoBehaviour
{
    GameManager gameMng;
    [SerializeField] Text txtPlayerPowUp;
    [SerializeField] Text txtCannonPowUp;
    [SerializeField] Text PlayerAttack;
    [SerializeField] Text CannonAttack;


    private int nPlayerPowerForCoin = 10;
    private int nCannonPowerForCoin = 10;
    private int nPlayerPowerInNum = 1;
    private int nCannonPowerInNum = 1;

    private int ballPower = 2;
    public bool isMenuOpen;

    public static ShopManager instance;

    private void Awake()
    {
        gameObject.SetActive(false);
        isMenuOpen = false;
        instance = this;
    }

    void Start()
    {
        gameMng = GameManager.Instance;
        PlayerAttack.text = "플레이어 공격력\n"+GameObject.FindGameObjectWithTag("Player").GetComponent<GirlControl>().electroPower.ToString();
        CannonAttack.text = "캐논 공격력\n"+GameObject.FindGameObjectWithTag("Cannon").GetComponent<Cannon>().BallPowerGetter().ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if(gameMng.isCursor == true)
        {
            MenuOut();
        }
    }

    public void MenuSet()
    {
        isMenuOpen= true;
        gameObject.SetActive(true);
    }
    public void MenuOut()
    {
        isMenuOpen= false;
        gameMng.isCursor = true;
        gameObject.SetActive(false);
    }
#if UNITY_EDITOR

    public void BuyPlayerPowerUp()
    {


        if(gameMng.getGold() >= nPlayerPowerForCoin)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<GirlControl>().electroPower += 6;
            int gold = gameMng.getGold();
            gold -= nPlayerPowerForCoin;
            gameMng.setGold(gold);
            nPlayerPowerInNum++;
            nPlayerPowerForCoin += 10;

            txtPlayerPowUp.text = nPlayerPowerInNum.ToString() + "강 - " + nPlayerPowerForCoin.ToString() + "골드";
            PlayerAttack.text = "플레이어 공격력\n" + player.GetComponent<GirlControl>().electroPower.ToString();
        }
        
    }

    public void BuyCannonPowerUp()
    {

        if (gameMng.getGold() >= nCannonPowerForCoin)
        {
            GameObject cannon = GameObject.FindGameObjectWithTag("Cannon");
            cannon.GetComponent<Cannon>().BallPowerSetter(ballPower);
            int gold = gameMng.getGold();
            gold -= nCannonPowerForCoin;
            gameMng.setGold(gold);
            ballPower += 3;
            nCannonPowerInNum++;
            nCannonPowerForCoin += 13;

            txtCannonPowUp.text = nCannonPowerInNum.ToString() + "강 - " + nCannonPowerForCoin.ToString() + "골드";
            CannonAttack.text = "캐논 공격력\n" + cannon.GetComponent<Cannon>().BallPowerGetter().ToString();
        }

    }

#else

    public void BuyPlayerPowerUp()
    {


        if(gameMng.getGold() >= nPlayerPowerForCoin)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<GirlControl>().electroPower += 2;
            int gold = gameMng.getGold();
            gold -= nPlayerPowerForCoin;
            gameMng.setGold(gold);
            nPlayerPowerInNum++;
            nPlayerPowerForCoin += 25*nPlayerPowerInNum;

            txtPlayerPowUp.text = nPlayerPowerInNum.ToString() + "강 - " + nPlayerPowerForCoin.ToString() + "골드";
            PlayerAttack.text = "플레이어 공격력\n" + player.GetComponent<GirlControl>().electroPower.ToString();
        }
        
    }

    public void BuyCannonPowerUp()
    {

        if (gameMng.getGold() >= nCannonPowerForCoin)
        {
            GameObject cannon = GameObject.FindGameObjectWithTag("Cannon");
            cannon.GetComponent<Cannon>().BallPowerSetter(ballPower);
            int gold = gameMng.getGold();
            gold -= nCannonPowerForCoin;
            gameMng.setGold(gold);
            ballPower += 1;
            nCannonPowerInNum++;
            nCannonPowerForCoin += 29*nCannonPowerInNum;

            txtCannonPowUp.text = nCannonPowerInNum.ToString() + "강 - " + nCannonPowerForCoin.ToString() + "골드";
            CannonAttack.text = "캐논 공격력\n" + cannon.GetComponent<Cannon>().BallPowerGetter().ToString();
        }

    }

#endif
}
