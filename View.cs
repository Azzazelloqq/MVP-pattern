using Disposable;

namespace MVP
{
public abstract class View<TPresenter> : DisposableBase, IView where TPresenter : IPresenter
{
    protected TPresenter presenter;
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();

    public void Initialize(TPresenter presenter)
    {
        this.presenter = presenter;
    }

    public override void Dispose()
    {
        base.Dispose();
        
        compositeDisposable?.Dispose();
    }
}
}