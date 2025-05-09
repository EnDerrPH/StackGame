using UnityEngine;
using UnityEngine.Events;

public class BlockHandler : MonoBehaviour
{
    [SerializeField] Rigidbody _rb;
    [SerializeField] Renderer _renderer;
    float _blockSpeed = 1.5f;
    float _moveRange = 1.5f; 
    bool _isHorizontal;
    bool _isMoving;
    Vector3 _startPos;
    public UnityEvent OnGameOverEvent, OnStackedEvent;

    void Start()
    {
       SetComponents();
    }

    void Update()
    {
        ToggleMovement();
    }

    public void SetBlockMaterial(Material material)
    {
        _renderer.material = material;
    }

    public void SetRenderer()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void SetBlockMovement(bool isHorizonal)
    {
        _isHorizontal = isHorizonal;
    }

    public void SetIsMoving(bool isMoving)
    {
        _isMoving = isMoving;
    }

    public void FreezePosition()
    {
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void SetComponents()
    {
        _startPos = transform.position;
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
