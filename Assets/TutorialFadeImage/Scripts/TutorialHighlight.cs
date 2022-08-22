namespace Abu
{
    using UnityEngine;

    [ExecuteInEditMode]
    [AddComponentMenu("UI/Tutorial Highlight")]
    public class TutorialHighlight : MonoBehaviour
    {
        [SerializeField] TutorialFadeImage tutorialFade;

        TutorialHole hole;

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