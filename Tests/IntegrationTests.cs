using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using MVP;

namespace MVP.Tests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void TestMVP_BasicWorkflow_WorksCorrectly()
        {
            // Arrange
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);

            try
            {
                // Act - Initialize
                presenter.Initialize();

                // Assert - Initial state
                Assert.AreEqual("Initial", model.TestData);

                // Act - Update data
                presenter.UpdateModelData("Updated");

                // Assert - Data updated
                Assert.AreEqual("Updated", model.TestData);

                // Act - Get data through presenter
                var result = presenter.GetModelData();

                // Assert - Data retrieved correctly
                Assert.AreEqual("Updated", result);
            }
            finally
            {
                presenter?.Dispose();
            }
        }

        [Test]
        public async Task TestMVP_AsyncWorkflow_WorksCorrectly()
        {
            // Arrange
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            try
            {
                // Act - Initialize async
                await presenter.InitializeAsync(cts.Token);

                // Assert - Initial state
                Assert.AreEqual("Initial", model.TestData);

                // Act - Business logic
                presenter.UpdateModelData("AsyncUpdated");

                // Assert
                Assert.AreEqual("AsyncUpdated", model.TestData);
            }
            finally
            {
                if (presenter != null)
                {
                    await presenter.DisposeAsync(cts.Token);
                }
            }
        }

        [Test]
        public void TestMVP_MonoBehaviourWorkflow_WorksCorrectly()
        {
            // Arrange
            var gameObject = new GameObject("TestView");
            var model = new TestModel();
            var viewMono = gameObject.AddComponent<TestViewMonoBehaviour>();
            var presenter = new TestMonoBehaviourPresenter(viewMono, model);

            try
            {
                // Act - Initialize
                presenter.Initialize();

                // Assert - Initial state
                Assert.AreEqual("Initial", model.TestData);

                // Act - Business logic
                presenter.UpdateModelData("MonoBehaviourUpdated");

                // Assert
                Assert.AreEqual("MonoBehaviourUpdated", model.TestData);
            }
            finally
            {
                presenter?.Dispose();
                if (gameObject != null)
                {
                    UnityEngine.Object.DestroyImmediate(gameObject);
                }
            }
        }

        [Test]
        public void MVP_ComponentLifecycle_WorksCorrectly()
        {
            // Arrange
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);

            // Act & Assert - Before initialization
            Assert.IsFalse(model.IsInitializeCalled);
            Assert.IsFalse(view.IsInitializeCalled);
            Assert.IsFalse(presenter.IsInitializeCalled);

            // Act - Initialize
            presenter.Initialize();

            // Assert - After initialization
            Assert.IsTrue(model.IsInitializeCalled);
            Assert.IsTrue(view.IsInitializeCalled);
            Assert.IsTrue(presenter.IsInitializeCalled);
            Assert.AreEqual(presenter, view.AssignedPresenter);

            // Act - Dispose
            presenter.Dispose();

            // Assert - After disposal
            Assert.IsTrue(model.IsDisposeCalled);
            Assert.IsTrue(view.IsDisposeCalled);
            Assert.IsTrue(presenter.IsDisposeCalled);
        }

        [Test]
        public async Task MVP_AsyncComponentLifecycle_WorksCorrectly()
        {
            // Arrange
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // Act & Assert - Before initialization
            Assert.IsFalse(model.IsInitializeAsyncCalled);
            Assert.IsFalse(view.IsInitializeAsyncCalled);
            Assert.IsFalse(presenter.IsInitializeAsyncCalled);

            // Act - Initialize async
            await presenter.InitializeAsync(cts.Token);

            // Assert - After initialization
            Assert.IsTrue(model.IsInitializeAsyncCalled);
            Assert.IsTrue(view.IsInitializeAsyncCalled);
            Assert.IsTrue(presenter.IsInitializeAsyncCalled);
            Assert.AreEqual(presenter, view.AssignedPresenter);

            // Act - Dispose async
            await presenter.DisposeAsync(cts.Token);

            // Assert - After disposal
            Assert.IsTrue(model.IsDisposeAsyncCalled);
            Assert.IsTrue(view.IsDisposeAsyncCalled);
            Assert.IsTrue(presenter.IsDisposeAsyncCalled);
        }

        [Test]
        public void MVP_CancellationToken_PropagatesCorrectly()
        {
            // Arrange
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);

            // Act
            presenter.Initialize();

            // Assert - Tokens should be valid initially
            Assert.IsFalse(model.ExposeDisposeToken.IsCancellationRequested);
            Assert.IsFalse(view.ExposeDisposeToken.IsCancellationRequested);
            Assert.IsFalse(presenter.ExposeDisposeToken.IsCancellationRequested);

            // Arrange - Capture tokens before dispose
            var modelToken = model.ExposeDisposeToken;
            var viewToken = view.ExposeDisposeToken;
            var presenterToken = presenter.ExposeDisposeToken;

            // Act - Dispose presenter
            presenter.Dispose();

            // Assert - All captured tokens should be cancelled
            Assert.IsTrue(modelToken.IsCancellationRequested);
            Assert.IsTrue(viewToken.IsCancellationRequested);
            Assert.IsTrue(presenterToken.IsCancellationRequested);
        }

        [Test]
        public void MVP_CompositeDisposable_DisposesAllComponents()
        {
            // Arrange
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);

            // Act
            presenter.Initialize();
            presenter.Dispose();

            // Assert - All components should be disposed
            Assert.IsTrue(presenter.IsDisposed);
            Assert.IsTrue(view.IsDisposed);
            Assert.IsTrue(model.IsDisposed);
        }

        [Test]
        public async Task MVP_StressTest_MultipleInitializeDisposeCycles()
        {
            // This test verifies that multiple cycles work correctly
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            for (int i = 0; i < 10; i++)
            {
                var model = new TestModel();
                var view = new TestView();
                var presenter = new TestCompletePresenter(view, model);

                try
                {
                    // Alternate between sync and async initialization
                    if (i % 2 == 0)
                    {
                        presenter.Initialize();
                        Assert.IsTrue(presenter.IsInitializeCalled);
                    }
                    else
                    {
                        await presenter.InitializeAsync(cts.Token);
                        Assert.IsTrue(presenter.IsInitializeAsyncCalled);
                    }

                    // Verify components are working
                    presenter.UpdateModelData($"TestData_{i}");
                    Assert.AreEqual($"TestData_{i}", presenter.GetModelData());
                }
                finally
                {
                    // Alternate between sync and async disposal
                    if (i % 2 == 0)
                    {
                        presenter.Dispose();
                    }
                    else
                    {
                        await presenter.DisposeAsync(cts.Token);
                    }
                }
            }
        }

        [Test]
        public void MVP_TypeSafety_EnforcesCorrectTypes()
        {
            // This test verifies that the generic constraints work correctly
            // and prevent incorrect type assignments at compile time
            
            var model = new TestModel();
            var view = new TestView();
            
            // This should compile fine
            var presenter = new TestCompletePresenter(view, model);
            
            // This test primarily serves as a compile-time check
            Assert.IsNotNull(presenter);
            
            presenter.Dispose();
        }

        [Test]
        public async Task MVP_ResourceManagement_NoMemoryLeaks()
        {
            // This test verifies proper resource cleanup
            var (presenterRef, viewRef, modelRef) = await CreateAndDisposePresenterAsync();
            
            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            // Assert - Objects should be garbage collected
            Assert.IsFalse(presenterRef.IsAlive, "Presenter should be garbage collected");
            Assert.IsFalse(viewRef.IsAlive, "View should be garbage collected");
            Assert.IsFalse(modelRef.IsAlive, "Model should be garbage collected");
        }

        private async Task<(WeakReference presenterRef, WeakReference viewRef, WeakReference modelRef)> CreateAndDisposePresenterAsync()
        {
            var model = new TestModel();
            var view = new TestView();
            var presenter = new TestCompletePresenter(view, model);
            
            var presenterRef = new WeakReference(presenter);
            var viewRef = new WeakReference(view);
            var modelRef = new WeakReference(model);
            
            presenter.Initialize();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            await presenter.DisposeAsync(cts.Token);
            
            // Clear local references
            presenter = null;
            view = null;
            model = null;
            
            return (presenterRef, viewRef, modelRef);
        }
    }
}