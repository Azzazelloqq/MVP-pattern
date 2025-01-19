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
public class Model : DisposableBase, IModel
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
	
	/// <inheritdoc cref="IModel.InitializeAsync(CancellationToken)" />
	public async Task InitializeAsync(CancellationToken token)
	{
		await OnInitializeAsync(token);
	}
	
	/// <inheritdoc/>
	public void Initialize()
	{
		OnInitialize();
	}

	protected virtual void OnInitialize()
	{
		
	}
	
	/// <inheritdoc/>
	public sealed override void Dispose()
	{
		base.Dispose();
		
		OnDispose();
		
		if (!_disposeCancellationSource.IsCancellationRequested)
		{
			_disposeCancellationSource.Cancel();
		}
		
		_disposeCancellationSource.Dispose();
		
		compositeDisposable?.Dispose();
	}
	
	protected virtual async Task OnInitializeAsync(CancellationToken token)
	{
		
	}

	protected virtual void OnDispose()
	{
		
	}
}
}