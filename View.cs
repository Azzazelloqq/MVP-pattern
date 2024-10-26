﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Disposable;

namespace MVP
{
/// <summary>
/// Represents the base view class in the MVP (Model-View-Presenter) pattern.
/// The view is responsible for rendering the user interface and interacting with the presenter.
/// Inherits from <see cref="DisposableBase"/> to provide built-in resource management and implements <see cref="IView"/>.
/// </summary>
/// <typeparam name="TPresenter">The type of the presenter, which implements <see cref="IPresenter"/>.</typeparam>
public abstract class View<TPresenter> : DisposableBase, IView where TPresenter : IPresenter
{
	/// <summary>
	/// The presenter associated with this view.
	/// </summary>
	protected TPresenter presenter;

	/// <summary>
	/// A composite disposable that manages the disposal of any additional resources associated with the view.
	/// </summary>
	protected readonly ICompositeDisposable compositeDisposable = new CompositeDisposable();
	
	/// <inheritdoc/>
	public virtual void Initialize(IPresenter presenter)
	{
		if (!(presenter is TPresenter correctPresenter)) {
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}
		
		this.presenter = correctPresenter;
		
		OnInitialize();
	}
	
	/// <inheritdoc/>
	public virtual async Task InitializeAsync(IPresenter presenter, CancellationToken token)
	{
		if (!(presenter is TPresenter correctPresenter)) {
			throw new ArgumentException("Presenter must be of type " + typeof(TPresenter).Name, nameof(presenter));
		}

		presenter = correctPresenter;
		
		await OnInitializeAsync(token);
	}
	
	/// <inheritdoc/>
	public override void Dispose()
	{
		base.Dispose();

		OnDispose();
		
		compositeDisposable?.Dispose();
	}

	protected virtual void OnInitialize()
	{
		
	}
	
	protected virtual async Task OnInitializeAsync(CancellationToken token)
	{
		
	}

	protected virtual void OnDispose()
	{
		
	}
}
}