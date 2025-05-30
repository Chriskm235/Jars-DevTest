using TMPro;
using UnityEngine;

namespace Jars.DevTest
{
    public class AnimElementUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        public void Init(AnimationData data) => text.text = data.clip.name;
    }
}