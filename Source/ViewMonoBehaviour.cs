using System;
using System.Threading;
using System.Threading.Tasks;
#if PROJECT_SUPPORT_UNITASK
using MVPTask = Cysharp.Threading.Tasks.UniTask;
#else
using MVPTask = System.Threading.Tasks.Task;
#endif
using Disposable;

namespace MVP
{
/// <summary>
/// Represents the base MonoBehaviour-based view class in the MVP (Model-View-Presenter) pattern.
/// This view is responsible for rendering the user interface in Unity and interacting with the presenter.
/// Inherits from <see cref="MonoBehaviourDisposable"/> to provide Unity-specific lifecycle management and implements <see cref="IView"/>.
/// </summary>
/// <typeparam name="TPresenter">The type of the presenter, which implements <see cref="IPresenter"/>.</typeparam>
public abstract class ViewMonoBehaviour<TPresenter> : MonoBehaviourDisposable, IView where TPresenter : IPresenter
{
	/// <summary>
	/// The presenter associated with this view.
	/// </summary>
	protected TPresenter presenter;

	/// <summary>
	/// Gets the cancellation token that is triggered when the view is disposed.
	/// </summary>
	protected CancellationToken disposeToken => disposeCancellationToken;

	/// <summary>
	/// A composite disposable that manages the disposal of any additional resources associated with the view.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();

	/// <inheritdoc/>
	public virtual void Initialize(IPresenter presenter)
	{
		if (presenter is not TPresenter correctPresenter)
		{
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}

		this.presenter = correctPresenter;

		OnInitialize();
	}

	/// <inheritdoc/>
	public virtual async MVPTask InitializeAsync(IPresenter presenter, CancellationToken token)
	{
		if (presenter is not TPresenter correctPresenter)
		{
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

		compositeDisposable.Dispose();
	}

	protected sealed override async ValueTask DisposeAsyncCore(CancellationToken token, bool continueOnCapturedContext = false)
	{
		await OnDisposeAsync(token);

		await compositeDisposable.DisposeAsync(token, continueOnCapturedContext);
	}

	protected abstract void OnInitialize();
	protected abstract ValueTask OnInitializeAsync(CancellationToken token);

	protected abstract void OnDispose();
	protected abstract ValueTask OnDisposeAsync(CancellationToken token);
}
}