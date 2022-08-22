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
                    else if (TryGetComponent(out Renderer rendererComponent) && tutorialFade != null)
                        hole = new RendererTutorialHole(rendererComponent, tutorialFade);
                }

                return hole;
            }
        }

        void OnEnable()
        {
            if(tutorialFade != null)
                tutorialFade.AddHole(Hole);
        }

        void OnDisable()
        {
            if(tutorialFade != null)
                tutorialFade.RemoveHole(Hole);
        }

        void OnDestroy()
        {
            if(tutorialFade != null)
                tutorialFade.RemoveHole(Hole);
        }

#if UNITY_EDITOR

        void OnValidate()
        {
            if (tutorialFade == null)
                tutorialFade = FindObjectOfType<TutorialFadeImage>();
            
            if(isActiveAndEnabled && tutorialFade != null)
                tutorialFade.AddHole(Hole);
        }
#endif
    }

}