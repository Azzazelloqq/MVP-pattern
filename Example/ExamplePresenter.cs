using System.Threading;
using System.Threading.Tasks;
using MVP;

namespace MVP.Example
{
/// <summary>
/// Example implementation of a presenter for demonstration purposes.
/// Shows how to implement MVP pattern with ExampleView and ExampleModel.
/// </summary>
public class ExamplePresenter : Presenter<ExampleView, ExampleModel>
{
	/// <summary>
	/// Initializes a new instance of the ExamplePresenter class.
	/// </summary>
	/// <param name="view">The example view instance.</param>
	/// <param name="model">The example model instance.</param>
	public ExamplePresenter(ExampleView view, ExampleModel model) : base(view, model)
	{
	}

	/// <summary>
	/// Increases the count in the model and updates the view with the new value.
	/// </summary>
	/// <param name="count">The amount to increase the count by.</param>
	public void ExampleIncreaseCount(int count)
	{
		model.ExampleIncreaseCount(count);
		var modelExampleCount = model.ExampleCount;
		view.ExampleUpdateCount(modelExampleCount);
	}

	protected override void OnInitialize()
	{
		// Initialize presenter logic
		// Update view with initial data from model
		view.ExampleUpdateCount(model.ExampleCount);
	}

	protected override ValueTask OnInitializeAsync(CancellationToken token)
	{
		return default;
	}

	protected override void OnDispose()
	{
	}

	protected override ValueTask OnDisposeAsync(CancellationToken token)
	{
		return default;
	}
}
}