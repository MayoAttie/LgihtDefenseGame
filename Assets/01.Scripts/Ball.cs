using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rigd3D;
    private float addForce = 1000;
    public int ballPower;

    private void Awake()
    {
        rigd3D = GetComponent<Rigidbody>();
        rigd3D.AddForce(transform.up * addForce);
    }
    void Start()
    {
        GameObject obj= GameObject.FindGameObjectWithTag("Cannon");
        ballPower = obj.GetComponent<Cannon>().BallPowerGetter();
        Destroy(gameObject, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        float rayLength = 0.2f;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray.origin, ray.direction, out hit, rayLength))
        {
            if (hit.collider.tag == "Ground" || hit.collider.tag == "EnemyField")
            {
                Destroy(gameObject, 0.7f);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
                
        //몬스터 레이어 검사.
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            MonsterStates monsterStates = other.gameObject.GetComponent<MonsterStates>();
            int currentHP = monsterStates.getMonsterHP();
            monsterStates.setMonsterHP(currentHP - ballPower);
            Destroy(gameObject);
            Debug.Log("Monster Hit");
        }
    }



}
