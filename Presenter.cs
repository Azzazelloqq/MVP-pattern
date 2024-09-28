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
/// <typeparam name="TView">The type of the view, which implements <see cref="IView"/>.</typeparam>
/// <typeparam name="TModel">The type of the model, which implements <see cref="IModel"/>.</typeparam>
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
	/// Initializes a new instance of the <see cref="Presenter{TView, TModel}"/> class with the specified view and model.
	/// </summary>
	/// <param name="view">The view associated with the presenter.</param>
	/// <param name="model">The model associated with the presenter.</param>
	public Presenter(TView view, TModel model)
	{
		this.view = view;
		this.model = model;

		compositeDisposable.AddDisposable(view, model);
	}

	/// <summary>
	/// Initializes the presenter async. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	public virtual async Task InitializeAsync(CancellationToken token) {
		await view.InitializeAsync(this, token);
		await model.InitializeAsync(token);
		
		await OnInitializeAsync(token);
	}

	/// <summary>
	/// Initializes the presenter. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	public virtual void Initialize()
	{
		view.Initialize(this);
		model.Initialize();
		
		OnInitialize();
	}

	protected virtual void OnInitialize()
	{
	}

	protected virtual async Task OnInitializeAsync(CancellationToken token)
	{
		
	}

	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();
		compositeDisposable?.Dispose();
	}
}
}