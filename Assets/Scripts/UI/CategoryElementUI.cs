using TMPro;
using UnityEngine;

namespace Jars.DevTest
{
    public class CategoryElementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        public void Init(string category) => text.text = category;
    }
}