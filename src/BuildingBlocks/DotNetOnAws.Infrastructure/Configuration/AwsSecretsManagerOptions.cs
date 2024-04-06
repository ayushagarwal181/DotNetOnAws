// ------------------------------------------------------------------
// <copyright file="AwsSecretsManagerOptions.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------

namespace DotNetOnAws.Infrastructure.Configuration
{
    /// <summary>
    /// Represents the configuration options for AWS Secrets Manager.
    /// </summary>
    public class AwsSecretsManagerOptions
    {
        /// <summary>
        /// Gets or sets the name of the secret.
        /// </summary>
        public string SecretName { get; set; }

        /// <summary>
        /// Gets or sets the AWS region for Secrets Manager.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the secret's data is in key-value pair format.
        /// </summary>
        public bool IsKeyValuePair { get; set; }

        /// <summary>
        /// Gets or sets the Reload Time In Seconds.
        /// </summary>
        public int ReloadTimeInSeconds { get; set; }
    }
}