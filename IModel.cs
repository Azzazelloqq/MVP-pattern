﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVP
{
/// <summary>
/// Represents the model in the MVP (Model-View-Presenter) pattern.
/// The model is responsible for handling the data and business logic of the application.
/// Implements <see cref="IDisposable"/> to allow proper resource management and disposal.
/// </summary>
public interface IModel : IDisposable
{
	/// <summary>
	/// Initializes the presenter async. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	public Task InitializeAsync(CancellationToken token);

	/// <summary>
	/// Initializes the model. This method can be overridden by derived classes to provide custom initialization logic.
	/// </summary>
	public void Initialize();

}
}