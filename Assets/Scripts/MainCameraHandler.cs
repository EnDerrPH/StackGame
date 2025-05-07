using UnityEngine;

public class MainCameraHandler : MonoBehaviour
{
    float _targetYposition;
    float _moveSpeed = 1.5f;

    void Start()
    {
        _targetYposition = transform.position.y;
    }

    void Update()
    {
        MoveCamera();
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

    public void MoveUp()
    {
        _targetYposition = transform.position.y + .1f;
    }
}
