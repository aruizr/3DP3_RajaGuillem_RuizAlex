using UnityEngine;
using Utilities.Messaging;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int HorizontalSpeed = Animator.StringToHash("Horizontal Speed");
        private static readonly int VerticalSpeed = Animator.StringToHash("Vertical Speed");
        private static readonly int OnJump = Animator.StringToHash("OnJump");
        private Animator _animator;

        private CharacterController _controller;
        private float _maxSpeed;

        private void Awake()
        {
            Messenger.Send<CharacterController>(controller => _controller = controller);
            Messenger.Send<Animator>(animator => _animator = animator);
            _maxSpeed = GameSettings.Instance.player.runningMovementSpeed;
        }

        private void FixedUpdate()
        {
            var velocity = _controller.velocity;
            var horizontalSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            _animator.SetFloat(HorizontalSpeed, horizontalSpeed / _maxSpeed);
            // var verticalSpeed = velocity.y;
            // _animator.SetFloat(VerticalSpeed, VerticalSpeed / _maxSpeed);
        }

        private void OnEnable()
        {
            EventManager.StartListening("OnPlayerJump", OnPlayerJump);
        }

        private void OnPlayerJump(Message message)
        {
            var isJumping = (bool) message["isJumping"];
            if (!isJumping) return;
            _animator.SetTrigger(OnJump);
        }
    }
}