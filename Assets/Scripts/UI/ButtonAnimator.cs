using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class ButtonAnimator : MonoBehaviour
    {
        [SerializeField] private float scale = 1.1f;
        [SerializeField] private float defaultScale = 1;
        [SerializeField] private float animDuration = 0.3f;

        public void EnterAnimation()
        {
            transform.DOScale(scale, animDuration);
        }
        
        public void ExitAnimation()
        {
            transform.DOScale(defaultScale, animDuration);
        }
    }
}