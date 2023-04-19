using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStates : MonoBehaviour
{
    [SerializeField] private int monsterHP;
    [SerializeField] GameObject DestroyEffect;
    bool isDead = false;
    public int gold;
    [SerializeField] bool isBoss;

    GameManager gameMng;

    void Start()
    {
        gameMng = GameManager.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (monsterHP <= 0)
        {
            isDead = true;
        }

        if (isDead)
        {
            if(isBoss)
            {
                gameMng.setVictory(true);
            }

            GameObject effectObj = Instantiate(DestroyEffect, transform.position, transform.rotation);
            Destroy(effectObj, 1.1f);
            gameMng.IncomeGold(gold);
            gameMng.setMonsterCount(1);
            Destroy(gameObject);

        }

        if(EndingCheek())
        {
            GameObject effectObj = Instantiate(DestroyEffect, transform.position, transform.rotation);
            Destroy(effectObj, 1.1f);
            Destroy(gameObject);
        }

        
    }

    public int getMonsterHP()
    {
        return monsterHP;
    }
    public void setMonsterHP(int hp)
    {
        this.monsterHP = hp;
    }
    private bool EndingCheek()
    {
        return gameMng.isEnding;
    }

}
