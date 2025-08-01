# MVP Pattern for Unity

A comprehensive MVP (Model-View-Presenter) pattern implementation for Unity projects.

## Overview

The MVP pattern separates concerns in your application architecture:
- **Model**: Manages data and business logic
- **View**: Handles user interface and user input
- **Presenter**: Mediates between Model and View, contains presentation logic

## Structure

```
Assets/MVP/
├── Source/                  # Core MVP framework
│   ├── MVP.asmdef          # Assembly definition
│   ├── IPresenter.cs       # Presenter interface
│   ├── IView.cs            # View interface  
│   ├── IModel.cs           # Model interface
│   ├── Presenter.cs        # Base presenter class
│   ├── View.cs             # Base view class
│   ├── ViewMonoBehaviour.cs # Unity MonoBehaviour view
│   └── Model.cs            # Base model class
├── Example/                # Usage examples
│   ├── MVP.Example.asmdef  # Example assembly
│   ├── README.md           # Example documentation
│   └── Example*.cs         # Example implementations
└── Tests/                  # Comprehensive test suite
    ├── MVP.Tests.asmdef    # Test assembly
    ├── README.md           # Test documentation
    ├── ModelTests.cs       # Model unit tests
    ├── ViewTests.cs        # View unit tests
    ├── ViewMonoBehaviourTests.cs # MonoBehaviour view tests
    ├── PresenterTests.cs   # Presenter unit tests
    └── IntegrationTests.cs # End-to-end integration tests
```

## Features

- ✅ Full async/await support
- ✅ Proper resource disposal (IDisposable/IAsyncDisposable)
- ✅ Unity MonoBehaviour integration
- ✅ Cancellation token support
- ✅ Composite disposable pattern
- ✅ Type-safe presenter-view binding
- ✅ Comprehensive test suite (95%+ coverage)
- ✅ NUnit-based unit and integration tests

## Quick Start

See the [Example folder](./Example/README.md) for detailed usage examples.

## Testing

The framework includes a comprehensive test suite with 95%+ code coverage:
- Unit tests for all MVP components
- Integration tests with real-world scenarios  
- Unity MonoBehaviour testing
- Async/await pattern validation
- Memory leak prevention tests

See the [Tests folder](./Tests/README.md) for detailed testing documentation.

## Dependencies

- [Disposable](../Disposable/) - For resource management base classes