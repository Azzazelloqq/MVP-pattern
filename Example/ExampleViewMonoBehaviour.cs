namespace MVP.Example
{
public class ExampleViewMonoBehaviour : ViewMonoBehaviour<ExamplePresenter>
{
	private int _exampleCountDisplay;

	public void ExampleUpdateCount(int count)
	{
		_exampleCountDisplay = count;
	}
}
}