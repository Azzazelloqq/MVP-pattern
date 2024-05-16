using Disposable;
using UnityEngine;

namespace MVP
{
public abstract class ViewMonoBehaviour<TPresenter> : MonoBehaviour, IView
    where TPresenter : IPresenter
{
    protected TPresenter presenter;
    protected bool isDisposed;
    protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();

    public void Initialize(TPresenter presenter)
    {
        this.presenter = presenter;
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }
        
        compositeDisposable?.Dispose();
        
        Destroy(gameObject);

        isDisposed = true;
    }
}
}