using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
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
            Destroy(this.gameObject);
        }

        
        if (collision.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Target"))
        {
            TargetBehvior tb = collision.gameObject.GetComponentInChildren<TargetBehvior>();
            tb.SetTrigger();

            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Door"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Block"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Laser"))
        {
            Destroy(this.gameObject);
        }
        if (collision.CompareTag("Assets"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("PushableBlock"))
        {
            Destroy(this.gameObject);
        }

        if (collision.CompareTag("Enemy"))
        {
            EnemyController ec = collision.GetComponent<EnemyController>();
            LineController lc = collision.GetComponentInChildren<LineController>();


            if(ec != null)
            {
                ec.isHit = true;
            }

            else if(lc != null)
            {
                Debug.Log("Hit");
                lc.isHit = true;
            }

            Destroy(this.gameObject);
        }
    }




}

