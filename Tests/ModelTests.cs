using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using MVP;

namespace MVP.Tests
{
    [TestFixture]
    public class ModelTests
    {
        private TestModel _model;

        [SetUp]
        public void SetUp()
        {
            _model = new TestModel();
        }

        [TearDown]
        public void TearDown()
        {
            _model?.Dispose();
        }

        [Test]
        public void Initialize_CallsOnInitialize()
        {
            // Act
            _model.Initialize();

            // Assert
            Assert.IsTrue(_model.IsInitializeCalled);
            Assert.IsFalse(_model.IsInitializeAsyncCalled);
        }

        [Test]
        public async Task InitializeAsync_CallsOnInitializeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _model.InitializeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_model.IsInitializeAsyncCalled);
            Assert.IsFalse(_model.IsInitializeCalled);
        }

        [Test]
        public void InitializeAsync_RespectsCancellationToken()
        {
            // Arrange
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            Assert.ThrowsAsync<TaskCanceledException>(
                async () => await _model.InitializeAsync(cts.Token));
        }

        [Test]
        public void Dispose_CallsOnDispose()
        {
            // Act
            _model.Dispose();

            // Assert
            Assert.IsTrue(_model.IsDisposeCalled);
            Assert.IsFalse(_model.IsDisposeAsyncCalled);
        }

        [Test]
        public async Task DisposeAsync_CallsOnDisposeAsync()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act
            await _model.DisposeAsync(cts.Token);

            // Assert
            Assert.IsTrue(_model.IsDisposeAsyncCalled);
            Assert.IsFalse(_model.IsDisposeCalled);
        }

        [Test]
        public void DisposeToken_IsValidBeforeDispose()
        {
            // Act & Assert
            Assert.IsFalse(_model.ExposeDisposeToken.IsCancellationRequested);
        }

        [Test]
        public void DisposeToken_IsCancelledAfterDispose()
        {
            // Arrange - Get token before dispose
            var token = _model.ExposeDisposeToken;
            
            // Act
            _model.Dispose();

            // Assert - Check the token we captured before dispose
            Assert.IsTrue(token.IsCancellationRequested);
        }

        [Test]
        public void CompositeDisposable_IsNotNull()
        {
            // Assert
            Assert.IsNotNull(_model.ExposeCompositeDisposable);
        }

        [Test]
        public void MultipleDispose_DoesNotThrow()
        {
            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                _model.Dispose();
                _model.Dispose(); // Second call should not throw
            });
        }

        [Test]
        public async Task MultipleDisposeAsync_DoesNotThrow()
        {
            // Arrange
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            // Act & Assert - First disposal
            await _model.DisposeAsync(cts.Token);
            
            // Act & Assert - Second disposal should not throw
            await _model.DisposeAsync(cts.Token);
        }
    }
}