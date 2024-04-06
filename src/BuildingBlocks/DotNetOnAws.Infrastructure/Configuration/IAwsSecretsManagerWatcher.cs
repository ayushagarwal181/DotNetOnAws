// ------------------------------------------------------------------
// <copyright file="IAwsSecretsManagerWatcher.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------

namespace DotNetOnAws.Infrastructure.Configuration
{
    using System;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Defines a mechanism for watching changes in AWS Secrets Manager.
    /// </summary>
    public interface IAwsSecretsManagerWatcher : IDisposable
    {
        /// <summary>
        /// Watches for changes and provides a change token.
        /// </summary>
        /// <returns>The <see cref="IChangeToken"/> used to observe changes.</returns>
        IChangeToken Watch();
    }
}