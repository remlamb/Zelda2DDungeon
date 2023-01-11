using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    [SerializeField] private float goodRange;
    private EnigmaDoorController _enigmaDoorController;
    [SerializeField] private GameObject[] _enigmaDoors;

    private Vector3 _initialPosition;
    private Animator _blockAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = this.transform.position;
        _blockAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_initialPosition == null)
        {
            
        }
        Debug.Log("Init pos : " + _initialPosition);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BlockGoal"))
        {
            float distance = Vector2.Distance(transform.position, collision.transform.position);

            if(distance < goodRange)
            {
                //creer des fonctions pour detruire le rigidbody2d
                Debug.Log("Nice Job");
                BlockGetGoalPosition(collision);
                DestroyRigidbodyComponent();
                OpenDoor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DestroyPosition"))
        {
            Debug.Log("Boom");
            CubeOnDestroyPosition();
        }
    }


    private void BlockGetGoalPosition(Collider2D collision)
    {
        this.transform.position = collision.transform.position;
    }

    private void DestroyRigidbodyComponent()
    {
        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
    }

    private void CubeOnDestroyPosition()
    {
        //Explosion and respawn
        _blockAnimator.SetBool("isDestroy", true);
        this.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        StartCoroutine(ResetBlockPosition());
    }

    IEnumerator ResetBlockPosition()
    {
        yield return new WaitForSeconds(0.8f);
        this.transform.position = _initialPosition;
        this.gameObject.GetComponent<Rigidbody2D>().simulated = true;
        _blockAnimator.SetBool("isDestroy", false);
    }

    private void OpenDoor()
    {
        foreach(GameObject door in _enigmaDoors)
        {
            if (door != null)
            {
                _enigmaDoorController = door.GetComponent<EnigmaDoorController>();
                _enigmaDoorController.OpenTheDoor();
            }
        }

    }
}
