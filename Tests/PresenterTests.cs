using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using MVP;

namespace MVP.Tests
{
    [TestFixture]
    public class PresenterTests
    {
        private TestCompletePresenter _presenter;
        private TestView _view;
        private TestModel _model;

        [SetUp]
        public void SetUp()
        {
            CallOrderCounter.Reset(); // Reset global call order for each test
            _view = new TestView();
            _model = new TestModel();
            _presenter = new TestCompletePresenter(_view, _model);
        }

        [TearDown]
        public void TearDown()
        {
            _presenter?.Dispose();
            // View and Model are disposed by Presenter's composite disposable
        }

        [Test]
        public void Constructor_AssignsViewAndModel()
        {
            // Assert
            Assert.AreEqual(_view, _presenter.AssignedView);
            Assert.AreEqual(_model, _presenter.AssignedModel);
        }

        [Test]
        public void Constructor_AddsViewAndModelToCompositeDisposable()
        {
            // This test verifies that view and model are properly managed
            // We can't directly test composite disposable contents, but we can verify
            // that they get disposed when presenter is disposed
            
            // Act
            _presenter.Dispose();

            // Assert - view and model should be disposed by composite disposable
            Assert.IsTrue(_view.IsDisposed);
            Assert.IsTrue(_model.IsDisposed);
        }

        [Test]
        public void Initialize_CallsViewAndModelInitialize()
        {
            // Act
            _presenter.Initialize();

            // Assert
            Assert.IsTrue(_view.IsInitializeCalled);
            Assert.IsTrue(_model.IsInitializeCalled);
            Assert.IsTrue(_presenter.IsInitializeCalled);
            
            // Verify the presenter was assigned to the view
            Assert.AreEqual(_presenter, _view.AssignedPresenter);
        }

        [Test]
        public async Task InitializeAsync_CallsViewAndModelInitializeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _presenter.InitializeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_view.IsInitializeAsyncCalled);
            Assert.IsTrue(_model.IsInitializeAsyncCalled);
            Assert.IsTrue(_presenter.IsInitializeAsyncCalled);
            
            // Verify the presenter was assigned to the view
            Assert.AreEqual(_presenter, _view.AssignedPresenter);
        }

        [Test]
        public void InitializeAsync_RespectsCancellationToken()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<TaskCanceledException>(
                async () => await _presenter.InitializeAsync(cts.Token));
        }

        [Test]
        public void Dispose_CallsOnDispose()
        {
            // Act
            _presenter.Dispose();

            // Assert
            Assert.IsTrue(_presenter.IsDisposeCalled);
            Assert.IsFalse(_presenter.IsDisposeAsyncCalled);
        }

        [Test]
        public async Task DisposeAsync_CallsOnDisposeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _presenter.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_presenter.IsDisposeAsyncCalled);
            Assert.IsFalse(_presenter.IsDisposeCalled);
        }

        [Test]
        public void DisposeToken_IsValidBeforeDispose()
        {
            // Act & Assert
            Assert.IsFalse(_presenter.ExposeDisposeToken.IsCancellationRequested);
        }

        [Test]
        public void DisposeToken_IsCancelledAfterDispose()
        {
            // Arrange - Get token before dispose
            var token = _presenter.ExposeDisposeToken;
            
            // Act
            _presenter.Dispose();

            // Assert - Check the token we captured before dispose
            Assert.IsTrue(token.IsCancellationRequested);
        }

        [Test]
        public void CompositeDisposable_IsNotNull()
        {
            // Assert
            Assert.IsNotNull(_presenter.ExposeCompositeDisposable);
        }

        [Test]
        public void BusinessLogic_CanAccessModelData()
        {
            // Arrange
            const string testData = "TestValue";

            // Act
            _presenter.UpdateModelData(testData);
            var result = _presenter.GetModelData();

            // Assert
            Assert.AreEqual(testData, result);
            Assert.AreEqual(testData, _model.TestData);
        }

        [Test]
        public void Dispose_DisposesViewAndModel()
        {
            // Act
            _presenter.Dispose();

            // Assert
            Assert.IsTrue(_view.IsDisposed);
            Assert.IsTrue(_model.IsDisposed);
        }

        [Test]
        public async Task DisposeAsync_DisposesViewAndModel()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _presenter.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_view.IsDisposed);
            Assert.IsTrue(_model.IsDisposed);
        }

        [Test]
        public void MultipleDispose_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _presenter.Dispose();
                _presenter.Dispose(); // Second call should not throw
            });
        }

        [Test]
        public async Task MultipleDisposeAsync_DoesNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act & Assert - First disposal
            await _presenter.DisposeAsync(cts.Token);
            
            // Act & Assert - Second disposal should not throw
            await _presenter.DisposeAsync(cts.Token);
        }

        [Test]
        public void Initialize_ExecutionOrder_IsCorrect()
        {
            // Act
            _presenter.Initialize();

            // Assert - View should be initialized first, then model, then presenter
            Assert.IsTrue(_view.CallOrder < _model.CallOrder);
            Assert.IsTrue(_model.CallOrder < _presenter.CallOrder);
        }

        [Test]
        public async Task InitializeAsync_ExecutionOrder_IsCorrect()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _presenter.InitializeAsync(cts.Token);

            // Assert - View should be initialized first, then model, then presenter
            Assert.IsTrue(_view.CallOrder < _model.CallOrder);
            Assert.IsTrue(_model.CallOrder < _presenter.CallOrder);
        }
    }
}