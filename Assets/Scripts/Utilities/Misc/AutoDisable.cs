using UnityEngine;

namespace Utilities.Misc
{
    public class AutoDisable : ExtendedMonoBehaviour
    {
        [SerializeField] private float delay;

        private CoroutineBuilder _coroutine;

        private void OnEnable()
        {
            _coroutine ??= Coroutine().WaitForSeconds(delay).Invoke(() => gameObject.SetActive(false));
            _coroutine.Run();
        }

        private void OnDisable()
        {
            _coroutine.Cancel();
        }
    }
}