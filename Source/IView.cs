using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVP
{
/// <summary>
/// Represents the view in the MVP (Model-View-Presenter) pattern.
/// The view is responsible for rendering the user interface and receiving user input, while being updated by the presenter.
/// Implements <see cref="IDisposable"/> to allow proper resource management and disposal.
/// </summary>
public interface IView : IDisposable
{
	/// <summary>
	/// Initializes the view with the specified presenter.
	/// </summary>
	/// <param name="presenter">The presenter associated with the view.</param>
	public void Initialize(IPresenter presenter);

	/// <summary>
	/// Initializes the view async. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	/// <param name="presenter">The presenter associated with the view.</param>
	/// <param name="token">Cancellation token to observe during the initialization process.</param>
	/// <returns>A task that represents the asynchronous initialization operation.</returns>
	public Task InitializeAsync(IPresenter presenter, CancellationToken token);
}
}