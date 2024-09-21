using UnityEngine.UI;

namespace MVP.Example
{
public class ExampleView : View<ExamplePresenter>
{
	private Text _exampleCountDisplay;

	public void ExampleUpdateCount(int count)
	{
		_exampleCountDisplay.text = count.ToString();
	}
}
}