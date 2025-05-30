using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using UnityEngine;

namespace Jars.DevTest
{
    public class AnimationBootstrap : MonoBehaviour
    {
        [SerializeField] AnimancerComponent anim;
        [SerializeField] AnimationClip[] clips;
        [SerializeField] AnimationClip idleClip;

        Vector3 animBasePos;
        Vector3 posVel;
        bool isTweening;

        private void Awake()
        {
            animBasePos = anim.transform.position;
        }

        private void OnGUI()
        {
            foreach (var c in clips)
                if (GUILayout.Button(c.name))
                    StartCoroutine(TweenToAnimation(c));
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