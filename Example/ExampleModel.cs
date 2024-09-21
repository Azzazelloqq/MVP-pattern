namespace MVP.Example
{
public class ExampleModel : Model
{
	public int ExampleCount { get; private set; }

	public void ExampleIncreaseCount(int count)
	{
		if (count < 0)
		{
			return;
		}

		ExampleCount += count;
	}
}
}