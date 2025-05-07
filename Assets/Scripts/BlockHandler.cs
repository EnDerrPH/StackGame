using UnityEngine;
using UnityEngine.Events;

public class BlockHandler : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    float _blockSpeed = 1.5f;
    float _moveRange = 1.5f; 
    bool _isHorizontal;
    bool _isMoving;
    Vector3 _startPos;
    Renderer _renderer;
    public UnityEvent OnGameOverEvent, OnStackedEvent;

    void Start()
    {
       SetComponents();
       RandomizeColor();
    }

    void Update()
    {
        ToggleMovement();
    }

    public void SetBlockMovement(bool isHorizonal)
    {
        _isHorizontal = isHorizonal;
    }

    public void FreezePosition()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void SetComponents()
    {
        _startPos = transform.position;
        _renderer = GetComponent<Renderer>();
        _isMoving = true;
    }

    private void ToggleMovement()
    {
        OnBlockMovement(_isHorizontal);
    }

    private void OnBlockMovement(bool isHorizontal)
    {
        if(!_isMoving)
        {
            return;
        }

        float offset = Mathf.PingPong(Time.time * _blockSpeed, _moveRange * 2) - _moveRange;

        if(!isHorizontal)
        {
            transform.position = new Vector3(_startPos.x + offset, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _startPos.z + offset);
        }
    }

    private void RandomizeColor()
    {
        _renderer.material.color = new Color(Random.value, Random.value, Random.value);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        FreezePosition();
        _isMoving = false;
        OnStackedEvent.Invoke();

        if(collision.gameObject.tag == "platform")
        {
            OnGameOverEvent.Invoke();
        }
    }
}
