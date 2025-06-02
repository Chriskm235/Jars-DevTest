using Animancer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jars.DevTest
{
    public class CategoryElementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Button button;
        [SerializeField] GameObject selectedOverlay;

        public UnityEvent OnClicked;

        public string Category { get; private set; }

        public void Init(string category)
        {
            Category = category;
            text.text = category;
        }

        public void TriggerClick() => OnClicked.Invoke();

        public bool Highlighted { set => selectedOverlay.SetActive(value); }
    }
}