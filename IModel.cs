using System;

namespace MVP
{
/// <summary>
/// Represents the model in the MVP (Model-View-Presenter) pattern.
/// The model is responsible for handling the data and business logic of the application.
/// Implements <see cref="IDisposable"/> to allow proper resource management and disposal.
/// </summary>
public interface IModel : IDisposable
{
}
}