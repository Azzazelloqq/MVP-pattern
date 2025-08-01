using System.Threading;
using System.Threading.Tasks;
using MVP;

namespace MVP.Example
{
/// <summary>
/// Example implementation of a model for demonstration purposes.
/// Manages a simple counter value to show basic model functionality.
/// </summary>
public class ExampleModel : Model
{
	/// <summary>
	/// Gets the current count value.
	/// </summary>
	public int ExampleCount { get; private set; }

	/// <summary>
	/// Increases the count by the specified amount. Ignores negative values.
	/// </summary>
	/// <param name="count">The amount to increase the count by. Must be non-negative.</param>
	public void ExampleIncreaseCount(int count)
	{
		if (count < 0)
		{
			return;
		}

		ExampleCount += count;
	}

	protected override void OnInitialize()
	{
		// Initialize model data
		ExampleCount = 0;
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