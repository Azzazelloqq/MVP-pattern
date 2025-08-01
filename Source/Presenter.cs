using System;
using System.Threading;
using System.Threading.Tasks;
using Disposable;

namespace MVP
{
/// <summary>
/// Represents the base presenter class in the MVP (Model-View-Presenter) pattern.
/// The presenter is responsible for mediating between the view and the model, handling business logic and updating the view.
/// Inherits from <see cref="DisposableBase"/> to provide built-in resource management and implements <see cref="IPresenter"/>.
/// </summary>
/// <typeparam name="TView">The type of the view, which implements <see cref="IView"/> and inherits from <see cref="DisposableBase"/>.</typeparam>
/// <typeparam name="TModel">The type of the model, which implements <see cref="IModel"/> and inherits from <see cref="DisposableBase"/>.</typeparam>
public abstract class Presenter<TView, TModel> : DisposableBase, IPresenter
	where TModel : IModel
	where TView : IView
{
	/// <summary>
	/// The view associated with this presenter.
	/// </summary>
	protected readonly TView view;

	/// <summary>
	/// The model associated with this presenter.
	/// </summary>
	protected readonly TModel model;

	/// <summary>
	/// A composite disposable that manages the disposal of both the view and the model, along with other disposable resources.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
		
	/// <summary>
	/// Gets the cancellation token that is triggered when the presenter is disposed.
	/// </summary>
	protected CancellationToken disposeToken => _disposeCancellationSource.Token;
	
	/// <summary>
	/// The cancellation token source that is used to signal disposal of the presenter.
	/// </summary>
	private readonly CancellationTokenSource _disposeCancellationSource = new();
	
	/// <summary>
	/// Initializes a new instance of the <see cref="Presenter{TView, TModel}"/> class with the specified view and model.
	/// </summary>
	/// <param name="view">The view associated with the presenter.</param>
	/// <param name="model">The model associated with the presenter.</param>
	public Presenter(TView view, TModel model)
	{
		this.view = view;
		this.model = model;

		// Note: View and Model are NOT added to CompositeDisposable to avoid 
		// "Have async disposables. Can't dispose synchronously." exception.
		// They are disposed manually in Dispose/DisposeAsyncCore methods.
		// CompositeDisposable is reserved for additional resources only.
	}

	/// <summary>
	/// Initializes the presenter async. This method can be overridden by derived classes to provide custom initialization logic.
	/// Initializes the view, model, and calls custom initialization logic.
	/// </summary>
	/// <param name="token">Cancellation token to observe during the initialization process.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	public async Task InitializeAsync(CancellationToken token) {
		await view.InitializeAsync(this, token);
		await model.InitializeAsync(token);
		
		await OnInitializeAsync(token);
	}

	/// <summary>
	/// Initializes the presenter. This method can be overridden by derived classes to provide custom initialization logic.
	/// Initializes the view, model, and calls custom initialization logic.
	/// </summary>
	public void Initialize()
	{
		view.Initialize(this);
		model.Initialize();
		
		OnInitialize();
	}
	
	protected sealed override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
		
		OnDispose();
		
		// Dispose view and model synchronously
		view?.Dispose();
		model?.Dispose();
		
		compositeDisposable?.Dispose();
		
		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}
		
		_disposeCancellationSource.Dispose();
	}
	
	protected sealed override async ValueTask DisposeAsyncCore(CancellationToken token, bool continueOnCapturedContext)
	{
		await OnDisposeAsync(token);
		
		// Dispose view and model asynchronously based on their actual types
		if (view is DisposableBase disposableView)
		{
			await disposableView.DisposeAsync(token, continueOnCapturedContext);
		}
		else if (view is IAsyncDisposable asyncView)
		{
			await asyncView.DisposeAsync();
		}
		else
		{
			view?.Dispose();
		}
		
		if (model is DisposableBase disposableModel)
		{
			await disposableModel.DisposeAsync(token, continueOnCapturedContext);
		}
		else if (model is IAsyncDisposable asyncModel)
		{
			await asyncModel.DisposeAsync();
		}
		else
		{
			model?.Dispose();
		}
		
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