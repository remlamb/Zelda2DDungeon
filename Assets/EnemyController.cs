using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float _attackRadius;
    public bool playerFound;
    [SerializeField] private Animator _animator;
    private bool _isDead;
    public bool isHit;
    private AudioSource _audioSource;
    private bool _deathSoundPlayed;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindWithTag("Player").transform;
        playerFound = false;
        _isDead = false;
        isHit = false;
        _deathSoundPlayed = false;
        _audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        RunOnPlayer();
        AttackPlayer();


        if (isHit)
        {
            isHit = false;
            EnemyDeath();
        }
    }

    private void RunOnPlayer()
    {
        if(playerFound)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position, 0.01f);
        }
    }

    private void AttackPlayer()
    {
        float distance = Vector2.Distance(this.transform.position, _target.transform.position);
        Debug.Log(distance);
        if (distance <= _attackRadius && playerFound)
        {
            //play attackAnim
            _animator.SetBool("isAttacking", true);
        }
        else
        {
            _animator.SetBool("isAttacking", false);
        }
    }


    private void EnemyDeath()
    {
        _isDead = true;
        _animator.SetBool("isDead", true);
        if (!_audioSource.isPlaying && !_deathSoundPlayed)
        {
            _audioSource.Play();
            _deathSoundPlayed = true;
        }
        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

}
