# MVP Example

This example demonstrates basic usage of the MVP (Model-View-Presenter) pattern.

## Files

- **ExampleModel.cs** - Model managing data (counter)
- **ExampleView.cs** - Regular View for non-MonoBehaviour scenarios
- **ExampleViewMonoBehaviour.cs** - MonoBehaviour-based View for Unity
- **ExamplePresenter.cs** - Presenter coordinating interaction between Model and View

## Architecture

```
ExamplePresenter
├── ExampleModel (manages data)
└── ExampleView (displays UI)
```

## Usage

1. Create instances of Model and View
2. Pass them to the Presenter constructor
3. Call `Initialize()` or `InitializeAsync()` on the presenter
4. Use public presenter methods for interaction

```csharp
var model = new ExampleModel();
var view = new ExampleView();
var presenter = new ExamplePresenter(view, model);

presenter.Initialize();
presenter.ExampleIncreaseCount(5);
```

## Notes

- View is responsible only for display
- Model contains business logic and data
- Presenter coordinates interaction and updates
- All components support asynchronous initialization and proper resource disposal