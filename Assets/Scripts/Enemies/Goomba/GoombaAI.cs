using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Utilities;
using Utilities.Health;
using Random = UnityEngine.Random;

namespace Enemies.Goomba
{
    public class GoombaAI : HealthSystem
    {
        private static readonly int OnChase = Animator.StringToHash("OnChase");
        private static readonly int OnPatrol = Animator.StringToHash("OnPatrol");

        [SerializeField] private float maxPatrolTravelDistance;
        [SerializeField] [ReadOnly] private States currentState;

        private NavMeshAgent _agent;
        private Animator _animator;
        private Vector3 _currentDestination;
        private FieldOfView _fov;
        private Transform _player;
        private StateMachine<States> _stateMachine;

        protected override void OnInit()
        {
            _agent = GetComponent<NavMeshAgent>();
            _stateMachine = InitStateMachine();
            _fov = GetComponentInChildren<FieldOfView>();
            _animator = GetComponentInChildren<Animator>();
        }

        private StateMachine<States> InitStateMachine()
        {
            var sm = new StateMachine<States>();

            sm.OnStatePhase(States.Patrol, StatePhase.Enter, () =>
            {
                SetRandomDestination();
                _animator.SetTrigger(OnPatrol);
            });
            sm.OnStatePhase(States.Patrol, StatePhase.Stay, () =>
            {
                OnStayState(() =>
                {
                    if (HasReachedDestination()) SetRandomDestination();
                });
            });
            sm.OnStatePhase(States.Chase, StatePhase.Enter, () => _animator.SetTrigger(OnChase));
            sm.OnStatePhase(States.Chase, StatePhase.Stay,
                () => OnStayState(() => _agent.SetDestination(_player.position)));

            sm.CurrentState = States.Patrol;

            return sm;
        }

        private void OnStayState(Action action)
        {
            _player = _fov.VisibleTargets.FirstOrDefault();

            var next = _player ? States.Chase : States.Patrol;

            if (next != _stateMachine.CurrentState)
            {
                ChangeState(next);
                return;
            }

            action?.Invoke();
        }

        private bool HasReachedDestination()
        {
            return Vector3.Distance(_currentDestination, transform.position) < 2f;
        }


        private void ChangeState(States state)
        {
            _stateMachine.CurrentState = state;
            currentState = _stateMachine.CurrentState;
        }

        private void SetRandomDestination()
        {
            var direction = Random.insideUnitSphere * maxPatrolTravelDistance + transform.position;
            NavMesh.SamplePosition(direction, out var hit, maxPatrolTravelDistance, 1);
            _currentDestination = hit.position;
            _agent.SetDestination(_currentDestination);
        }

        private enum States
        {
            Patrol,
            Chase
        }
    }
}