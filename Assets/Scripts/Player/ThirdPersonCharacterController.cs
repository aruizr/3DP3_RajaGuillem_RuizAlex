using System;
using UnityEngine;
using Utilities;
using Utilities.Messaging;
using Utilities.Physics;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class ThirdPersonCharacterController : ExtendedMonoBehaviour
    {
        [SerializeField] private RayCaster wallDetector;
        [SerializeField] private RayCaster groundDetector;
        private Transform _camera;
        private CharacterController _controller;
        private float _currentAngle;
        private Vector3 _currentVelocity;
        private Vector2 _direction;
        private float _gravity;
        private bool _isJumping;
        private bool _isPunching;
        private bool _isRunning;
        private float _maxJumpVelocity;
        private float _minJumpVelocity;
        private CoroutineBuilder _punchCoroutine;
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
            _punchCoroutine = Coroutine(destroyOnFinish: false).
                Invoke(() =>
                {
                    _isPunching = true;
                    EventManager.TriggerEvent("OnPlayerPunch", new Message(this));
                }).
                WaitForSeconds(0.3f).
                Invoke(() => _isPunching = false);
        }

        private void Update()
        {
            if (_isPunching) return;
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            var isGrounded = _controller.isGrounded;

            if (!isGrounded && wallDetector.IsColliding && _isJumping)
            {
                transform.Rotate(0, 180, 0);
                _velocity = transform.forward * GameSettings.Instance.player.runningMovementSpeed * 2;
                _verticalVelocity = _maxJumpVelocity;
                _velocity.y = _verticalVelocity;
                EventManager.TriggerEvent("OnPlayerWallJump", new Message(this));
                return;
            }

            _verticalVelocity = isGrounded switch
            {
                true when _isJumping => ((Func<float>) (() =>
                {
                    EventManager.TriggerEvent("OnPlayerJump", new Message(this));
                    return _maxJumpVelocity;
                }))(),
                true => 0f,
                false when !_isJumping && _verticalVelocity > _minJumpVelocity => _minJumpVelocity,
                false => _verticalVelocity - _gravity * Time.fixedDeltaTime
            };

            if (_direction.magnitude < 0.1f)
            {
                _velocity = Vector3.SmoothDamp(_velocity, Vector3.zero, ref _currentVelocity,
                    GameSettings.Instance.player.movementSmoothingTime);
                _velocity.y = _verticalVelocity;
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
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnActionMove", OnActionMove);
            EventManager.StartListening("OnActionRun", OnActionRun);
            EventManager.StartListening("OnActionJump", OnActionJump);
            EventManager.StartListening("OnActionPunch", OnActionPunch);
        }

        private void OnDisable()
        {
            EventManager.StopListening("OnActionMove", OnActionMove);
            EventManager.StopListening("OnActionRun", OnActionRun);
            EventManager.StopListening("OnActionJump", OnActionJump);
            EventManager.StopListening("OnActionPunch", OnActionPunch);
        }

        private void OnActionPunch(Message message)
        {
            if (_isPunching) return;
            if (!groundDetector.IsColliding) return;
            if (_isRunning) return;
            _punchCoroutine.Run();
        }

        private void OnActionMove(Message message)
        {
            _direction = (Vector2) message["direction"];
        }

        private void OnActionRun(Message message)
        {
            _isRunning = (bool) message["isRunning"];
        }

        private void OnActionJump(Message message)
        {
            _isJumping = (bool) message["isJumping"];
        }
    }
}