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
        [SerializeField] TMP_InputField searchField;
        [SerializeField] GameObject scrubberRoot;
        [SerializeField] Scrollbar scrubber;
        [SerializeField] TextMeshProUGUI speedText;
        [SerializeField] Image playButtonImage;
        [SerializeField] Sprite playSprite;
        [SerializeField] Sprite pauseSprite;
        [SerializeField] TextMeshProUGUI animNameText;
        [SerializeField] int[] scrubberSpeeds;

        // HACK: I keep these public so I can load them with default elements for sizing and such
        [SerializeField] List<CategoryElementUI> tabElements = new List<CategoryElementUI>();
        [SerializeField] List<AnimElementUI> animElements = new List<AnimElementUI>();

        AnimancerState AnimState => state.animState.Value;

        void Populate()
        {
            PopulateTabs();
            PopulateAnims();
        }

        void PopulateTabs()
        {
            // Clear all tab views
            foreach (var e in tabElements)
                Destroy(e.gameObject);
            tabElements.Clear();

            // Meow
            var cats = library.anims
                .Select(a => a.category)
                .Distinct();

            // Set the category to the first be default
            state.category.Value = cats.FirstOrDefault();

            // Many meows
            foreach (var c in cats)
            {
                // Create and init all the new views and add them to the list
                var newGo = Instantiate(tabElementPrefab, tabElementParent);
                var element = newGo.GetComponent<CategoryElementUI>();
                tabElements.Add(element);

                var category = c;
                element.Init(category);
                element.OnClicked.AddListener(() =>
                {
                    // Set the category and clear the search bar
                    state.category.Value = category;
                    state.search.Value = string.Empty;
                });
            }
        }

        void PopulateAnims()
        {
            // Clear all the anim views
            foreach (var e in animElements)
                Destroy(e.gameObject);
            animElements.Clear();

            // Filter by search if there is one, otherwise filter by category
            var filtered = !string.IsNullOrEmpty(state.search.Value) ?
                library.anims
                    .Where(a => a.clip.name.ToLower().Contains(state.search.Value.ToLower()))
                : library.anims
                    .Where(a => a.category == state.category.Value);

            // Create all the view elements
            foreach (var a in filtered)
            {
                // Instantiate the anim view element and add it to the list
                var newGo = Instantiate(animElementPrefab, animElementParent);
                var element = newGo.GetComponent<AnimElementUI>();
                animElements.Add(element);

                var anim = a;
                element.Init(a);
                element.OnClicked.AddListener(() =>
                {
                    // If you send the message when its tweening, its ignored
                    if (!state.isTweening.Value)
                        state.clipData.Value = anim;
                });
            }

            RefreshAnimHighlighted();
        }

        private void Start()
        {
            Populate();

            // Repopulate the anim views whenever the category changes
            state.category
                .Subscribe(_ => PopulateAnims())
                .AddTo(this);

            // Show the scrubber if its not tweening and there is an animation state to scrub with
            state.isTweening
                .CombineLatest(state.animState, (t, s) => !t && s != null)
                .Subscribe(scrubberRoot.gameObject.SetActive)
                .AddTo(this);

            // When the scrubber is moves, pause the animation and set the time to v
            scrubber.onValueChanged.AddListener(v =>
            {
                if (AnimState != null)
                {
                    AnimState.NormalizedTime = v;
                    AnimState.IsPlaying = false;
                }
            });

            // Refresh highlighted elements
            state.category
                .Subscribe(_ => RefreshTabsHighlighted())
                .AddTo(this);
            state.clipData
                .Subscribe(_ => RefreshAnimHighlighted())
                .AddTo(this);

            // When search field is entered, set the search state directly
            searchField
                .onValueChanged.AddListener(t => state.search.Value = t);

            state.search
                .Subscribe(s =>
                {
                    // Make sure these stay synced 
                    searchField.SetTextWithoutNotify(s);
                    // Refresh the anim views
                    PopulateAnims();
                    // Refresh the tab highlights, turning them all off
                    RefreshTabsHighlighted();
                });
        }

        private void Update()
        {
            // This will keep the bar in sync with the anim state
            scrubber.SetValueWithoutNotify((AnimState?.NormalizedTime ?? 0) % 1);


            if (AnimState != null)
            {
                // Keep the speed text and play buttons in sync every update
                speedText.text = AnimState.Speed + "x";
                playButtonImage.sprite = AnimState.IsPlaying ? pauseSprite : playSprite;
            }

            // Say "Tweening" if anything is tweening,
            // Say nothing if there is no animation
            // Say clip name if theres a clip
            animNameText.text = state.isTweening.Value ? "Tweening..." :
                AnimState == null ? string.Empty :
                AnimState.Clip.name;
        }

        // For external button use
        public void PlayPause()
        {
            if (AnimState != null) AnimState.IsPlaying = !AnimState.IsPlaying;
        }

        // For external button use
        public void Restart()
        {
            if (AnimState != null)
            {
                // if youre going backwards, set the clip at the end
                AnimState.NormalizedTime = AnimState.Speed > 0 ? 0 : 1;
            }
        }

        // For external button use
        public void IncreaseSpeed() => IterateSpeed(1);

        // For external button use
        public void DecreaseSpeed() => IterateSpeed(-1);

        // Turns the speed up or down relative to the scrubber speed options
        void IterateSpeed(int iteration)
        {
            var index = Mathf.Clamp(CurrentSpeedIndex + iteration, 0, scrubberSpeeds.Length - 1);
            if (AnimState != null) AnimState.Speed = scrubberSpeeds[index];
        }

        // A crude way of working backwards to determine the current speed relative
        // to all the scrubber speed options
        int CurrentSpeedIndex
        {
            get
            {
                if (AnimState == null) return 0;

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

        void RefreshAnimHighlighted()
        {
            foreach (var e in animElements)
            {
                // HACK: for some reason the clip names were different where one
                // clip name had its white spaces removed. I dunno...
                e.Highlighted = e.Data.clip.GetInstanceID() == state.clipData.Value?.clip?.GetInstanceID();
            }
        }

        void RefreshTabsHighlighted()
        {
            // Highlight the current category, turn off if there is a search active
            foreach (var e in tabElements)
                e.Highlighted = e.Category == state.category.Value && string.IsNullOrEmpty(state.search.Value);
        }
    }
}