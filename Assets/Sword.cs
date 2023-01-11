using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject _rubis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Instantiate(_rubis, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }


        if (collision.CompareTag("Enemy"))
        {
            EnemyController ec = collision.GetComponent<EnemyController>();
            ec.isHit = true;
        }
    }

}
