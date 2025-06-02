using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Jars.DevTest
{
    public class AnimElementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] GameObject highlightedBG;

        public AnimationData Data { get; private set; }

        public UnityEvent OnClicked = new UnityEvent();

        public void Init(AnimationData data) 
        { 
            this.Data = data;
            text.text = data.clip.name;
        }

        public void TriggerClick() => OnClicked.Invoke();

        public bool Highlighted { set => highlightedBG.SetActive(value); }
    }
}