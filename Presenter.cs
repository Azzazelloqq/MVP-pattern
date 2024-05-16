using Disposable;

namespace MVP
{
public abstract class Presenter<TView, TModel> : DisposableBase, IPresenter 
    where TModel : IModel 
    where TView : IView
{
    protected IView view;
    protected TModel model;
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
    
    public Presenter(TView view, TModel model)
    {
        this.view = view;
        this.model = model;
        
        compositeDisposable.AddDisposable(view, model);
    }

    public override void Dispose()
    {
        base.Dispose();
        compositeDisposable?.Dispose();
    }
}
}