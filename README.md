# View Binding [![Github license](https://img.shields.io/github/license/codewriter-packages/View-Binding.svg?style=flat-square)](#) [![Unity 2020.1](https://img.shields.io/badge/Unity-2020.1+-2296F3.svg?style=flat-square)](#) ![GitHub package.json version](https://img.shields.io/github/package-json/v/codewriter-packages/View-Binding?style=flat-square) [![openupm](https://img.shields.io/npm/v/com.codewriter.view-binding?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.codewriter.view-binding/)
_View binding library for unity_

#### NOTE: To use View Binding library you need to install [Tri Inspector](https://github.com/codewriter-packages/Tri-Inspector) - Free and open-source library that improves unity inspector.

## How to use?

#### 1. Setup ViewContext
ViewContext contains the data necessary to display current part of the interface.

![View Context Preview](https://user-images.githubusercontent.com/26966368/160893683-222809f4-8753-41ca-967e-78864f4c75e6.png)

#### 2. Add applicators and Binders

Applicators reactively update components (Text, Slider, etc) when data changes in ViewContext. 
Binders subscribes to events and pass them to ViewContext.

![Text Applicator Preview](https://user-images.githubusercontent.com/26966368/160635846-c5fcbf6f-633b-4eda-b14e-ac4783a07cf2.png)

#### 3. Set values from code

![Code Preview](https://user-images.githubusercontent.com/26966368/160636024-ee024ecf-98a3-4571-b29b-8638fb80e7d1.png)

```csharp
using UniMob;
using UnityEngine;
using CodeWriter.ViewBinding;

public class ViewBindingSample : MonoBehaviour
{
    public ViewVariableBool soundEnabled;
    public ViewVariableBool musicEnabled;
    public ViewVariableFloat volume;

    public ViewEventVoid onClose;

    private void Start()
    {
        soundEnabled.SetValue(true);
        soundEnabled.SetValue(false);
        volume.SetValue(0.5f);

        onClose.AddListener(() => Debug.Log("Close clicked"));
    }
}
```

## Documentation

#### Builtin variable types:
- Boolean (ViewVariableBool)
- Integer (ViewVariableInt)
- Float (ViewVariableFloat)
- String (ViewVariableString)

#### Builtin event types:
- Void (ViewEventVoid)
- Boolean (ViewEventBool)
- Integer (ViewEventInt)
- Float (ViewEventFloat)
- String (ViewEventString)

#### Builtin applicators:
- UnityEvent ([Bool](./Runtime/Applicators/UnityEvent/UnityEventBoolApplicator.cs), [Float](./Runtime/Applicators/UnityEvent/UnityEventFloatApplicator.cs), [Integer](./Runtime/Applicators/UnityEvent/UnityEventIntApplicator.cs), [String](./Runtime/Applicators/UnityEvent/UnityEventStringApplicator.cs))
- [GameObject - Activity](./Runtime/Applicators/GameObjectActivityApplicator.cs)
- [UI CanvasGroup - Alpha](./Runtime/Applicators/UI/CanvasGroupAlphaApplicator.cs)
- [UI CanvasGroup - Interactable](./Runtime/Applicators/UI/CanvasGroupInteractableApplicator.cs)
- [UI CanvasGroup - RaycastTarget](./Runtime/Applicators/UI/CanvasGroupRaycastTargetApplicator.cs)
- [UI CanvasGroup - Visibility](./Runtime/Applicators/UI/CanvasGroupVisibilityApplicator.cs)
- [UI Button - Interactable](./Runtime/Applicators/UI/ButtonInteractableApplicator.cs)
- [UI Image - Enabled](./Runtime/Applicators/UI/ImageEnabledApplicator.cs)
- [UI Image - Fill Amount](./Runtime/Applicators/UI/ImageFillAmountApplicator.cs)
- [UI InputField - Text](./Runtime/Applicators/UI/InputFieldApplicator.cs)
- [UI Slider - Value](./Runtime/Applicators/UI/SliderValueApplicator.cs)
- [UI Text - Text](./Runtime/Applicators/UI/TextApplicator.cs)
- [UI TextMeshPro - Text](./Runtime/Applicators/UI/TMPTextApplicator.cs)
- [UI TextMeshPro - Formatted Text](./Runtime/Applicators/UI/FormattedTMPTextApplicator.cs)
- [UI TextMeshPro - Localized Text](./Runtime/Applicators/UI/LocalizedTMPTextApplicator.cs)
- [UI Toggle - IsOn](./Runtime/Applicators/UI/ToggleApplicator.cs)

#### Builtin adapters:
- [Bool To String](./Runtime/Applicators/Adapters/BoolToStringAdapter.cs)
- [Bool To Formatted String](./Runtime/Applicators/Adapters/BoolToFormattedStringAdapter.cs)
- [Formatted Text](./Runtime/Applicators/Adapters/FormattedTextAdapter.cs)
- [Compare String](./Runtime/Applicators/Adapters/CompareStringAdapter.cs)
- [Float Format](./Runtime/Applicators/Adapters/FloatFormatAdapter.cs)
- [Float Ratio](./Runtime/Applicators/Adapters/FloatRatioAdapter.cs)
- [Inverse Boolean](./Runtime/Applicators/Adapters/InverseBoolAdapter.cs)
- [Text Localize](./Runtime/Applicators/Adapters/TextLocalizeAdapter.cs)
- [Time Localize](./Runtime/Applicators/Adapters/TimeLocalizeAdapter.cs)

#### Builtin binders:
- [UI Button - Click](./Runtime/Binders/UI/ButtonClickBinder.cs)
- [UI Toggle - ValueChanged](./Runtime/Binders/UI/ToggleValueChangedBinder.cs)
- [UI Slider - ValueChanged](./Runtime/Binders/UI/SliderValueChangedBinder.cs)
- [UI InputField - TextChanged](./Runtime/Binders/UI/InputFieldTextChangedBinder.cs)
- [UI InputField - EndEdit](./Runtime/Binders/UI/InputFieldEndEditBinder.cs)

## How to Install
Minimal Unity Version is 2020.1.

Library distributed as git package ([How to install package from git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html))
<br>Git URL: `https://github.com/codewriter-packages/View-Binding.git`

## License

View-Binding is [MIT licensed](./LICENSE.md).
