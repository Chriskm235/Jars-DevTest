using UnityEngine;
using R3;

namespace Jars.DevTest
{
    public class ViewerState : MonoBehaviour
    {
        public ReactiveProperty<Vector2> inputRot = new ReactiveProperty<Vector2>();
        public ReactiveProperty<string> category = new ReactiveProperty<string>();
        public ReactiveProperty<AnimationData> clipData = new ReactiveProperty<AnimationData>();
        public ReactiveProperty<bool> isTweening = new ReactiveProperty<bool>();
    }
}