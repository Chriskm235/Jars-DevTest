using System.Collections;
using Animancer;
using UnityEngine;
using R3;
using System.Linq;

namespace Jars.DevTest
{
    public class ViewerAnimator : MonoBehaviour
    {
        [SerializeField] AnimancerComponent anim;
        [SerializeField] AnimationClip idleClip;
        [SerializeField] ViewerState state;

        Vector3 animBasePos;
        bool isTweening;

        private void Awake() => animBasePos = anim.transform.position;

        private void Start()
        {
            state.clip
                .Select(c => c ?? idleClip)
                .Subscribe(c => StartCoroutine(TweenToAnimation(c)))
                .AddTo(this);
        }

        IEnumerator TweenToAnimation(AnimationClip clip)
        {
            while (isTweening)
                yield return null;
            isTweening = true;

            float fadeTime = .25f;
            anim.Play(idleClip, fadeTime);
            yield return new WaitForSeconds(fadeTime);
            anim.transform.position = animBasePos;

            anim.Play(clip, fadeTime);

            isTweening = false;
        }
    }
}