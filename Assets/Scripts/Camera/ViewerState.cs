using UnityEngine;
using R3;
using Animancer;

namespace Jars.DevTest
{
    public class ViewerState : MonoBehaviour
    {
        // The rotation of the camera in 2D input screen space
        public ReactiveProperty<Vector2> inputRot = new ReactiveProperty<Vector2>();
        // The selected category
        public ReactiveProperty<string> category = new ReactiveProperty<string>();
        // The clip data of the current clip being played
        public ReactiveProperty<AnimationData> clipData = new ReactiveProperty<AnimationData>();
        // If the clip is tweening, try not to do anything while this is happening
        public ReactiveProperty<bool> isTweening = new ReactiveProperty<bool>();
        // The animancer state
        // NOTE: this is not reset everythime there is a change to the state itself
        // Keep checking value every update!
        public ReactiveProperty<AnimancerState> animState = new ReactiveProperty<AnimancerState>(null);
        // The current search query
        public ReactiveProperty<string> search = new ReactiveProperty<string>();
    }
}