using UnityEngine;

public class MainCameraHandler : MonoBehaviour
{
    float _targetYposition;
    float _moveSpeed = 1.5f;
    const float _zoomInValue = .02f;

    void Start()
    {
        _targetYposition = transform.position.y;
    }

    void Update()
    {
        MoveCamera();
    }

    public void MoveUp()
    {
        _targetYposition = transform.position.y + 0.07f;
        ZoomIn();
    }

    private void MoveCamera()
    {
        if (transform.position.y < _targetYposition)
        {
            // Lerp the Y position towards the target Y smoothly
            float newY = Mathf.Lerp(transform.position.y, _targetYposition, Time.deltaTime * _moveSpeed);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }

    private void ZoomIn()
    {
        Vector3 pos = transform.position;
        pos.x += _zoomInValue; 
        pos.z +=_zoomInValue; // -2 becomes -2.01
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }
}
