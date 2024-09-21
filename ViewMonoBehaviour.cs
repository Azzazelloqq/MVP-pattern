using Disposable;

namespace MVP
{
	public abstract class ViewMonoBehaviour<TPresenter> : MonoBehaviourDisposable, IView where TPresenter : IPresenter
	{
		protected TPresenter presenter;

		public void Initialize(TPresenter presenter)
		{
			this.presenter = presenter;
		}
	}
}