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

        public UnityEvent OnClicked;

        public void Init(string category) => text.text = category;

        public void TriggerClick() => OnClicked.Invoke();

        public void Highlight() => button.Select();
    }
}