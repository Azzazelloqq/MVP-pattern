using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using MVP;

namespace MVP.Example
{
/// <summary>
/// Example implementation of a MonoBehaviour-based view for demonstration purposes.
/// Shows how to implement MVP pattern in Unity with MonoBehaviour lifecycle.
/// </summary>
public class ExampleViewMonoBehaviour : ViewMonoBehaviour<ExamplePresenter>
{
	/// <summary>
	/// Stores the current count value. Note: This should probably be a UI Text component instead.
	/// </summary>
	private int _exampleCountDisplay;

	/// <summary>
	/// Updates the count display value.
	/// </summary>
	/// <param name="count">The count value to display.</param>
	public void ExampleUpdateCount(int count)
	{
		_exampleCountDisplay = count;
	}

	protected override void OnInitialize()
	{
		// In a real Unity project, UI components would be initialized here
		// Example: find child UI elements, subscribe to button events, etc.
		// Example: GetComponent<Button>()?.onClick.AddListener(() => presenter.OnButtonClicked());
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