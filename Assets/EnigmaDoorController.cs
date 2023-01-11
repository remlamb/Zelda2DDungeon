using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnigmaDoorController : MonoBehaviour
{
    [SerializeField] private Sprite _openEnigmaDoor;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider2d;

    [SerializeField] private bool _isEnemyDoor;
    public float doorScore;
    [SerializeField] private float _nbrEnemy;

    private AudioSource _audioSource;
    private bool _audioPlayed;


    // Start is called before the first frame update
    void Start()
    {
        _audioPlayed = false;   
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2d = GetComponent<BoxCollider2D>();
        _audioSource = GetComponent<AudioSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyDoor)
        {
            if(doorScore >= _nbrEnemy)
            {
                OpenTheDoor();
            }
        }
    }

    public void OpenTheDoor()
    {
        _spriteRenderer.sprite = _openEnigmaDoor;
        _collider2d.enabled = false;
        if (!_audioSource.isPlaying && !_audioPlayed)
        {
            _audioSource.Play();
            _audioPlayed = true;
        }
    }
}
