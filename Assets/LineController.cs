using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LineController : MonoBehaviour
{
    [SerializeField]  private LineRenderer lineRenderer = null;
    private BoxCollider2D lineCollider2D;
    [SerializeField] private Vector2 _lineDirection;
    [SerializeField] private GameObject _lineFirstPoint;
    [SerializeField] private GameObject _laserEnemy;
    public bool _Xaxis;
    public bool isHit;

    [SerializeField] private GameObject _roomcCamera;
    [SerializeField] private AudioSource _audioSource;

    void Start()
    {
        lineCollider2D = GetComponent<BoxCollider2D>();
        isHit = false;
    }

    void Update()
    {
        //Set the first point of the line renderer 
        lineRenderer.SetPosition(0, _lineFirstPoint.transform.position);

        EndLineOnCollision();
        SetupColliderSize();
        PlayLaserSound();
        OnHit();

    }

    private float MiddleBetweenTwoPoint(float firstPoint, float secondPoint)
    {
        return firstPoint - secondPoint;
    }

    private void SetupColliderSize()
    {
        if (_lineDirection == new Vector2(1, 0))
        {
            lineCollider2D.offset = new Vector2(0, -1 * (MiddleBetweenTwoPoint(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(0).x) / 2) + MiddleBetweenTwoPoint(this.transform.position.x, lineRenderer.GetPosition(0).x));
            lineCollider2D.size = new Vector2(0.2f, MiddleBetweenTwoPoint(lineRenderer.GetPosition(1).x, lineRenderer.GetPosition(0).x));
        }

        if (_lineDirection == new Vector2(-1, 0))
        {
            lineCollider2D.offset = new Vector2(0, -1 * (MiddleBetweenTwoPoint(lineRenderer.GetPosition(0).x, lineRenderer.GetPosition(1).x) / 2) - MiddleBetweenTwoPoint(this.transform.position.x, lineRenderer.GetPosition(0).x));
            lineCollider2D.size = new Vector2(0.2f, lineRenderer.GetPosition(0).x - lineRenderer.GetPosition(1).x);
        }

        if (_lineDirection == new Vector2(0, 1))
        {
            lineCollider2D.offset = new Vector2(0, -1 * (MiddleBetweenTwoPoint(lineRenderer.GetPosition(1).y, lineRenderer.GetPosition(0).y) / 2) + MiddleBetweenTwoPoint(this.transform.position.y, lineRenderer.GetPosition(0).y));
            lineCollider2D.size = new Vector2(0.2f, lineRenderer.GetPosition(1).y - lineRenderer.GetPosition(0).y);
        }

        if (_lineDirection == new Vector2(0, -1))
        {
            lineCollider2D.offset = new Vector2(0, -1 * (MiddleBetweenTwoPoint(lineRenderer.GetPosition(0).y, lineRenderer.GetPosition(1).y) / 2) - MiddleBetweenTwoPoint(this.transform.position.y, lineRenderer.GetPosition(0).y));
            lineCollider2D.size = new Vector2(0.2f, (lineRenderer.GetPosition(1).y + lineRenderer.GetPosition(0).y) / 2);
        }
    }

    private void EndLineOnCollision()
    {
        //Get all the Raycast collision if the object is not trigger or not a limit we create the second point of the line renderer at the collision position
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, _lineDirection);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (!hit.transform.gameObject.CompareTag("LimitBlockGame") && !hit.transform.gameObject.CompareTag("Player"))
            {
                if (hit.collider.isTrigger == false)
                {
                    lineRenderer.SetPosition(1, hit.point);
                    break;
                }
            }
        }
    }

    private void OnHit()
    {
        if (isHit)
        {
            Animator animator = GetComponentInParent<Animator>();
            animator.SetBool("isDead", true);
            lineRenderer.SetPosition(0, new Vector2(0,0));
            lineRenderer.SetPosition(1, new Vector2(0, 0));
            StartCoroutine(DestroyGameObject());
        }
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(1f);
        Destroy(_laserEnemy);
    }

    private void PlayLaserSound()
    {
        if(_roomcCamera.activeSelf && !_audioSource.isPlaying && !isHit)
        {
            _audioSource.Play();
        }
    }
}
