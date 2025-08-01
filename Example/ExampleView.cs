using System.Threading;
using System.Threading.Tasks;
using UnityEngine.UI;
using MVP;

namespace MVP.Example
{
/// <summary>
/// Example implementation of a view for demonstration purposes.
/// Shows how to implement MVP pattern with UI updates.
/// </summary>
public class ExampleView : View<ExamplePresenter>
{
	/// <summary>
	/// UI text component that displays the current count value.
	/// </summary>
	private Text _exampleCountDisplay;

	/// <summary>
	/// Updates the count display in the UI.
	/// </summary>
	/// <param name="count">The count value to display.</param>
	public void ExampleUpdateCount(int count)
	{
		if (_exampleCountDisplay != null)
			_exampleCountDisplay.text = count.ToString();
	}

	protected override void OnInitialize()
	{
		// In a real project, UI components would be initialized here
		// Example: _exampleCountDisplay = GetComponent<Text>();
		// Or: _exampleCountDisplay = transform.Find("CountText").GetComponent<Text>();
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