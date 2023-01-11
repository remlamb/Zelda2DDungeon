using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _playerSprite;
    private bool _isTakingDamage;
    [SerializeField] private float _invulnerabilityTime;
    [SerializeField] private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _isTakingDamage = false;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ReturnToBasicState());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if Player enter Detection Zone
        if (collision.CompareTag("Enemy"))
        {
            if (!_isTakingDamage)
            {
                TakeDamage();
            }
        }



        if (collision.CompareTag("EnemySword"))
        {
            if (!_isTakingDamage)
            {
                TakeDamage();
            }
        }
    }



    public void TakeDamage()
    {
        //Invulnerability frames during coroutine
        _isTakingDamage = true;
        _playerSprite.color = new Color(1, 0.533f, 0.533f);
        _audioSource.Play();
    }

    private IEnumerator ReturnToBasicState()
    {
        if (_isTakingDamage)
        {
            yield return new WaitForSeconds(_invulnerabilityTime);
            _isTakingDamage = false;
            _playerSprite.color = new Color(1, 1, 1);
        }
    }

}
