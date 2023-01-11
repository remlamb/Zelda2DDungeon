using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetBehvior : MonoBehaviour
{
    [SerializeField] private GameObject _bridgeToCreate;
    [SerializeField] private Collider2D _oldHoleCollider;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _eyeClosed;
    private bool _isTrigger;
    private bool _successSongPlayed;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _isTrigger = false;
        _audioSource = GetComponent<AudioSource>(); 
        _successSongPlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        TargetActivated();
    }

    public void SetTrigger()
    {
        _isTrigger = true;
    }

    private void TargetActivated()
    {
        if (_isTrigger)
        {
            _bridgeToCreate.SetActive(true);
            _oldHoleCollider.enabled = false;
            _spriteRenderer.sprite = _eyeClosed;

            if (!_audioSource.isPlaying && !_successSongPlayed)
            {
                _audioSource.Play();
                _successSongPlayed = true;
            }
        }
    }
}
