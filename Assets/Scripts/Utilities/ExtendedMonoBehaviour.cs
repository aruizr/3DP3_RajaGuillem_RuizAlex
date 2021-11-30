using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities
{
    public class ExtendedMonoBehaviour : MonoBehaviour
    {
        protected CoroutineBuilder Coroutine()
        {
            return gameObject.AddComponent<CoroutineBuilder>();
        }

        protected class CoroutineBuilder : MonoBehaviour
        {
            private readonly Queue<ExecutionStep> _sequence = new Queue<ExecutionStep>();

            private bool _destroyOnFinish = true;
            private Coroutine _coroutine;
            public bool IsRunning { get; private set; }

            private void OnDisable()
            {
                Cancel();
            }

            public CoroutineBuilder Invoke(Action action)
            {
                _sequence.Enqueue(new ExecutionStep(StepType.Invoke, action));
                return this;
            }

            public CoroutineBuilder WaitForSeconds(float seconds)
            {
                _sequence.Enqueue(new ExecutionStep(StepType.WaitForSeconds, seconds));
                return this;
            }

            public CoroutineBuilder ForTimes(int times)
            {
                _sequence.Enqueue(new ExecutionStep(StepType.ForTimes, times));
                return this;
            }

            public CoroutineBuilder While(Func<bool> predicate)
            {
                _sequence.Enqueue(new ExecutionStep(StepType.While, predicate));
                return this;
            }

            public void Run()
            {
                _coroutine = StartCoroutine(RunCoroutine());
            }

            public CoroutineBuilder WaitForEndOfFrame()
            {
                _sequence.Enqueue(new ExecutionStep(StepType.WaitForEndOfFrame, null));
                return this;
            }

            public CoroutineBuilder WaitForFixedUpdate()
            {
                _sequence.Enqueue(new ExecutionStep(StepType.WaitForFixedUpdate, null));
                return this;
            }

            public CoroutineBuilder WaitUntil(Func<bool> predicate)
            {
                _sequence.Enqueue(new ExecutionStep(StepType.WaitUntil, predicate));
                return this;
            }

            public CoroutineBuilder WaitWhile(Func<bool> predicate)
            {
                _sequence.Enqueue(new ExecutionStep(StepType.WaitWhile, predicate));
                return this;
            }

            public CoroutineBuilder DestroyOnFinish(bool condition = true)
            {
                _destroyOnFinish = condition;
                return this;
            }

            public void Cancel()
            {
                if (IsRunning)
                {
                    StopCoroutine(_coroutine);
                    IsRunning = false;
                }
                if (_destroyOnFinish) Destroy(this);
            }

            // private async Task RunAsThread()
            // {
            //     IsRunning = true;
            //
            //     var iterations = 0;
            //     var start = 0;
            //
            //     for (var i = 0; i < _sequence.Count; i++)
            //     {
            //         var step = _sequence.ElementAt(i);
            //
            //         switch (step.Type)
            //         {
            //             case StepType.Invoke:
            //                 var action = (Action) step.Value;
            //                 action();
            //                 break;
            //             case StepType.WaitForSeconds:
            //                 await Task.Delay((int) ((float) step.Value * 1000));
            //                 break;
            //             case StepType.WaitForEndOfFrame:
            //                 break;
            //             case StepType.WaitForFixedUpdate:
            //                 break;
            //             case StepType.WaitUntil:
            //                 await TaskWaitWhile(() =>
            //                 {
            //                     var predicate = (Func<bool>) step.Value;
            //                     return !predicate();
            //                 });
            //                 break;
            //             case StepType.WaitWhile:
            //                 await TaskWaitWhile((Func<bool>) step.Value);
            //                 break;
            //             case StepType.ForTimes when iterations < (int) step.Value - 1:
            //                 i = start - 1;
            //                 iterations++;
            //                 break;
            //             case StepType.ForTimes:
            //                 iterations = 0;
            //                 start = i + 1;
            //                 break;
            //             case StepType.While when ((Func<bool>) step.Value).Invoke():
            //                 i = start - 1;
            //                 iterations++;
            //                 break;
            //             case StepType.While:
            //                 iterations = 0;
            //                 start = i + 1;
            //                 break;
            //             default:
            //                 throw new ArgumentOutOfRangeException();
            //         }
            //     }
            //
            //     IsRunning = false;
            //     if (_destroyOnFinish) Destroy(this);
            // }

            // private static async Task TaskWaitWhile(Func<bool> predicate)
            // {
            //     while (predicate()) await Task.Yield();
            // }

            private IEnumerator RunCoroutine()
            {
                IsRunning = true;

                var iterations = 0;
                var start = 0;

                for (var i = 0; i < _sequence.Count; i++)
                {
                    var step = _sequence.ElementAt(i);

                    switch (step.Type)
                    {
                        case StepType.Invoke:
                            ((Action) step.Value).Invoke();
                            break;
                        case StepType.WaitForSeconds:
                            yield return StartCoroutine(RunWaitForSeconds((float) step.Value));
                            break;
                        case StepType.WaitForEndOfFrame:
                            yield return StartCoroutine(RunWaitForEndOfFrame());
                            break;
                        case StepType.WaitForFixedUpdate:
                            yield return StartCoroutine(RunWaitForFixedUpdate());
                            break;
                        case StepType.WaitUntil:
                            yield return StartCoroutine(RunWaitUntil((Func<bool>) step.Value));
                            break;
                        case StepType.WaitWhile:
                            yield return StartCoroutine(RunWaitWhile((Func<bool>) step.Value));
                            break;
                        case StepType.ForTimes when iterations < (int) step.Value - 1:
                            i = start - 1;
                            iterations++;
                            break;
                        case StepType.ForTimes:
                            iterations = 0;
                            start = i + 1;
                            break;
                        case StepType.While when ((Func<bool>) step.Value).Invoke():
                            i = start - 1;
                            iterations++;
                            break;
                        case StepType.While:
                            iterations = 0;
                            start = i + 1;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                IsRunning = false;
                if (_destroyOnFinish) Destroy(this);
            }

            private static IEnumerator RunWaitForSeconds(float seconds)
            {
                yield return new WaitForSeconds(seconds);
            }

            private static IEnumerator RunWaitForEndOfFrame()
            {
                yield return new WaitForEndOfFrame();
            }

            private static IEnumerator RunWaitForFixedUpdate()
            {
                yield return new WaitForFixedUpdate();
            }

            private static IEnumerator RunWaitUntil(Func<bool> predicate)
            {
                yield return new WaitUntil(predicate);
            }

            private static IEnumerator RunWaitWhile(Func<bool> predicate)
            {
                yield return new WaitWhile(predicate);
            }

            private enum StepType
            {
                Invoke,
                WaitForSeconds,
                WaitForEndOfFrame,
                ForTimes,
                While,
                WaitForFixedUpdate,
                WaitUntil,
                WaitWhile
            }

            private readonly struct ExecutionStep
            {
                public readonly StepType Type;
                public readonly object Value;

                public ExecutionStep(StepType type, object value)
                {
                    Type = type;
                    Value = value;
                }
            }
        }
    }
}