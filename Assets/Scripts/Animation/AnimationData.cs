using Animancer;
using UnityEngine;

namespace Jars.DevTest
{
    [CreateAssetMenu(fileName = "AnimationData", menuName = "Jars/AnimationData")]
    public class AnimationData : ScriptableObject
    {
        public AnimationClip clip;
        [SerializeReference]
        public ClipTransition enterClip;
        [SerializeReference]
        public ClipTransition exitClip;
        public string category;
        public bool reverseExit;
    }
}