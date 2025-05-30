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
            while (isTweening)
                yield return null;
            isTweening = true;

            float fadeTime = .25f;
            if (prev.exitClip != null)
            {
                var state = anim.Play(prev.exitClip);
                var done = false;
                state.Events(this).OnEnd ??= () => done = true;
                while (!done)
                    yield return null;
            }
            else
            {
                anim.Play(idleClip, fadeTime);
                yield return new WaitForSeconds(fadeTime);
            }

            if (data.enterClip != null)
            {
                var state = anim.Play(data.enterClip);
                var done = false;
                state.Events(this).OnEnd ??= () => done = true;
                while (!done)
                    yield return null;
                anim.Play(data.clip);
            }
            else
                anim.Play(data.clip, fadeTime);

            //anim.transform.position = animBasePos;

            isTweening = false;
        }
    }
}