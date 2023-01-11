using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _sword;
    private Animator _animator;
    private int _nbrKey;
    private int _nbrRubis;
    private int _nbrBigKey;
    [SerializeField] private TextMeshProUGUI _keyText;
    [SerializeField] private TextMeshProUGUI _rubisText;

    private bool _isFacingRight;
    private bool _isFacingLeft;
    private bool _isFacingBottom;
    private bool _isFacingTop;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _arrowSpeed;
    private bool _canShoot;
    [SerializeField] private float _timeBetweenArrow;
    private float _timerArrow;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _arrowClip;
    [SerializeField] private AudioClip _breathClip;
    [SerializeField] private AudioClip _swingClip;
    [SerializeField] private AudioClip _getItemClip;

    [SerializeField] private GameObject _bigKeyIcon;
    [SerializeField] private GameObject _startScreen;
    private DamageController _damageController;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;

    private bool _isKnockBack;
    private float _KbTimer;

    private bool _isPushing;
    private bool _canAtt;
    [SerializeField] private float _timeBetweenAtt;
    private float _timerAtt;

    private bool _gotBow;
    [SerializeField] private GameObject _arrowIcon;

    // Start is called before the first frame update
    void Start()
    {
        _nbrRubis = 0;
        _nbrKey = 0;
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _isFacingBottom = true;
        _isFacingTop = false;
        _isFacingLeft = false;
        _isFacingRight = false;

        _isKnockBack = false;
        _KbTimer = 0f;

        _canShoot = true;
        _canAtt = true;
        _timerArrow = 0f;
        _timerAtt = 0f;

        _nbrBigKey = 0;
        _startScreen.SetActive(true);

        _damageController = GetComponent<DamageController>();
        _gotBow = false;
    }

    private void Update()
    {
        //Ui
        _keyText.text = Convert.ToString(_nbrKey);
        _rubisText.text = Convert.ToString(_nbrRubis);
        if(_nbrBigKey >= 1)
        {
            _bigKeyIcon.SetActive(true);
        }
        else
        {
            _bigKeyIcon.SetActive(false);
        }

        ShowBowUI();
        ResetKnockBack();
        CooldownArrow();
        CooldownSword();
        StartCoroutine(PlayEffortSound());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_rb != null && !_isKnockBack)
        {
            _rb.velocity = _moveInput * _speed;
        }
        _animator.SetFloat("Speed", _rb.velocity.magnitude);
        _animator.SetFloat("VelY", -1 * _rb.velocity.y);
        _animator.SetFloat("VelX", _rb.velocity.x);

        if(_rb.velocity.x > 1)
        {
            _animator.SetBool("isMovingOnX", true);
        }
        else if(_rb.velocity.x < -1)
        {
            _animator.SetBool("isMovingOnX", true);
            _isFacingLeft = true;
            _animator.SetBool("isFacingLeft", true);
        }
        else
        {
            _animator.SetBool("isMovingOnX", false);
            _isFacingLeft = false;
            _animator.SetBool("isFacingLeft", false);
        }

        if(_rb.velocity.x > -0.1 && _rb.velocity.x < 0.1)
        {
            _animator.SetBool("isMovingOnX", false);
        }

        if (_isFacingLeft)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }

        //for BreathingSound
        _isPushing = false;
    }

    public void HandleMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();

        
        if(ctx.ReadValue<Vector2>().x >= 1)
        {
            _isFacingRight = true;
            _isFacingLeft = false;
            _isFacingBottom = false;
            _isFacingTop = false;
            _animator.SetBool("isMovingOnX", true);
            _animator.SetBool("isFacingLeft", false);
        }
        else if (ctx.ReadValue<Vector2>().x <= -1)
        {
            _isFacingLeft = true;
            _isFacingRight = false;
            _isFacingBottom = false;
            _isFacingTop = false;
            _animator.SetBool("isMovingOnX", true);
            _animator.SetBool("isFacingLeft", true);
        }
        else if(ctx.ReadValue<Vector2>().y <= -1)
        {
            _isFacingLeft = false;
            _isFacingRight = false;
            _isFacingBottom = true;
            _isFacingTop = false;
            _animator.SetBool("isFacingLeft", false);
        }
        else if (ctx.ReadValue<Vector2>().y >= 1)
        {
            _isFacingLeft = false;
            _isFacingRight = false;
            _isFacingBottom = false;
            _isFacingTop = true;
            _animator.SetBool("isFacingLeft", false);
        }

    }

    public void HandleSword(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Started) //or (ctx.Started)
        {
            if (_canAtt)
            {
                _animator.SetBool("isAttacking", true);
                //_sword.SetActive(true);

                SwordSwingAudio();
                _canAtt = false;
                _timerAtt = 0;
            }
        }

        if(ctx.phase == InputActionPhase.Canceled)
        {

            //_animator.SetBool("isAttacking", false);
        }
    }

    private void SwordSwingAudio()
    {
        _audioSource.clip = _swingClip;
        _audioSource.Play();
    }

    private void CooldownSword()
    {
        _timerAtt += Time.deltaTime;
        if (_timerAtt >= _timeBetweenAtt)
        {
            _canAtt = true;
            _animator.SetBool("isAttacking", false);
        }
    }

    public void HandleSpawnArrow(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Started) //or (ctx.Started)
        {
            if (_canShoot && _gotBow)
            {
                //Spawn Arrow + give Velocity
                if (_isFacingLeft)
                {
                    SettingArrowDirectionAndVelocity(-90, -1, 0);
                }

                if (_isFacingRight)
                {
                    SettingArrowDirectionAndVelocity(90, 1, 0);
                }

                if (_isFacingTop)
                {
                    SettingArrowDirectionAndVelocity(-180, 0, 1);
                }

                if (_isFacingBottom)
                {
                    SettingArrowDirectionAndVelocity(0, 0, -1);
                }
                ArrowSound();
                _canShoot = false;
                _timerArrow = 0;

            }
        }
    }

    private void CooldownArrow()
    {
        _timerArrow += Time.deltaTime;
        if (_timerArrow >= _timeBetweenArrow)
        {
            _canShoot = true;
        }
    }

    private void ShowBowUI()
    {
        if (_gotBow)
        {
            _arrowIcon.SetActive(true);
        }
        else
        {
            _arrowIcon.SetActive(false);
        }
    }

    private void ArrowSound()
    {
        _audioSource.clip = _arrowClip;
        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectible"))
        {
            KeyController kc = collision.GetComponent<KeyController>();
            if (kc != null)
            {
                _nbrKey += 1;
            }

            RubisController rc = collision.GetComponent<RubisController>();
            if (rc != null)
            {
                _nbrRubis += 1;
            }

            BigKeyController bkc = collision.GetComponent<BigKeyController>();
            if (bkc != null)
            {
                _nbrBigKey += 1;
            }

            PlayItemSong();
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Bow"))
        {
            _gotBow = true;
            PlayItemSong();
            Destroy(collision.gameObject);
        }


        //LaserContact KnockBack
        if (collision.CompareTag("Laser"))
        {
            _KbTimer = 0;
            _isKnockBack = true;
            LineController lc = collision.GetComponent<LineController>();
            _damageController.TakeDamage();

            if (lc._Xaxis)
            {
                if (collision.transform.position.y > this.transform.position.y)
                {
                    _rb.AddForce(new Vector2(0f, -10f), ForceMode2D.Impulse);
                }
                else if (collision.transform.position.y < this.transform.position.y)
                {
                    _rb.AddForce(new Vector2(0f, 10f), ForceMode2D.Impulse);
                }
            }

            else
            {
                if (collision.transform.position.x > this.transform.position.x)
                {
                    _rb.AddForce(new Vector2(-10f, 0f), ForceMode2D.Impulse);
                }
                else if (collision.transform.position.x < this.transform.position.x)
                {
                    _rb.AddForce(new Vector2(10f, 0f), ForceMode2D.Impulse);
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Door")
        {
            if (_nbrKey > 0)
            {
                collision.gameObject.GetComponent<LockedDoorController>().isOpen = true;
                _nbrKey -= 1;
            }
        }


        if (collision.gameObject.tag == "FinalDoor")
        {
            if (_nbrBigKey > 0)
            {
                collision.gameObject.GetComponent<LockedDoorController>().isOpen = true;
                _nbrBigKey -= 1;
            }
        }

            if (collision.gameObject.tag == "PushableBlock")
        {
            _isPushing = true;
        }
    }

    IEnumerator PlayEffortSound()
    {
        if(!_audioSource.isPlaying && _isPushing)
        {
            _audioSource.clip = _breathClip;
            _audioSource.Play();
            yield return new WaitForSeconds(.4f);
        }
    }

    private void SettingBoolDirection(bool trueBool, bool firstFalseBool, bool secondFalseBool, bool thirdFalseBool)
    {
        trueBool = true;
        Debug.Log(trueBool);
        firstFalseBool = false;
        secondFalseBool = false;
        thirdFalseBool = false;
    }

    private void SettingArrowDirectionAndVelocity(float rotateY, float directionX, float directionY)
    {
        GameObject thisArrow = Instantiate(_arrow, this.transform.position, Quaternion.identity);
        thisArrow.transform.Rotate(0, 0, rotateY);
        Rigidbody2D arrowRb = thisArrow.GetComponent<Rigidbody2D>();
        arrowRb.velocity = new Vector2(directionX * _arrowSpeed, directionY * _arrowSpeed);
    }

    private void ResetKnockBack()
    {
        //For the laser Enigma
        _KbTimer += Time.deltaTime;
        if (_KbTimer > 0.2f)
        {
            _isKnockBack = false;
        }
    }

    private void PlayItemSong()
    {
        _audioSource.clip = _getItemClip;
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void RemoveScreen()
    {
        if (_startScreen.activeSelf)
        {
            _startScreen.SetActive(false);
        }  
    }

}
