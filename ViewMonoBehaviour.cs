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
	/// Initializes the view with the specified presenter.
	/// </summary>
	/// <param name="presenter">The presenter associated with the view.</param>
	public void Initialize(TPresenter presenter)
	{
		this.presenter = presenter;
	}
}
}