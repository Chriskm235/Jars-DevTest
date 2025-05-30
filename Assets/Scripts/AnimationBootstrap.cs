using System.Collections;
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
            float fadeTime = 1f;
            anim.Play(idleClip, fadeTime / 2);
            yield return new WaitForSeconds(fadeTime / 2);
            anim.Play(clip, fadeTime / 2);
        }
    }
}