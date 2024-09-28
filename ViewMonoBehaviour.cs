using System;
using System.Threading;
using System.Threading.Tasks;
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

	/// <inheritdoc/>
	public virtual void Initialize(IPresenter presenter)
	{
		if (!(presenter is TPresenter correctPresenter)) {
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}
		
		this.presenter = correctPresenter;
		
		OnInitialize();
	}
	
	/// <inheritdoc/>
	public virtual async Task InitializeAsync(IPresenter presenter, CancellationToken token)
	{
		if (!(presenter is TPresenter correctPresenter)) {
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}

		presenter = correctPresenter;
		
		await OnInitializeAsync(token);
	}
	protected virtual async Task OnInitializeAsync(CancellationToken token)
	{
		
	}

	protected virtual void OnInitialize()
	{
		
	}
}
}