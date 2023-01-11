using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _openedDoor;
    public bool isOpen;
    private Collider2D _doorCollider;
    [SerializeField] private AudioSource _audioSource;
    private bool _audioPlayed;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        _audioPlayed = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _doorCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            OpenDoor();
        }
    }


    private void OpenDoor()
    {
        _doorCollider.enabled = false;
        if (!_audioSource.isPlaying && !_audioPlayed)
        {
            _audioSource.Play();
            _audioPlayed = true;
        }
        _spriteRenderer.sprite = _openedDoor;
    }

}
