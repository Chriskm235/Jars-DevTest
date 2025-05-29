using Animancer;
using UnityEngine;

namespace Jars.DevTest
{
    public class AnimationBootstrap : MonoBehaviour
    {
        [SerializeField] AnimancerComponent anim;
        [SerializeField] AnimationClip[] clips;

        private void OnGUI()
        {
            foreach (var c in clips)
                if (GUILayout.Button(c.name))
                    anim.Play(c);
        }
    }
}