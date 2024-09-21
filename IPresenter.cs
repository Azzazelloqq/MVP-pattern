using System;

namespace MVP
{
/// <summary>
/// Represents the presenter in the MVP (Model-View-Presenter) pattern.
/// The presenter is responsible for mediating between the view and the model, handling all logic related to updating the view and interacting with the model.
/// Implements <see cref="IDisposable"/> to allow proper resource management and disposal.
/// </summary>
public interface IPresenter : IDisposable
{
}
}