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
	/// Initializes the view with the specified presenter.
	/// </summary>
	/// <param name="presenter">The presenter associated with the view.</param>
	public virtual void Initialize(TPresenter presenter)
	{
		this.presenter = presenter;
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();

		compositeDisposable?.Dispose();
	}
}
}