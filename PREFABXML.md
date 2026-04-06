# View Binding — PrefabXML Integration Guide

Guide for using View Binding components in `.prefabxml` markup.

## Key Concepts

- **ViewContext** — container with variables and events, added to root GameObject.
- **Applicators** — reactively update UI components when a variable changes.
- **Binders** — listen to UI events and fire ViewContext events.
- **Adapters** — transform variable values (bool→string, float ratio, etc).

## ViewContext Variables and Events

Variables and events are `[SerializeReference]` lists on ViewContext. Define them using `<Ref>` inside the ViewContext tag and reference via `<Item rid="...">`:

```xml
<ViewContext renderOnStart="true">
    <Ref id="myVar" type="CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewVariableString"
        name="myVar" context="#Root" />
    <Ref id="myEvent" type="CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventVoid"
        name="myEvent" context="#Root" />
    <Field name="vars">
        <Item rid="myVar" />
    </Field>
    <Field name="evts">
        <Item rid="myEvent" />
    </Field>
    <Field name="listeners">
        <Item ref="#SomeApplicator" />
        <Item ref="#SomeBinder" />
    </Field>
</ViewContext>
```

### Available types for Ref

#### Variable types

| Type attribute | Value type |
|---|---|
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewVariableBool` | bool |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewVariableInt` | int |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewVariableFloat` | float |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewVariableString` | string |

#### Event types

| Type attribute | Parameter |
|---|---|
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventVoid` | none |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventBool` | bool |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventInt` | int |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventFloat` | float |
| `CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventString` | string |

## Dot Notation for Binding Fields

Applicators, binders, and controllers have inline `ViewVariable*` / `ViewEvent*` fields with two sub-fields:

- `context` — reference to `ViewContextBase` component (use `#id`)
- `name` — string name matching a variable/event in that context

Use dot notation in XML attributes:

```xml
<TMPTextApplicator target="#Display"
    source.context="#Root" source.name="myVariable" />

<ButtonClickBinder button="#Btn"
    onClick.context="#Root" onClick.name="myEvent" />
```

## Setup Checklist

1. Add `<ViewContext />` on the root GameObject (give it an `id`)
2. Define variables and events via `<Ref>`, wire into `vars`/`evts` via `<Item rid="...">`
3. Add `listeners` with `<Item ref="#id">` pointing to all applicators/binders
4. Add applicators/binders on child GameObjects, wiring fields via dot notation

## Example

```xml
<UnityPrefab>
    <GameObject name="Counter" id="Root">
        <RectTransform m_SizeDelta="200, 80" />
        <Image m_Color="#1E1E2E" />
        <ViewContext renderOnStart="true">
            <Ref id="counter" type="CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewVariableInt"
                name="counter" context="#Root" />
            <Ref id="increment" type="CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventVoid"
                name="increment" context="#Root" />
            <Ref id="decrement" type="CodeWriter.ViewBinding CodeWriter.ViewBinding.ViewEventVoid"
                name="decrement" context="#Root" />
            <Field name="vars">
                <Item rid="counter" />
            </Field>
            <Field name="evts">
                <Item rid="increment" />
                <Item rid="decrement" />
            </Field>
            <Field name="listeners">
                <Item ref="#DecrementBtn" />
                <Item ref="#Display" />
                <Item ref="#IncrementBtn" />
            </Field>
        </ViewContext>
        <CounterController
            counter.context="#Root" counter.name="counter"
            increment.context="#Root" increment.name="increment"
            decrement.context="#Root" decrement.name="decrement" />

        <GameObject name="DecrementBtn" id="DecrementBtn">
            <Image m_Color="#FF5577" />
            <Button />
            <ButtonClickBinder button="#DecrementBtn"
                onClick.context="#Root" onClick.name="decrement" />
        </GameObject>

        <GameObject name="Display" id="Display">
            <TextMeshProUGUI m_text="0" m_fontSize="36" m_fontColor="#FFFFFF"
                m_HorizontalAlignment="Center" m_VerticalAlignment="Middle" />
            <FormattedTMPTextApplicator target="#Display" format="&lt;counter&gt;" />
        </GameObject>

        <GameObject name="IncrementBtn" id="IncrementBtn">
            <Image m_Color="#44D962" />
            <Button />
            <ButtonClickBinder button="#IncrementBtn"
                onClick.context="#Root" onClick.name="increment" />
        </GameObject>
    </GameObject>
</UnityPrefab>
```

---

## Component Reference

### ViewContext

Container for variables and events. Add to root GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `renderOnStart` | bool | Call Render() on Start |

Variables (`vars`) and events (`evts`) are configured in Inspector.

---

### Binders

#### ButtonClickBinder

Fires a void event on button click. Requires `Button` on same GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `button` | Button | Target button (`#id` reference) |
| `onClick` | ViewEventVoid | `.context` = ViewContext ref, `.name` = event name |

#### ToggleValueChangedBinder

Fires a bool event when toggle value changes. Requires `Toggle` on same GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `toggle` | Toggle | Target toggle (`#id` reference) |
| `onToggle` | ViewEventBool | `.context` = ViewContext ref, `.name` = event name |

#### SliderValueChangedBinder

Fires a float event when slider value changes. Requires `Slider` on same GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `slider` | Slider | Target slider (`#id` reference) |
| `onValueChanged` | ViewEventFloat | `.context` = ViewContext ref, `.name` = event name |

#### InputFieldTextChangedBinder

Fires a string event when input field text changes. Requires `InputField` on same GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `inputField` | InputField | Target input field (`#id` reference) |
| `onTextChanged` | ViewEventString | `.context` = ViewContext ref, `.name` = event name |

#### InputFieldEndEditBinder

Fires a string event when input field editing ends. Requires `InputField` on same GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `inputField` | InputField | Target input field (`#id` reference) |
| `onEndEdit` | ViewEventString | `.context` = ViewContext ref, `.name` = event name |

---

### Applicators

All applicators based on `ComponentApplicatorBase<TTarget, TVariable>` share these fields:

| Field | Type | Description |
|-------|------|-------------|
| `target` | TTarget | Target component (`#id` reference) |
| `source` | TVariable | `.context` = ViewContext ref, `.name` = variable name |

#### TMPTextApplicator

Sets `TMP_Text.text` from a string variable. Requires `TMP_Text` on same GameObject.

- `target`: TMP_Text
- `source`: ViewVariableString

#### TextApplicator

Sets `Text.text` from a string variable. Requires `Text` on same GameObject.

- `target`: Text
- `source`: ViewVariableString

#### ToggleApplicator

Sets `Toggle.isOn` from a bool variable. Requires `Toggle` on same GameObject.

- `target`: Toggle
- `source`: ViewVariableBool

#### SliderValueApplicator

Sets `Slider.value` from a float variable. Requires `Slider` on same GameObject.

- `target`: Slider
- `source`: ViewVariableFloat

#### InputFieldApplicator

Sets `InputField.text` from a string variable. Requires `InputField` on same GameObject.

- `target`: InputField
- `source`: ViewVariableString

#### ImageFillAmountApplicator

Sets `Image.fillAmount` from a float variable.

- `target`: Image
- `source`: ViewVariableFloat

#### ImageEnabledApplicator

Sets `Image.enabled` from a bool variable.

- `target`: Image
- `source`: ViewVariableBool

#### ButtonInteractableApplicator

Sets `Button.interactable` from a bool variable.

- `target`: Button
- `source`: ViewVariableBool

#### CanvasGroupAlphaApplicator

Sets `CanvasGroup.alpha` from a float variable.

- `target`: CanvasGroup
- `source`: ViewVariableFloat

#### CanvasGroupInteractableApplicator

Sets `CanvasGroup.interactable` from a bool variable.

- `target`: CanvasGroup
- `source`: ViewVariableBool

#### CanvasGroupRaycastTargetApplicator

Sets `CanvasGroup.blocksRaycasts` from a bool variable.

- `target`: CanvasGroup
- `source`: ViewVariableBool

#### CanvasGroupVisibilityApplicator

Sets `CanvasGroup.alpha` to 0/1 and `blocksRaycasts` from a bool variable.

- `target`: CanvasGroup
- `source`: ViewVariableBool

#### GameObjectActivityApplicator

Sets `GameObject.SetActive` from a bool variable.

| Field | Type | Description |
|-------|------|-------------|
| `target` | Transform | Target transform (`#id` reference) |
| `source` | ViewVariableBool | `.context` = ViewContext ref, `.name` = variable name |
| `inverse` | bool | Invert the value |

---

### Formatted / Localized Text Applicators

These applicators do NOT use `source`. They format text from variables referenced in a format string.

#### FormattedTMPTextApplicator

Formats text using `<variable_name>` placeholders. Requires `TMP_Text` on same GameObject.

| Field | Type | Description |
|-------|------|-------------|
| `target` | TMP_Text | Target text component (`#id` reference) |
| `format` | string | Format string, e.g. `"Score: <score>"` |
| `extraContexts` | ViewContextBase[] | Additional contexts to resolve variables from |

#### LocalizedTMPTextApplicator

Same as FormattedTMPTextApplicator but localizes the format string first.

| Field | Type | Description |
|-------|------|-------------|
| `target` | TMP_Text | Target text component (`#id` reference) |
| `format` | string | Localization key / format string |
| `extraContexts` | ViewContextBase[] | Additional contexts to resolve variables from |

---

### UnityEvent Applicators

Invoke a `UnityEvent<T>` when a variable changes. Useful for custom reactions.

All share:

| Field | Type | Description |
|-------|------|-------------|
| `source` | TVariable | `.context` = ViewContext ref, `.name` = variable name |
| `callback` | UnityEvent\<T\> | Unity event to invoke |

- **UnityEventBoolApplicator** — source: ViewVariableBool
- **UnityEventIntApplicator** — source: ViewVariableInt
- **UnityEventFloatApplicator** — source: ViewVariableFloat
- **UnityEventStringApplicator** — source: ViewVariableString

---

### Adapters

Adapters transform variable values. They act as mini-contexts producing a result variable. All adapters based on `SingleResultAdapterBase` have a `result` field (the output variable with `.context` and `.name`).

#### InverseBoolAdapter

Inverts a bool variable.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableBool | Output: `.context` and `.name` |
| `source` | ViewVariableBool | Input: `.context` and `.name` |

#### BoolToStringAdapter

Converts bool to string.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableString | Output |
| `source` | ViewVariableBool | Input |
| `trueString` | string | String when true (default: `"TRUE"`) |
| `falseString` | string | String when false (default: `"FALSE"`) |

#### BoolToFormattedStringAdapter

Converts bool to formatted string with variable placeholders.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableString | Output |
| `source` | ViewVariableBool | Input |
| `trueFormat` | string | Format when true |
| `falseFormat` | string | Format when false |
| `extraContexts` | ViewContextBase[] | Additional contexts |

#### CompareStringAdapter

Compares a string variable to a constant, outputs bool.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableBool | Output |
| `source` | ViewVariableString | Input |
| `comparer` | enum | `Equals` or `NotEquals` |
| `other` | string | String to compare against |

#### FloatFormatAdapter

Passes float through with precision formatting.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableFloatFormatted | Output (has `precision` and `fixedPrecision`) |
| `source` | ViewVariableFloat | Input |

#### FloatRatioAdapter

Computes numerator / denominator.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableFloat | Output |
| `numerator` | ViewVariableFloat | Numerator: `.context` and `.name` |
| `denominator` | ViewVariableFloat | Denominator: `.context` and `.name` |

#### FormattedTextAdapter

Formats a string with `<variable>` placeholders.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableString | Output |
| `format` | string | Format string |

#### TextLocalizeAdapter

Localizes and formats a string.

| Field | Type | Description |
|-------|------|-------------|
| `result` | ViewVariableStringLocalized | Output |
| `format` | string | Localization key / format |

---

## Variable and Event Types

### Variables (for `source`, `result` fields)

All have sub-fields: `context` (ViewContextBase, `#id`) and `name` (string).

| Type | Value Type |
|------|-----------|
| `ViewVariableBool` | bool |
| `ViewVariableInt` | int |
| `ViewVariableFloat` | float |
| `ViewVariableString` | string |

### Events (for `onClick`, `onToggle`, etc.)

All have sub-fields: `context` (ViewContextBase, `#id`) and `name` (string).

| Type | Parameter |
|------|-----------|
| `ViewEventVoid` | none |
| `ViewEventBool` | bool |
| `ViewEventInt` | int |
| `ViewEventFloat` | float |
| `ViewEventString` | string |
