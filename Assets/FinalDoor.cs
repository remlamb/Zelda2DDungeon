using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private LockedDoorController _doorController;
    [SerializeField] private GameObject _endScreen;  
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _doorController = GetComponent<LockedDoorController>();
        _endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_doorController.isOpen)
        {
            _spriteRenderer.enabled = false;
            _endScreen.SetActive(true);
        }
    }
}
