using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using MVP;
using Disposable;

namespace MVP.Tests
{
    /// <summary>
    /// Global call order counter for testing execution sequence
    /// </summary>
    public static class CallOrderCounter
    {
        private static int _globalCounter = 0;
        
        public static int GetNext() => ++_globalCounter;
        
        public static void Reset() => _globalCounter = 0;
    }

    /// <summary>
    /// Test implementation of IPresenter for testing purposes
    /// </summary>
    public class TestPresenter : IPresenter
    {
        public bool IsDisposed { get; private set; }
        public bool IsAsyncDisposed { get; private set; }

        public void Dispose()
        {
            IsDisposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            IsAsyncDisposed = true;
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Wrong presenter type for testing type safety
    /// </summary>
    public class WrongPresenter : IPresenter
    {
        public void Dispose() { }
        public ValueTask DisposeAsync() => default;
    }

    /// <summary>
    /// Test implementation of Model for testing purposes
    /// </summary>
    public class TestModel : Model
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public string TestData { get; set; } = "Initial";
        public int CallOrder { get; private set; }

        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;
        public ICompositeDisposable ExposeCompositeDisposable => compositeDisposable;

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            TestData = "Initial"; // Reset to initial state
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            TestData = "Initial"; // Reset to initial state
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }
    }

    /// <summary>
    /// Simple test implementation of View for unit testing purposes
    /// </summary>
    public class SimpleTestView : View<TestPresenter>
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public TestPresenter AssignedPresenter => presenter;
        public int CallOrder { get; private set; }


        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;
        public ICompositeDisposable ExposeCompositeDisposable => compositeDisposable;

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }
    }

    /// <summary>
    /// Test implementation of View for integration testing purposes
    /// </summary>
    public class TestView : View<TestCompletePresenter>
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public TestCompletePresenter AssignedPresenter => presenter;
        public int CallOrder { get; private set; }


        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;
        public ICompositeDisposable ExposeCompositeDisposable => compositeDisposable;

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }
    }

    /// <summary>
    /// Simple test implementation of ViewMonoBehaviour for unit testing purposes
    /// </summary>
    public class SimpleTestViewMonoBehaviour : ViewMonoBehaviour<TestPresenter>
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public TestPresenter AssignedPresenter => presenter;
        public int CallOrder { get; private set; }


        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }
    }

    /// <summary>
    /// Test implementation of ViewMonoBehaviour for integration testing purposes
    /// </summary>
    public class TestViewMonoBehaviour : ViewMonoBehaviour<TestMonoBehaviourPresenter>
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public TestMonoBehaviourPresenter AssignedPresenter => presenter;
        public int CallOrder { get; private set; }


        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }
    }

    /// <summary>
    /// Test implementation of Presenter for MonoBehaviour testing purposes
    /// </summary>
    public class TestMonoBehaviourPresenter : Presenter<TestViewMonoBehaviour, TestModel>
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public TestViewMonoBehaviour AssignedView => view;
        public TestModel AssignedModel => model;
        public int CallOrder { get; private set; }


        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;
        public ICompositeDisposable ExposeCompositeDisposable => compositeDisposable;

        public TestMonoBehaviourPresenter(TestViewMonoBehaviour view, TestModel model) : base(view, model)
        {
        }

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }

        // Public method to test business logic
        public void UpdateModelData(string newData)
        {
            model.TestData = newData;
        }

        public string GetModelData()
        {
            return model.TestData;
        }
    }

    /// <summary>
    /// Test implementation of Presenter for testing purposes
    /// </summary>
    public class TestCompletePresenter : Presenter<TestView, TestModel>
    {
        public bool IsInitializeCalled { get; private set; }
        public bool IsInitializeAsyncCalled { get; private set; }
        public bool IsDisposeCalled { get; private set; }
        public bool IsDisposeAsyncCalled { get; private set; }
        
        public TestView AssignedView => view;
        public TestModel AssignedModel => model;
        public int CallOrder { get; private set; }


        // Expose protected members for testing
        public CancellationToken ExposeDisposeToken => disposeToken;
        public ICompositeDisposable ExposeCompositeDisposable => compositeDisposable;

        public TestCompletePresenter(TestView view, TestModel model) : base(view, model)
        {
        }

        protected override void OnInitialize()
        {
            // Reset state flags
            IsInitializeCalled = true;
            IsInitializeAsyncCalled = false;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnInitializeAsync(CancellationToken token)
        {
            // Reset state flags
            IsInitializeCalled = false;
            IsInitializeAsyncCalled = true;
            IsDisposeCalled = false;
            IsDisposeAsyncCalled = false;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async work
        }

        protected override void OnDispose()
        {
            IsDisposeCalled = true;
            CallOrder = CallOrderCounter.GetNext();
        }

        protected override async ValueTask OnDisposeAsync(CancellationToken token)
        {
            IsDisposeAsyncCalled = true;
            CallOrder = CallOrderCounter.GetNext();
            await Task.Delay(10, token); // Simulate async cleanup
        }

        // Public method to test business logic
        public void UpdateModelData(string newData)
        {
            model.TestData = newData;
        }

        public string GetModelData()
        {
            return model.TestData;
        }
    }
}