using UnityEngine;

namespace Jars.DevTest
{
    [CreateAssetMenu(fileName = "AnimationLibrary", menuName = "Jars/AnimationLibrary")]
    public class AnimationLibrary : ScriptableObject
    {
        public AnimationData[] anims;
    }
}