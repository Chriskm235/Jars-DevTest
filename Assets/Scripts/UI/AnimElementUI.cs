using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Jars.DevTest
{
    public class AnimElementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        public UnityEvent OnClicked = new UnityEvent();

        public void Init(AnimationData data) => text.text = data.clip.name;

        public void TriggerClick() => OnClicked.Invoke();
    }
}