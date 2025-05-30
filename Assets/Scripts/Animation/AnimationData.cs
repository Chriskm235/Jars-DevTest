using UnityEngine;

namespace Jars.DevTest
{
    [CreateAssetMenu(fileName = "AnimationData", menuName = "Jars/AnimationData")]
    public class AnimationData : ScriptableObject
    {
        public AnimationClip clip;
        public string category;
    }
}