using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jars.DevTest
{
    public class AnimViewerUI : MonoBehaviour
    {
        [SerializeField] AnimationLibrary library;
        [SerializeField] Transform tabElementParent;
        [SerializeField] GameObject tabElementPrefab;
        [SerializeField] Transform animElementParent;
        [SerializeField] GameObject animElementPrefab;

        List<GameObject> tabElements = new List<GameObject>();
        List<GameObject> animElements = new List<GameObject>();
        string selectedCategory;

        void Populate()
        {
            foreach (var e in tabElements)
                Destroy(e);
            tabElements.Clear();

            // Meow
            var cats = library.anims
                .Select(a => a.category)
                .Distinct();

            selectedCategory = cats.FirstOrDefault();

            foreach (var c in cats)
            {
                var newGo = Instantiate(tabElementPrefab, tabElementParent);
                tabElements.Add(newGo);

                var category = c;
                var element = newGo.GetComponent<CategoryElementUI>();
                element.Init(category);
                element.OnClicked.AddListener(() =>
                {
                    selectedCategory = category;
                    PopulateAnims();
                });
            }

            PopulateAnims();
        }

        void PopulateAnims()
        {
            foreach (var e in animElements)
                Destroy(e);
            animElements.Clear();

            var filtered = library.anims
                .Where(a => a.category == selectedCategory);
            foreach (var a in filtered)
            {
                var newGo = Instantiate(animElementPrefab, animElementParent);
                animElements.Add(newGo);

                var element = newGo.GetComponent<AnimElementUI>();
                var anim = a;
                element.Init(a);
                element.OnClicked.AddListener(() =>
                {
                    //Play Animation
                    Debug.Log(anim.name);
                });
            }
        }

        private void Start()
        {
            Populate();
        }
    }
}