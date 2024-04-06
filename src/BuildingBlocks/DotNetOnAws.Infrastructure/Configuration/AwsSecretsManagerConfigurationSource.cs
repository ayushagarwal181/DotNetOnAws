// ------------------------------------------------------------------
// <copyright file="AwsSecretsManagerConfigurationSource.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------
namespace DotNetOnAws.Infrastructure.Configuration
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Aws Secrets Manager Configuration Source.
    /// </summary>
    public class AwsSecretsManagerConfigurationSource : IConfigurationSource
    {
        private readonly AwsSecretsManagerOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsManagerConfigurationSource"/> class.
        /// </summary>
        /// <param name="options">Aws Secrets Manager Options.</param>
        public AwsSecretsManagerConfigurationSource(AwsSecretsManagerOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Builds the Microsoft.Extensions.Configuration.IConfigurationProvider for this source (AwsSecretManagerConfigurationSource).
        /// </summary>
        /// <returns>An Microsoft.Extensions.Configuration.IConfigurationProvider.</returns>
        /// <param name="builder">The Microsoft.Extensions.Configuration.IConfigurationBuilder.</param>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AwsSecretsManagerConfigurationProvider(this.options);
        }
    }
}