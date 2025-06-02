using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using R3;
using UnityEngine.UI;
using TMPro;
using System;
using Animancer;

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
        [SerializeField] GameObject scrubberRoot;
        [SerializeField] Scrollbar scrubber;
        [SerializeField] TextMeshProUGUI speedText;
        [SerializeField] Image playButtonImage;
        [SerializeField] Sprite playSprite;
        [SerializeField] Sprite pauseSprite;
        [SerializeField] int[] scrubberSpeeds;

        [SerializeField] List<CategoryElementUI> tabElements = new List<CategoryElementUI>();
        [SerializeField] List<GameObject> animElements = new List<GameObject>();

        AnimancerState AnimState => state.animState.Value;

        void Populate()
        {
            foreach (var e in tabElements)
                Destroy(e.gameObject);
            tabElements.Clear();

            // Meow
            var cats = library.anims
                .Select(a => a.category)
                .Distinct();

            state.category.Value = cats.FirstOrDefault();

            foreach (var c in cats)
            {
                var newGo = Instantiate(tabElementPrefab, tabElementParent);
                var element = newGo.GetComponent<CategoryElementUI>();
                tabElements.Add(element);

                var category = c;
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
                element.OnClicked.AddListener(() =>
                {
                    if (!state.isTweening.Value) state.clipData.Value = anim;
                });
            }
        }

        private void Start()
        {
            Populate();

            state.category
                .Subscribe(_ => PopulateAnims())
                .AddTo(this);

            state.isTweening
                .CombineLatest(state.animState,(t,s) => !t && s!= null)
                .Subscribe(scrubberRoot.gameObject.SetActive)
                .AddTo(this);
            
            scrubber.onValueChanged.AddListener(v =>
            {
                if (AnimState != null)
                {
                    AnimState.NormalizedTime = v;
                    AnimState.IsPlaying = false;
                }
            });

            state.category
                .Subscribe(c =>
                {
                    foreach(var e in tabElements)
                        e.Highlighted = e.Category == c;
                })
                .AddTo(this);
        }

        private void Update()
        {
            scrubber.SetValueWithoutNotify((AnimState?.NormalizedTime ?? 0) % 1);

            if (AnimState != null)
            {
                speedText.text = AnimState.Speed + "x";
                playButtonImage.sprite = AnimState.IsPlaying ? playSprite : pauseSprite;
            }
        }

        public void PlayPause()
        {
            if (AnimState != null) AnimState.IsPlaying = !AnimState.IsPlaying;
        }

        public void IncreaseSpeed() => IterateSpeed(1);

        public void DecreaseSpeed() => IterateSpeed(-1);

        void IterateSpeed(int iteration)
        {
            var index = Mathf.Clamp(CurrentSpeedIndex + iteration, 0, scrubberSpeeds.Length - 1);
            if (AnimState != null) AnimState.Speed = scrubberSpeeds[index];
        }

        int CurrentSpeedIndex
        {
            get
            {
                if(AnimState == null) return 0;

                var currentIndex = 0;
                for (int a = 0; a < scrubberSpeeds.Length; a++)
                {
                    if (scrubberSpeeds[a] > AnimState.Speed)
                        break;
                    currentIndex = a;
                }
                
                return currentIndex;
            }
        }
    }
}