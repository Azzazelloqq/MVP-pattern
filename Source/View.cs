using System;
using System.Threading;
using System.Threading.Tasks;
using Disposable;

namespace MVP
{
/// <summary>
/// Represents the base view class in the MVP (Model-View-Presenter) pattern.
/// The view is responsible for rendering the user interface and interacting with the presenter.
/// Inherits from <see cref="DisposableBase"/> to provide built-in resource management and implements <see cref="IView"/>.
/// </summary>
/// <typeparam name="TPresenter">The type of the presenter, which implements <see cref="IPresenter"/>.</typeparam>
public abstract class View<TPresenter> : DisposableBase, IView where TPresenter : IPresenter
{
	/// <summary>
	/// The presenter associated with this view.
	/// </summary>
	protected TPresenter presenter;

	/// <summary>
	/// A composite disposable that manages the disposal of any additional resources associated with the view.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();

	/// <summary>
	/// Gets the cancellation token that is triggered when the view is disposed.
	/// </summary>
	protected CancellationToken disposeToken => _disposeCancellationSource.Token;
	
	/// <summary>
	/// The cancellation token source that is used to signal disposal of the view.
	/// </summary>
	private readonly CancellationTokenSource _disposeCancellationSource = new();
	
	/// <inheritdoc/>
	public void Initialize(IPresenter presenter)
	{
		if (presenter is not TPresenter correctPresenter) {
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}
		
		this.presenter = correctPresenter;
		
		OnInitialize();
	}
	
	/// <inheritdoc/>
	public async Task InitializeAsync(IPresenter presenter, CancellationToken token)
	{
		if (presenter is not TPresenter correctPresenter) {
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}

		this.presenter = correctPresenter;
		
		await OnInitializeAsync(token);
	}
	
	/// <summary>
	/// Disposes the view and releases associated resources.
	/// </summary>
	/// <param name="disposing">True if called from Dispose method, false if called from finalizer.</param>
	protected sealed override void Dispose(bool disposing)
	{
		base.Dispose(disposing);

		OnDispose();
		
		compositeDisposable?.Dispose();
		
		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}
		
		_disposeCancellationSource.Dispose();
	}

	protected override async ValueTask DisposeAsyncCore(CancellationToken token, bool continueOnCapturedContext)
	{
		await OnDisposeAsync(token);
		
		if (compositeDisposable != null)
		{
			await compositeDisposable.DisposeAsync(token, continueOnCapturedContext);
		}
		
		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}
		
		_disposeCancellationSource.Dispose();
	}
	

	protected abstract void OnInitialize();
	protected abstract ValueTask OnInitializeAsync(CancellationToken token);
	
	protected abstract void OnDispose();
	protected abstract ValueTask OnDisposeAsync(CancellationToken token);
}
}