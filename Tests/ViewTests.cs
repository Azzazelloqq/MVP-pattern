using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using MVP;

namespace MVP.Tests
{
    [TestFixture]
    public class ViewTests
    {
        private SimpleTestView _view;
        private TestPresenter _presenter;

        [SetUp]
        public void SetUp()
        {
            _view = new SimpleTestView();
            _presenter = new TestPresenter();
        }

        [TearDown]
        public void TearDown()
        {
            _view?.Dispose();
            _presenter?.Dispose();
        }

        [Test]
        public void Initialize_WithCorrectPresenterType_CallsOnInitialize()
        {
            // Act
            _view.Initialize(_presenter);

            // Assert
            Assert.IsTrue(_view.IsInitializeCalled);
            Assert.IsFalse(_view.IsInitializeAsyncCalled);
            Assert.AreEqual(_presenter, _view.AssignedPresenter);
        }

        [Test]
        public void Initialize_WithWrongPresenterType_ThrowsArgumentException()
        {
            // Arrange
            var wrongPresenter = new WrongPresenter();

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _view.Initialize(wrongPresenter));
            Assert.That(ex.Message, Contains.Substring("Presenter must be of type TestPresenter"));
            Assert.That(ex.ParamName, Is.EqualTo("presenter"));
            
            wrongPresenter.Dispose();
        }

        [Test]
        public async Task InitializeAsync_WithCorrectPresenterType_CallsOnInitializeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _view.InitializeAsync(_presenter, cts.Token);

            // Assert
            Assert.IsTrue(_view.IsInitializeAsyncCalled);
            Assert.IsFalse(_view.IsInitializeCalled);
            Assert.AreEqual(_presenter, _view.AssignedPresenter);
        }

        [Test]
        public void InitializeAsync_WithWrongPresenterType_ThrowsArgumentException()
        {
            // Arrange
            var wrongPresenter = new WrongPresenter();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(
                async () => await _view.InitializeAsync(wrongPresenter, cts.Token));
            Assert.That(ex.Message, Contains.Substring("Presenter must be of type TestPresenter"));
            Assert.That(ex.ParamName, Is.EqualTo("presenter"));
            
            wrongPresenter.Dispose();
        }

        [Test]
        public void InitializeAsync_RespectsCancellationToken()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<TaskCanceledException>(
                async () => await _view.InitializeAsync(_presenter, cts.Token));
        }

        [Test]
        public void Dispose_CallsOnDispose()
        {
            // Act
            _view.Dispose();

            // Assert
            Assert.IsTrue(_view.IsDisposeCalled);
            Assert.IsFalse(_view.IsDisposeAsyncCalled);
        }

        [Test]
        public async Task DisposeAsync_CallsOnDisposeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _view.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_view.IsDisposeAsyncCalled);
            Assert.IsFalse(_view.IsDisposeCalled);
        }

        [Test]
        public void DisposeToken_IsValidBeforeDispose()
        {
            // Act & Assert
            Assert.IsFalse(_view.ExposeDisposeToken.IsCancellationRequested);
        }

        [Test]
        public void DisposeToken_IsCancelledAfterDispose()
        {
            // Arrange - Get token before dispose
            var token = _view.ExposeDisposeToken;
            
            // Act
            _view.Dispose();

            // Assert - Check the token we captured before dispose
            Assert.IsTrue(token.IsCancellationRequested);
        }

        [Test]
        public void CompositeDisposable_IsNotNull()
        {
            // Assert
            Assert.IsNotNull(_view.ExposeCompositeDisposable);
        }

        [Test]
        public void MultipleDispose_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _view.Dispose();
                _view.Dispose(); // Second call should not throw
            });
        }

        [Test]
        public async Task MultipleDisposeAsync_DoesNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act & Assert - First disposal
            await _view.DisposeAsync(cts.Token);
            
            // Act & Assert - Second disposal should not throw
            await _view.DisposeAsync(cts.Token);
        }
    }
}