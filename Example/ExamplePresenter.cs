namespace MVP.Example
{
public class ExamplePresenter : Presenter<ExampleView, ExampleModel>
{
	public ExamplePresenter(ExampleView view, ExampleModel model) : base(view, model)
	{
	}

	public void ExampleIncreaseCount(int count)
	{
		model.ExampleIncreaseCount(count);
		var modelExampleCount = model.ExampleCount;
		view.ExampleUpdateCount(modelExampleCount);
	}
}
}