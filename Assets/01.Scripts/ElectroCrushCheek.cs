using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroCrushCheek : MonoBehaviour
{
    bool isCheek = false;
    int AttackNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isCheek)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isCheek)
            return;

        //몬스터 레이어 검사.
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            MonsterStates monsterStates = other.gameObject.GetComponent<MonsterStates>();
            int currentHP = monsterStates.getMonsterHP();
            monsterStates.setMonsterHP(currentHP - AttackNum);
            Debug.Log("Monster Hit");
            isCheek= false;
        }
    }


    public void setCheek(bool cheek, int AttackNum)
    {
        isCheek= cheek;
        this.AttackNum= AttackNum;
        gameObject.GetComponent<Collider>().enabled = true;
        Invoke("CheekFalse", 1.0f);
    }


    void CheekFalse()
    {
        isCheek= false;
    }
}
