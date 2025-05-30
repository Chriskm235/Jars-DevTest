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

        List<GameObject> currentElements = new List<GameObject>();
        string selectedCategory;

        void Populate()
        {
            foreach (var e in currentElements)
                Destroy(e);
            currentElements.Clear();

            // Meow
            var cats = library.anims
                .Select(a => a.category)
                .Distinct();

            selectedCategory = cats.FirstOrDefault();

            foreach (var c in cats)
            {
                var newGo = Instantiate(tabElementPrefab, tabElementParent);
                currentElements.Add(newGo);
                newGo.GetComponent<CategoryElementUI>().Init(c);
            }

            var filtered = library.anims
                .Where(a => a.category == selectedCategory);
            foreach (var a in filtered)
            {
                var newGo = Instantiate(animElementPrefab, animElementParent);
                currentElements.Add(newGo);
                newGo.GetComponent<AnimElementUI>().Init(a);
            }
        }

        private void Start() => Populate();
    }
}