using System.Collections;
using Animancer;
using UnityEngine;
using R3;
using System.Linq;
using System;

namespace Jars.DevTest
{
    public class ViewerAnimator : MonoBehaviour
    {
        [SerializeField] AnimancerComponent anim;
        [SerializeField] AnimationClip idleClip;
        [SerializeField] ViewerState state;
        [SerializeField] float fadeTime = .25f;

        private void Start()
        {
            state.clipData
                .Select(c => c ?? new AnimationData
                {
                    clip = idleClip
                })
                .Pairwise()
                .Subscribe(p => StartCoroutine(TweenToAnimation(p.Current, p.Previous)))
                .AddTo(this);
        }

        IEnumerator TweenToAnimation(AnimationData data, AnimationData prev)
        {
            if (state.isTweening.Value) yield break;
            state.isTweening.Value = true;

            yield return TweenOut(prev);
            yield return TweenIn(data);

            state.isTweening.Value = false;
        }

        IEnumerator TweenOut(AnimationData prev)
        {
            if (prev.reverseExit)
            {
                var animState = anim.Play(prev.clip, fadeTime);
                animState.NormalizedTime = animState.NormalizedTime > 1 ? 1 : animState.NormalizedTime;
                animState.Speed = -3;
                while (animState.NormalizedTime > 0)
                    yield return null;
                animState.Speed = 1;
            }
            if (prev.exitClip != null)
            {
                var animState = anim.Play(prev.exitClip, fadeTime);
                while (animState.NormalizedTime < 1)
                    yield return null;
            }
        }

        IEnumerator TweenIn(AnimationData data)
        {
            if (data.enterClip != null)
            {
               var animState = anim.Play(data.enterClip, fadeTime);

                while (animState.NormalizedTime < 1)
                    yield return null;
            }
            {
                var animState = anim.Play(data.clip, fadeTime);
                state.animState.Value = animState;
                yield return new WaitForSeconds(fadeTime);
            }
        }
    }
}