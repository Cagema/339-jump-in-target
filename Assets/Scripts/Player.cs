using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float _maxForce;
    [SerializeField] float _speed;
    [SerializeField] float _rotSpeed;
    [SerializeField] float _heightJump;

    [SerializeField] Slider _forceSlider;

    float _force;
    Vector3 _startPos;
    bool _inJump;
    bool _inTarget;

    Rigidbody2D _rb;
    private void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody2D>();

        _forceSlider.minValue = _force;
        _forceSlider.maxValue = _maxForce;
    }
    private void Update()
    {
        if (GameManager.Single.GameActive)
        {
            if (!_inJump && Input.GetMouseButton(0))
            {
                _force += Time.deltaTime * _speed;
                _forceSlider.value = _force;

                if (_force > _maxForce)
                {
                    Jump();
                }
            }

            if (Input.GetMouseButtonUp(0) && !_inJump)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        _inJump = true;
        _rb.velocity = Vector3.zero;
        _rb.AddForce(new Vector2(_force, _heightJump), ForceMode2D.Impulse);
        _rb.angularVelocity = -_rotSpeed;
    }

    private void Restart()
    {
        _inJump = false;
        _force = 0;
        _forceSlider.value = _force;
        transform.position = _startPos;
        _rb.velocity = Vector3.zero;

        FindObjectOfType<Target>().SetNewPos();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_inJump)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0;

            float curAngle = transform.rotation.eulerAngles.z;
            if (curAngle < 45 && curAngle >= 0 || curAngle > 315 && curAngle <= 360)
            {
                if (_inTarget)
                {
                    GameManager.Single.Score++;
                    Invoke(nameof(Restart), 1.5f);
                }
                else
                {
                    GameManager.Single.LostLive();
                }
            }
            else
            {
                GameManager.Single.LostLive();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _inTarget = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _inTarget = false;
    }
}
