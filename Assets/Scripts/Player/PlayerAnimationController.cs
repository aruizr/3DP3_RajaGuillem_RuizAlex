using UnityEngine;
using Utilities;
using Utilities.Attributes;
using Utilities.Messaging;
using Utilities.Physics;

namespace Player
{
    public class PlayerAnimationController : ExtendedMonoBehaviour
    {
        private static readonly int HorizontalSpeed = Animator.StringToHash("Horizontal Speed");
        private static readonly int VerticalSpeed = Animator.StringToHash("Vertical Speed");
        private static readonly int OnJump = Animator.StringToHash("OnJump");
        private static readonly int OnDoubleJump = Animator.StringToHash("OnDoubleJump");
        private static readonly int OnTripleJump = Animator.StringToHash("OnTripleJump");
        private static readonly int IsGrounded = Animator.StringToHash("isGrounded");
        private static readonly int OnLongJump = Animator.StringToHash("OnLongJump");
        private static readonly int OnFirstPunch = Animator.StringToHash("OnFirstPunch");
        private static readonly int OnSecondPunch = Animator.StringToHash("OnSecondPunch");
        private static readonly int OnThirdPunch = Animator.StringToHash("OnThirdPunch");
        private static readonly int OnWallJump = Animator.StringToHash("OnWallJump");

        [SerializeField] private RayCaster groundDetector;
        [SerializeField] [ReadOnly] private int jumpCounter;
        [SerializeField] [ReadOnly] private int punchCounter;

        private Animator _animator;
        private CharacterController _controller;
        private bool _isRunning;
        private CoroutineBuilder _jumpCounterReset;
        private float _maxSpeed;
        private CoroutineBuilder _punchCounterReset;
        private CoroutineBuilder _specialIdle;

        private void Awake()
        {
            Messenger.Send<CharacterController>(controller => _controller = controller, gameObject);
            Messenger.Send<Animator>(animator => _animator = animator, gameObject);
            _maxSpeed = GameSettings.Instance.player.runningMovementSpeed;
            _jumpCounterReset = Coroutine(destroyOnFinish: false).
                WaitForSeconds(GameSettings.Instance.player.jumpComboResetTime).
                Invoke(() => jumpCounter = 0);
            _punchCounterReset = Coroutine(destroyOnFinish: false).
                WaitForSeconds(GameSettings.Instance.player.punchComboResetTime).
                Invoke(() => jumpCounter = 0);
            _specialIdle = Coroutine(destroyOnFinish: false).
                WaitForSeconds(10)
                .Invoke(() => _animator.SetTrigger(OnThirdPunch));
        }

        private void FixedUpdate()
        {
            var velocity = _controller.velocity;
            var horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            if (horizontalSpeed < 0.1f && !_specialIdle.IsRunning) _specialIdle.Run();
            else if (horizontalSpeed >= 0.1f && _specialIdle.IsRunning) _specialIdle.Cancel();
            _animator.SetFloat(HorizontalSpeed, horizontalSpeed / _maxSpeed);
            _animator.SetFloat(VerticalSpeed, velocity.y);
            _animator.SetBool(IsGrounded, groundDetector.IsColliding);
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnPlayerJump", OnPlayerJump);
            EventManager.StartListening("OnPlayerWallJump", OnPlayerWallJump);
            EventManager.StartListening("OnActionRun", OnActionRun);
            EventManager.StartListening("OnPlayerPunch", OnPlayerPunch);
        }

        private void OnPlayerPunch(Message message)
        {
            switch (punchCounter)
            {
                case 0:
                    _animator.SetTrigger(OnFirstPunch);
                    EventManager.TriggerEvent("OnFirstPunch", new Message(this));
                    break;
                case 1:
                    _animator.SetTrigger(OnSecondPunch);
                    EventManager.TriggerEvent("OnSecondPunch", new Message(this));
                    break;
                case 2:
                    _animator.SetTrigger(OnThirdPunch);
                    EventManager.TriggerEvent("OnThirdPunch", new Message(this));
                    break;
            }

            punchCounter++;
            if (punchCounter > 2) punchCounter = 0;

            _punchCounterReset.Cancel();
            _punchCounterReset.Run();
        }

        private void OnPlayerWallJump(Message message)
        {
            _animator.SetTrigger(OnLongJump);
            EventManager.TriggerEvent("OnLongJump", new Message(this));
        }

        private void OnActionRun(Message message)
        {
            _isRunning = (bool) message["isRunning"];
        }

        private void OnPlayerJump(Message message)
        {
            if (_isRunning)
            {
                _animator.SetTrigger(OnLongJump);
                EventManager.TriggerEvent("OnLongJump", new Message(this));
                jumpCounter = 0;
                return;
            }

            switch (jumpCounter)
            {
                case 0:
                    _animator.SetTrigger(OnJump);
                    EventManager.TriggerEvent("OnJump", new Message(this));
                    break;
                case 1:
                    _animator.SetTrigger(OnDoubleJump);
                    EventManager.TriggerEvent("OnDoubleJump", new Message(this));
                    break;
                case 2:
                    _animator.SetTrigger(OnTripleJump);
                    EventManager.TriggerEvent("OnTripleJump", new Message(this));
                    break;
            }

            jumpCounter++;
            if (jumpCounter > 2) jumpCounter = 0;

            _jumpCounterReset.Cancel();
            _jumpCounterReset.Run();
        }
    }
}