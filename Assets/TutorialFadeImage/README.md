# UI Tutorial Fade

Tutorial Fade is an easy-to-implement solution for creating tutorials that allows you to highlight UI elements or 3D renderers, allowing the user to click only on the highlighted elements.

[Link](https://assetstore.unity.com/packages/slug/230845) | [Documentation](https://akinat0.github.io/TUTORIAL_FADE/index.html)

## Description:

__TutorialFadeImage__ allows you to create full screen fade (similar with simple __Panel__), however custom shader will draw holes inside this fade.

Moreover, __TutorialFadeImage__ will prevent clicking on any zone except these "holes", so you can force user to click entire button, for example.

## Getting Started:

1. Add __Tutorial Fade Image__ game object to your canvas via GameObject->UI->Tutorial Fade Image.
2. Add to UI element or any Renderer on your canvas __Tutorial Highlight__ component via AddComponent->UI->Tutorial Highlight
3. To enable/disable hole you can enable/disable either __Tutorial Highlight__ component or game object this component attached to.
4. You're awesome! ❤️

## Issues:

### Overlay canvases
While using __RendererTutorialHole__ or just attaching __Tutorial Highlight__ component to renderer (not UI element) it's recommended to not use Canvases with render type __Overlay__.
Actually you can use __Overlay__ canvas, but it's important to add camera to them. For that you can change canvas render mode to __Camera__, then select your __Main Camera__ in appeared field and then change your canvas render mode back to __Overlay__.

## Max holes count
By default max holes size is set to 5. If you will try to create more holes, they won't appear and you will receive error message in console. However if you need, you can change this value. It is an advanced tip, because extending max holes count means changing source file, however if your lead instructions everything will be ok.
First of all find file called __TutorialFadeImage.cs__ and find there this line of code:

    const int HolesSize = 5;
Tou can change this value to anything you want. Technically the value can be any (even 128 or higher for example). However it's not tested.
Then you should find another file called __UITutorialFade.shader__, find there this line of code

    float4 _Holes[5];
And then change it to the same value.
All done!

## Properties:

### TutorialFadeImage

| __Property__                                                                      | __Description__                                                                                                                                                                          |
|-----------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __Source Image__                                                                  | This value could be used if you want to add texture to your fade image.                                                                                                                  |
| __Color__                                                                         | Color of the fade. If Source Image property provided, the sprite will be multiplied to this color                                                                                        |
| __Material__                                                                      | You can select your custom material here. Very advanced trick, it's not recommended to use this field.                                                                                   |
| __Smoothness__                                                                    | Property that allows you to change smoothness of hole edges (i.e. hole size). Allowed values: from 0 to 1.                                                                               |
| __Raycast Target__                                                                | This property allows you to pass click through __TutorialFadeImage__ if it's unchecked. It can be used of you using __TutorialFadeImage__ for visual effect, but not for click blocking. |


### Tutorial Highlight

| __Property__       | __Description__                                                                                                                                                                                         |
|--------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __Tutorial Fade__  | __TutorialFadeImage__ which will render hole for this object. The field is filled automatically, if there exists any __TutorialFadeImage__ in the scene and you could not change this value to __None__ |

## UITutorialFade.shader


| __Property__       | __Description__                                                                                                                                                                          |
|--------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| ___MainTex__       | Base color texture.                                                                                                                                                                      |
| ___Aspect__        | Aspect ratio of the UITutorialFade's quad.                                                                                                                                               |
| ___Smoothness__    | Property that allows you to change smoothness of hole edges (i.e. hole size). Should be greater then 0.                                                                |

