using System;
using System.Threading;
#if PROJECT_SUPPORT_UNITASK
using Cysharp.Threading.Tasks;
using MVPTask = Cysharp.Threading.Tasks.UniTask;
#else
using System.Threading.Tasks;
using MVPTask = System.Threading.Tasks.Task;
#endif

namespace MVP
{
/// <summary>
/// Represents the model in the MVP (Model-View-Presenter) pattern.
/// The model is responsible for handling the data and business logic of the application.
/// Implements <see cref="IDisposable"/> to allow proper resource management and disposal.
/// </summary>
public interface IModel : IDisposable
{
	/// <summary>
	/// Initializes the model async. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	/// <param name="token">Cancellation token to observe during the initialization process.</param>
	/// <returns>An awaitable that represents the asynchronous initialization operation.</returns>
	public MVPTask InitializeAsync(CancellationToken token);

	/// <summary>
	/// Initializes the model. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	public void Initialize();

}
}