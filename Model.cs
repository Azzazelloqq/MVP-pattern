using Disposable;

namespace MVP
{
public class Model : DisposableBase, IModel
{
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
    
    public override void Dispose()
    {
        base.Dispose();
        compositeDisposable?.Dispose();
    }
}
}