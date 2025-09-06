using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController: MonoBehaviour
{
    const float MIN = -89;
    const float MAX = 89;

    [SerializeField] private float _speed = 5;
    [SerializeField] private float _sens = 4;
    [SerializeField] private float _jump = 0.6f;
    [SerializeField] private float _gravityScale = 1;
    [SerializeField] private float _runModifier = 1.5f;
    [SerializeField] private KeyCode _runKey = KeyCode.LeftShift;

    private float _vert;
    private float _gravity;
    private CharacterController _controller;
    private Transform _vertical;

    private void Reset()
    {
        var camera = Camera.main.transform;
        camera.parent = transform;
        camera.forward = transform.forward;
        camera.localPosition = new Vector3(0, 1.72f, 0);
        GetComponent<CharacterController>().center = new Vector3(0, 1, 0);
    }

    void Start()
    {
        _vertical = Camera.main.transform;
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0)
            return;

        //look
        float mouseX = Input.GetAxis("Mouse X") *  _sens;
        float mouseY = Input.GetAxis("Mouse Y") * -_sens;

        transform.Rotate(0, mouseX, 0);

        _vert += mouseY;
        _vert = Mathf.Clamp(_vert, MIN, MAX);
        Vector3 angles = _vertical.eulerAngles;
        angles.x = _vert;
        _vertical.eulerAngles = angles;


        //movement
        float speed = _speed;

        if (Input.GetKey(_runKey))
            speed *= _runModifier;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(x * speed, 0, z * speed);
        move = Vector3.ClampMagnitude(move, speed);

        if (_controller.isGrounded)
        {
            if (Input.GetAxis("Jump") != 0)
                _gravity = _jump;
            else
                _gravity = -1;
        }
        else
        {
            _gravity += Physics.gravity.y * _gravityScale * Time.deltaTime;
        }

        move.y = _gravity;

        _controller.Move(transform.rotation * move * Time.deltaTime);
    }
}