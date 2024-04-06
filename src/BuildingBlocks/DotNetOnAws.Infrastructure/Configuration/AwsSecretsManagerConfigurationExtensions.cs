// ------------------------------------------------------------------
// <copyright file="AwsSecretsManagerConfigurationExtensions.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------
namespace CTSI.Infrastructure.Configuration
{
    using DotNetOnAws.Infrastructure.Configuration;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Aws Secrets Manager Configuration Extensions.
    /// </summary>
    public static class AwsSecretsManagerConfigurationExtensions
    {
        /// <summary>
        /// Add Aws Secrets Manager.
        /// </summary>
        /// <returns>An Microsoft.Extensions.Configuration.IConfigurationBuilder.</returns>
        /// <param name="builder">An Microsoft.Extensions.Configuration.IConfigurationBuilder .</param>
        /// <param name="options">Aws Secrets Manager Options.</param>
        public static IConfigurationBuilder AddAwsSecretManager(this IConfigurationBuilder builder, AwsSecretsManagerOptions options)
        {
            return builder.Add(new AwsSecretsManagerConfigurationSource(options));
        }
    }
}