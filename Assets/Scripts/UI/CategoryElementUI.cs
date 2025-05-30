using Animancer;
using TMPro;
using UnityEngine;

namespace Jars.DevTest
{
    public class CategoryElementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        public UnityEvent OnClicked;

        public void Init(string category) => text.text = category;

        public void TriggerClick() => OnClicked.Invoke();
    }
}