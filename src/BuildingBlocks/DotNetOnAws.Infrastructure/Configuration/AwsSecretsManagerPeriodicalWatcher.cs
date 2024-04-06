// ------------------------------------------------------------------
// <copyright file="AwsSecretsManagerPeriodicalWatcher.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------

namespace DotNetOnAws.Infrastructure.Configuration
{
    using System;
    using System.Threading;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Periodically watches for changes in AWS Secrets Manager and triggers a refresh.
    /// </summary>
    public class AwsSecretsManagerPeriodicalWatcher : IAwsSecretsManagerWatcher
    {
        private readonly TimeSpan refreshInterval;
        private readonly Timer timer;
        private IChangeToken changeToken;
        private CancellationTokenSource cancellationTokenSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsManagerPeriodicalWatcher"/> class.
        /// </summary>
        /// <param name="refreshInterval">The time interval between each check for changes.</param>
        public AwsSecretsManagerPeriodicalWatcher(TimeSpan refreshInterval)
        {
            this.refreshInterval = refreshInterval;
            this.timer = new Timer(this.Change, null, TimeSpan.Zero, this.refreshInterval);
        }

        /// <summary>
        /// Starts watching for changes and returns a change token that is triggered on changes.
        /// </summary>
        /// <returns>The change token to observe.</returns>
        public IChangeToken Watch()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.changeToken = new CancellationChangeToken(this.cancellationTokenSource.Token);

            return this.changeToken;
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="AwsSecretsManagerPeriodicalWatcher"/> class.
        /// </summary>
        public void Dispose()
        {
            this.timer?.Dispose();
            this.cancellationTokenSource?.Dispose();

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Triggers a change event.
        /// </summary>
        /// <param name="state">The state object passed to the callback method.</param>
        private void Change(object state)
        {
            this.cancellationTokenSource?.Cancel();
        }
    }
}