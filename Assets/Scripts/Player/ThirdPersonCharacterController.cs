using UnityEngine;
using Utilities.Messaging;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonCharacterController : MonoBehaviour
    {
        private Transform _camera;
        private CharacterController _controller;
        private float _currentAngle;
        private Vector3 _currentVelocity;
        private Vector2 _direction;
        private float _gravity;
        private bool _isJumping;
        private bool _isRunning;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        private float _turningSmoothingTime;
        private Vector3 _velocity;
        private float _verticalVelocity;

        private void Awake()
        {
            _camera = Camera.main.transform;
            _controller = GetComponent<CharacterController>();
            _turningSmoothingTime = GameSettings.Instance.player.turnSmoothingTime;
            var maxJumpHeight = GameSettings.Instance.player.maxJumpHeight;
            var minJumpHeight = GameSettings.Instance.player.minJumpHeight;
            var jumpApexTime = GameSettings.Instance.player.jumpApexTime;
            _gravity = 2 * maxJumpHeight / (jumpApexTime * jumpApexTime);
            _maxJumpVelocity = 2 * maxJumpHeight / jumpApexTime;
            _minJumpVelocity = 2 * minJumpHeight / jumpApexTime;
        }

        private void FixedUpdate()
        {
            _verticalVelocity = _controller.isGrounded switch
            {
                true when _isJumping => _maxJumpVelocity,
                true => 0f,
                false when !_isJumping && _verticalVelocity > _minJumpVelocity => _minJumpVelocity,
                false => _verticalVelocity - _gravity * Time.fixedDeltaTime
            };

            if (_direction.magnitude < 0.1f)
            {
                _velocity = Vector3.SmoothDamp(_velocity, Vector3.zero, ref _currentVelocity,
                    GameSettings.Instance.player.movementSmoothingTime);
                _velocity.y = _verticalVelocity;
                _controller.Move(_velocity * Time.fixedDeltaTime);
                return;
            }

            var targetAngle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            targetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentAngle,
                _turningSmoothingTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            var movementSpeed = _isRunning
                ? GameSettings.Instance.player.runningMovementSpeed
                : GameSettings.Instance.player.walkingMovementSpeed;
            var targetVelocity = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * movementSpeed;

            _velocity = Vector3.SmoothDamp(_velocity, targetVelocity, ref _currentVelocity,
                GameSettings.Instance.player.movementSmoothingTime);

            _velocity.y = _verticalVelocity;
            _controller.Move(_velocity * Time.fixedDeltaTime);
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnPlayerMove", OnPlayerMove);
            EventManager.StartListening("OnPlayerRun", OnPlayerRun);
            EventManager.StartListening("OnPlayerJump", OnPlayerJump);
        }

        private void OnDisable()
        {
            EventManager.StopListening("OnPlayerMove", OnPlayerMove);
            EventManager.StopListening("OnPlayerRun", OnPlayerRun);
            EventManager.StopListening("OnPlayerJump", OnPlayerJump);
        }

        private void OnPlayerMove(Message message)
        {
            _direction = (Vector2) message["direction"];
        }

        private void OnPlayerRun(Message message)
        {
            _isRunning = (bool) message["isRunning"];
        }

        private void OnPlayerJump(Message message)
        {
            _isJumping = (bool) message["isJumping"];
        }
    }
}