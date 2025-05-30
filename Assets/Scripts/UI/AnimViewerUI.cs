using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using R3;

namespace Jars.DevTest
{
    public class AnimViewerUI : MonoBehaviour
    {
        [SerializeField] AnimationLibrary library;
        [SerializeField] Transform tabElementParent;
        [SerializeField] GameObject tabElementPrefab;
        [SerializeField] Transform animElementParent;
        [SerializeField] GameObject animElementPrefab;
        [SerializeField] ViewerState state;

        List<GameObject> tabElements = new List<GameObject>();
        List<GameObject> animElements = new List<GameObject>();

        void Populate()
        {
            foreach (var e in tabElements)
                Destroy(e);
            tabElements.Clear();

            // Meow
            var cats = library.anims
                .Select(a => a.category)
                .Distinct();

            state.category.Value = cats.FirstOrDefault();

            foreach (var c in cats)
            {
                var newGo = Instantiate(tabElementPrefab, tabElementParent);
                tabElements.Add(newGo);

                var category = c;
                var element = newGo.GetComponent<CategoryElementUI>();
                element.Init(category);
                element.OnClicked.AddListener(() => state.category.Value = category);
            }

            PopulateAnims();
        }

        void PopulateAnims()
        {
            foreach (var e in animElements)
                Destroy(e);
            animElements.Clear();

            var filtered = library.anims
                .Where(a => a.category == state.category.Value);
            foreach (var a in filtered)
            {
                var newGo = Instantiate(animElementPrefab, animElementParent);
                animElements.Add(newGo);

                var element = newGo.GetComponent<AnimElementUI>();
                var anim = a;
                element.Init(a);
                element.OnClicked.AddListener(() => state.clip.Value = anim.clip);
            }
        }

        private void Start()
        {
            Populate();

            state.category
                .Subscribe(_ => PopulateAnims())
                .AddTo(this);
        }
    }
}