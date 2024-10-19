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

	/// <inheritdoc cref="IModel.InitializeAsync(CancellationToken)" />
	public virtual async Task InitializeAsync(CancellationToken token)
	{
		await OnInitializeAsync(token);
	}
	
	/// <inheritdoc/>
	public virtual void Initialize()
	{
		OnInitialize();
	}

	protected virtual void OnInitialize()
	{
		
	}
	
	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();
		compositeDisposable?.Dispose();
		
		OnDispose();
	}
	
	protected virtual async Task OnInitializeAsync(CancellationToken token)
	{
		
	}

	protected virtual void OnDispose()
	{
		
	}
}
}