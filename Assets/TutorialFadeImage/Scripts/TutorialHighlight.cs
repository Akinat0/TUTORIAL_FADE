namespace Abu
{
    using UnityEngine;

    /// <summary>
    /// Highlights RectTransform or Renderer. Shouldn't be added to game object without RectTransform or Renderer.
    /// </summary>
    [ExecuteInEditMode]
    [AddComponentMenu("UI/Tutorial Highlight")]
    public class TutorialHighlight : MonoBehaviour
    {
        /// <summary>
        /// TutorialFadeImage to render hole.
        /// Note: "Automatically finds TutorialFadeImage in active scene. So it can't be null if there is any TutorialFadeImage in the scene. To stop rendering hole use component's enabled flag.
        /// </summary>
        [SerializeField] 
        [Tooltip("Automatically finds TutorialFadeImage in active scene. So it can't be null if there is any TutorialFadeImage in the scene. To stop rendering hole use component's enabled flag.")] 
        TutorialFadeImage tutorialFade;

        /// <summary>
        /// Hole scale. Useful for animations and tweaking.
        /// </summary>
        [SerializeField]
        [Tooltip("Hole scale. Useful for animations and tweaking.")]
        float holeScale = 1;

        /// <summary>
        /// Hole scale. Useful for animations and tweaking.
        /// </summary>
        public float HoleScale
        {
            get => holeScale;
            set => holeScale = value;
        }
        
        /// <summary>
        /// TutorialFadeImage to render hole.
        /// Note: Required to be set manually if object is created during runtime.
        /// </summary>
        public TutorialFadeImage TutorialFade
        {
            get => tutorialFade;
            set
            {
                //do nothing if tutorialFade the same as new
                if(tutorialFade == value)
                    return;
                
                //unregister tutorial hole from prev tutorialFade 
                if(tutorialFade != null)
                    tutorialFade.RemoveHole(Hole);
                
                tutorialFade = value;
                
                //register tutorial hole with new tutorialFade
                if(tutorialFade != null)
                    tutorialFade.AddHole(Hole);
            }
        }
        
        TutorialHole hole;

        /// <summary>
        /// Tutorial hole.
        /// </summary>
        TutorialHole Hole
        {
            get
            {
                if (hole == null)
                {
                    if(TryGetComponent(out RectTransform rectTransform))
                        hole = new RectTransformTutorialHole(rectTransform);
                    else if (TryGetComponent(out Renderer rendererComponent) && TutorialFade != null)
                        hole = new RendererTutorialHole(rendererComponent, TutorialFade);
                }

                return hole;
            }
        }

        void OnEnable()
        {
            if(TutorialFade != null)
                TutorialFade.AddHole(Hole);
        }

        void OnDisable()
        {
            if(TutorialFade != null)
                TutorialFade.RemoveHole(Hole);
        }

        void OnDestroy()
        {
            if(TutorialFade != null)
                TutorialFade.RemoveHole(Hole);
        }

#if UNITY_EDITOR

        void OnValidate()
        {
            // I don't wont to confuse people that they can add
            // new TutorialHighlight while they are playing in editor
            // without setting tutorialFade 
            if(Application.isPlaying)
                return;
            
            if (TutorialFade == null)
                TutorialFade = FindObjectOfType<TutorialFadeImage>();
            
            if(isActiveAndEnabled && tutorialFade != null)
                TutorialFade.AddHole(Hole);
        }
#endif
    }

}