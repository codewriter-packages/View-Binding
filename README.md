# View Binding [![Github license](https://img.shields.io/github/license/codewriter-packages/View-Binding.svg?style=flat-square)](#) [![Unity 2020.1](https://img.shields.io/badge/Unity-2020.1+-2296F3.svg?style=flat-square)](#) ![GitHub package.json version](https://img.shields.io/github/package-json/v/codewriter-packages/View-Binding?style=flat-square)
_View binding library for unity_

## How to use?

#### 1. Setup ViewContext
ViewContext contains the data necessary to display current part of the interface.

![View Context Preview](https://user-images.githubusercontent.com/26966368/120635821-8f571a00-c475-11eb-922e-2b69bce52b4e.png)

#### 2. Add applicators

Applicators reactively update components (Text, Slider, etc) when data changes in ViewContext.

![Text Applicator Preview](https://user-images.githubusercontent.com/26966368/120635942-b281c980-c475-11eb-9c4c-91826b75fdbd.png)

#### 3. Bind

Finally, you need to bind the variable to any `[Atom]`. Applicators will automatically update every time the atom value changes.

![Code Preview](https://user-images.githubusercontent.com/26966368/120636104-e6f58580-c475-11eb-9a63-bbc3534cc820.png)

```csharp
using UniMob;
using UnityEngine;
using CodeWriter.ViewBinding;

public class ViewBindingSample : MonoBehaviour
{
    public ViewVariableBool soundEnabled;
    public ViewVariableBool musicEnabled;
    public ViewVariableFloat volume;

    private ViewState State { get; } = new ViewState();

    private void Start()
    {
        soundEnabled.BindTo(() => State.SoundEnabled);
        musicEnabled.BindTo(() => State.MusicEnabled);
        volume.BindTo(() => State.Volume);
    }
}

public class ViewState
{
    [Atom] public bool SoundEnabled { get; set; }
    [Atom] public bool MusicEnabled { get; set; }
    [Atom] public float Volume { get; set; }
}
```

<details>
  <summary>BindTo extensions</summary>

```csharp
public static class BindingExtension
{
    public static void BindTo<TVariable, T>(this TVariable variable, AtomPull<T> f)
        where TVariable : ViewVariable<T, TVariable>
    {
        variable.SetSource(Atom.Computed(f));
    }
}
```
</details>

## How to Install
Minimal Unity Version is 2020.1.

Library distributed as git package ([How to install package from git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html))
<br>Git URL: `https://github.com/codewriter-packages/View-Binding.git`

## License

View-Binding is [MIT licensed](./LICENSE.md).
