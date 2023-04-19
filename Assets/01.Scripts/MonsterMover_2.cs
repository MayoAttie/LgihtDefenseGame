using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMover_2 : MonoBehaviour
{
    public Transform[] destinations;
    //몬스터 속도
    [SerializeField] float moveSpeed;
    private int currentIndex = 0;



    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, Vector3.down);
        if (Physics.Raycast(ray.origin, ray.direction, out hit))
        {
            if (hit.collider.tag == "EnemyField")
            {
                gameObject.transform.Translate((destinations[currentIndex].position - transform.position).normalized * moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, destinations[currentIndex].position) < 0.1f)
                {
                    currentIndex = (currentIndex + 1) % destinations.Length;
                }
            }
        }


    }

}
