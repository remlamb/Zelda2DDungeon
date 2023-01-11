using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    [SerializeField] private Sprite _openChessSprite;
    [SerializeField] private SpriteRenderer _chessSpriteRdr;
    [SerializeField] private GameObject _key;
    bool chestIsOpen;
    [SerializeField] private Transform _spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        chestIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody.CompareTag("Player"))
        {
            if (!chestIsOpen)
            {
                Instantiate(_key, _spawnPoint.position, Quaternion.identity);
                _chessSpriteRdr.sprite = _openChessSprite;
                chestIsOpen = true;
            }
        }
    }

}
