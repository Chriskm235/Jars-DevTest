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
        [SerializeField] AnimationData idleClip;
        [SerializeField] ViewerState state;
        [SerializeField] float fadeTime = .25f;

        private void Start()
        {
            // Subscribe to the clip state in pairs so you always know what your exit animation was
            state.clipData
                .Select(c => c ?? idleClip)
                .Pairwise()
                .Subscribe(p => StartCoroutine(TweenToAnimation(p.Current, p.Previous)))
                .AddTo(this);

            StartCoroutine(TweenToAnimation(idleClip, null));
        }

        IEnumerator TweenToAnimation(AnimationData data, AnimationData prev)
        {
            // Discard all messages sent when tweening
            if (state.isTweening.Value) yield break;
            // Lock tweening
            state.isTweening.Value = true;

            // Tween out of the previous and into the next
            yield return TweenOut(prev);
            yield return TweenIn(data);

            // Unlock tweening
            state.isTweening.Value = false;
        }

        IEnumerator TweenOut(AnimationData prev)
        {
            if (prev == null)
                yield break;

            if (prev.reverseExit)
            {
                // Play a reverse of the animation
                var animState = anim.Play(prev.clip, fadeTime);
                // Clamp time to the begining or end of the animation
                animState.NormalizedTime = Mathf.Clamp01(animState.NormalizedTime);
                // Reverse at 3x
                animState.Speed = -3;
                // Wait for your time to be back to 0
                while (animState.NormalizedTime > 0)
                    yield return null;
                // Return the layer back to normal speed
                animState.Speed = 1;
            }
            if (prev.exitClip != null)
            {
                // Play and await exit clip
                var animState = anim.Play(prev.exitClip, fadeTime);

                while (animState.NormalizedTime < 1)
                    yield return null;
            }
        }

        IEnumerator TweenIn(AnimationData data)
        {
            if (data.enterClip != null)
            {
                // Play enter clip
                var animState = anim.Play(data.enterClip, fadeTime);

                while (animState.NormalizedTime < 1)
                    yield return null;
            }
            {
                // Fade in the new animation
                var animState = anim.Play(data.clip, fadeTime);
                state.animState.Value = animState;
                // Await the end of the fade
                yield return new WaitForSeconds(fadeTime);
            }
        }
    }
}