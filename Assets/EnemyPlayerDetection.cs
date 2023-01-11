using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerDetection : MonoBehaviour
{
    private EnemyController _enemyController;



    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if Player enter Detection Zone
        if (collision.CompareTag("Player"))
        {
            _enemyController.playerFound = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if Player enter Detection Zone
        if (collision.CompareTag("Player"))
        {
            _enemyController.playerFound = false;
        }
    }
}
