using UnityEngine;
using UnityEngine.Serialization;
using Utilities;
using Utilities.Messaging;

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

        private Animator _animator;
        private CharacterController _controller;
        private CoroutineBuilder _counterReset;
        [SerializeField] [ReadOnly] private int jumpCounter;
        private float _maxSpeed;
        private bool _isRunning;
        private static readonly int OnLongJump = Animator.StringToHash("OnLongJump");

        private void Awake()
        {
            Messenger.Send<CharacterController>(controller => _controller = controller);
            Messenger.Send<Animator>(animator => _animator = animator);
            _maxSpeed = GameSettings.Instance.player.runningMovementSpeed;
            _counterReset = Coroutine().WaitForSeconds(GameSettings.Instance.player.jumpComboResetTime)
                .Invoke(() => jumpCounter = 0).DestroyOnFinish(false);
        }

        private void FixedUpdate()
        {
            var velocity = _controller.velocity;
            var horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            _animator.SetFloat(HorizontalSpeed, horizontalSpeed / _maxSpeed);
            _animator.SetFloat(VerticalSpeed, velocity.y);
            _animator.SetBool(IsGrounded, _controller.isGrounded);
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnPlayerJump", OnPlayerJump);
            EventManager.StartListening("OnPlayerRun", OnPlayerRun);
        }

        private void OnPlayerRun(Message message)
        {
            _isRunning = (bool) message["isRunning"];
        }

        private void OnPlayerJump(Message message)
        {
            var isJumping = (bool) message["isJumping"];
            if (!isJumping) return;

            if (_isRunning)
            {
                _animator.SetTrigger(OnLongJump);
                jumpCounter = 0;
                return;
            }

            switch (jumpCounter)
            {
                case 0:
                    _animator.SetTrigger(OnJump);
                    break;
                case 1:
                    _animator.SetTrigger(OnDoubleJump);
                    break;
                case 2:
                    _animator.SetTrigger(OnTripleJump);
                    break;
            }

            jumpCounter++;
            if (jumpCounter > 2) jumpCounter = 0;
            
            _counterReset.Cancel();
            _counterReset.Run();
        }
    }
}