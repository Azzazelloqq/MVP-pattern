using System.Threading;
using System.Threading.Tasks;
using Disposable;

namespace MVP
{
/// <summary>
/// Represents the base model class in the MVP (Model-View-Presenter) pattern.
/// Inherits from <see cref="DisposableBase"/> to provide built-in resource management and implements <see cref="IModel"/>.
/// This class manages the disposal of resources through the use of a composite disposable.
/// </summary>
public abstract class Model : DisposableBase, IModel
{
	/// <summary>
	/// A composite disposable that manages the disposal of multiple resources.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();

	/// <summary>
	/// Gets the cancellation token that is triggered when the model is disposed.
	/// </summary>
	protected CancellationToken disposeToken => _disposeCancellationSource.Token;

	/// <summary>
	/// The cancellation token source that is used to signal disposal of the model.
	/// </summary>
	private readonly CancellationTokenSource _disposeCancellationSource = new();

	/// <inheritdoc/>
	public async Task InitializeAsync(CancellationToken token)
	{
		await OnInitializeAsync(token);
	}

	/// <inheritdoc/>
	public void Initialize()
	{
		OnInitialize();
	}

	/// <summary>
	/// Disposes the model and releases associated resources.
	/// </summary>
	/// <param name="disposing">True if called from Dispose method, false if called from finalizer.</param>
	protected sealed override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		OnDispose();

		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}

		_disposeCancellationSource.Dispose();

		compositeDisposable?.Dispose();
	}

	protected sealed override async ValueTask DisposeAsyncCore(CancellationToken token, bool continueOnCapturedContext)
	{
		await OnDisposeAsync(token);
		
		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}

		_disposeCancellationSource.Dispose();

		if (compositeDisposable != null)
		{
			await compositeDisposable.DisposeAsync(token, continueOnCapturedContext);
		}
	}

	protected abstract void OnInitialize();
	protected abstract ValueTask OnInitializeAsync(CancellationToken token);
	
	protected abstract void OnDispose();
	protected abstract ValueTask OnDisposeAsync(CancellationToken token);
}
}