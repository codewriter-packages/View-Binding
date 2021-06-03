# View Binding [![Github license](https://img.shields.io/github/license/codewriter-packages/View-Binding.svg?style=flat-square)](#) [![Unity 2020.1](https://img.shields.io/badge/Unity-2020.1+-2296F3.svg?style=flat-square)](#) ![GitHub package.json version](https://img.shields.io/github/package-json/v/codewriter-packages/View-Binding?style=flat-square)
_View binding library for unity_

## How to use?

1. Setup ViewContext

![View Context Preview](https://user-images.githubusercontent.com/26966368/120635821-8f571a00-c475-11eb-922e-2b69bce52b4e.png)

2. Add applicators

_Text Applicator_<br/>
![Text Applicator Preview](https://user-images.githubusercontent.com/26966368/120635942-b281c980-c475-11eb-9c4c-91826b75fdbd.png)

_Slider Applicator_<br/>
![Slider Applicator Preview](https://user-images.githubusercontent.com/26966368/120635982-c299a900-c475-11eb-8105-0c631b1da239.png)

_Unity Event Applicator_<br/>
![Unity Event Applicator Preview](https://user-images.githubusercontent.com/26966368/120636032-d218f200-c475-11eb-9ef8-53e8f13646d0.png)
<br/>or write you own applicators

3. Control from code

![Code Preview](https://user-images.githubusercontent.com/26966368/120636104-e6f58580-c475-11eb-9a63-bbc3534cc820.png)

```csharp
using UnityEngine;
using CodeWriter.ViewBinding;

public class ViewBindingSample : MonoBehaviour
{
    public ViewVariableBool soundEnabled = default;
    public ViewVariableBool musicEnabled = default;
    public ViewVariableFloat health = default;
    public ViewVariableString userName = default;

    private void Start()
    {
        soundEnabled.Value = true;
        musicEnabled.Value = false;
        health.Value = 60;
        userName.Value = "Hello!";
    }
}
```


## How to Install
Minimal Unity Version is 2020.1.

Library distributed as git package ([How to install package from git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html))
<br>Git URL: `https://github.com/codewriter-packages/View-Binding.git`

## License

View-Binding is [MIT licensed](./LICENSE.md).
