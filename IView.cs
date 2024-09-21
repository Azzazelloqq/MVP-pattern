using System;

namespace MVP
{
/// <summary>
/// Represents the view in the MVP (Model-View-Presenter) pattern.
/// The view is responsible for rendering the user interface and receiving user input, while being updated by the presenter.
/// Implements <see cref="IDisposable"/> to allow proper resource management and disposal.
/// </summary>
public interface IView : IDisposable
{
}
}