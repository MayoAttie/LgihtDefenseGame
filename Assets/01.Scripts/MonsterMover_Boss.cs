using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMover_Boss : MonoBehaviour
{
    public Transform[] destinations;
    //몬스터 속도
    [SerializeField] float moveSpeed;
    private int currentIndex = 0;
    private LayerMask layerMask;
    GameManager gameMng;
    
    void Start()
    {
        gameMng = GameManager.Instance;
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
                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("EnemyStartPos"))
                {
                    if(currentIndex != 0)
                    {
                        gameMng.setFailed(true);
                    }
                    
                }
                
                gameObject.transform.Translate((destinations[currentIndex].position - transform.position).normalized * moveSpeed * Time.deltaTime);


                if (Vector3.Distance(transform.position, destinations[currentIndex].position) < 0.1f)
                {
                    currentIndex = (currentIndex + 1) % destinations.Length;
                }
            }

            
        }

    }

}
