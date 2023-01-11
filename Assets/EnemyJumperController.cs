using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumperController : MonoBehaviour
{
    [SerializeField] private Transform _pos1;
    [SerializeField] private Transform _pos2;
    private float _movementTimer;
    private Transform _lastPosition;
    private Transform _currentDirection;
    [SerializeField] private Animator _animator;
    private bool _isDead;
    private bool _deathSoundPlayed;
    private AudioSource _audioSource;

    [SerializeField] private GameObject _doorLink;
    [SerializeField] private float _timeBetweenJump;

    // Start is called before the first frame update
    void Start()
    {
        _movementTimer = 0;
        _lastPosition = _pos1;
        _animator = GetComponent<Animator>();
        _isDead = false;
        _deathSoundPlayed = false;  
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDead)
        {
            _movementTimer += Time.deltaTime;
            EnemyJumping();
        }
    }



    private Transform EnemyJumpingPosition()
    {
            if (_lastPosition == _pos1)
            {
            _movementTimer = 0;
            _lastPosition = _pos2;
                return _pos2;

            }
            else
            {
            _movementTimer = 0;
            _lastPosition = _pos1;
                return _pos1;

            }
    }

    private void EnemyJumping()
    {
        if (_movementTimer >= _timeBetweenJump)
        {
            _currentDirection = EnemyJumpingPosition();
        }
        if(_currentDirection != null)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, _currentDirection.position, 0.05f);
        }
        _animator.SetBool("isJumping", true);
        if (transform.position == _currentDirection.position)
        {
            _animator.SetBool("isJumping", false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if Player enter Detection Zone
        if (collision.CompareTag("Arrow"))
        {
            //Sound + Anim + Drop
            EnemyDeath();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Sword"))
        {
            //Sound + Anim + Drop
            EnemyDeath();
        }
    }


    private void EnemyDeath()
    {
        _isDead = true;
        _animator.SetBool("isDead", true);
        AddDoorScore();
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

    private void AddDoorScore()
    {
        EnigmaDoorController EDC = _doorLink.GetComponent<EnigmaDoorController>();
        if(EDC != null)
        {
            EDC.doorScore += 1;
        }
    }
}
