// ------------------------------------------------------------------
// <copyright file="ConfigurationCollectionExtensions.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------

namespace DotNetOnAws.Infrastructure.Configuration
{
    using System;
    using CTSI.Infrastructure.Configuration;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configuration Collection Extensions.
    /// </summary>
    public static class ConfigurationCollectionExtensions
    {
        /// <summary>
        /// Adds Application Configuration.
        /// </summary>
        /// <returns>An Microsoft.Extensions.Configuration.IConfigurationBuilder.</returns>
        /// <param name="configurationBuilder">An Microsoft.Extensions.Configuration.IConfigurationBuilder .</param>
        public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder configurationBuilder)
        {
            var envValue = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToUpperInvariant();

            if (envValue != "DEVELOPMENT")
            {
                var secretRegion = GetEnvironmentVariableOrDefault("SECRET_REGION", "us-east-2");
                var isKeyValuePairString = GetEnvironmentVariableOrDefault("SECRET_IS_KEY_VALUE_PAIR", "false");
                var reloadTimeInSecondsString = GetEnvironmentVariableOrDefault("SECRET_RELOAD_TIME_IN_SECONDS", "30");

                if (!bool.TryParse(isKeyValuePairString, out bool isKeyValuePair))
                {
                    isKeyValuePair = false;
                }

                if (!int.TryParse(reloadTimeInSecondsString, out int reloadTimeInSeconds))
                {
                    reloadTimeInSeconds = 30;
                }

                configurationBuilder.AddAwsSecretManager(
                    new AwsSecretsManagerOptions
                    {
                        SecretName = $"DOTNET_ON_AWS_SECRETS_{envValue}",
                        Region = secretRegion,
                        IsKeyValuePair = isKeyValuePair,
                        ReloadTimeInSeconds = reloadTimeInSeconds,
                    });
            }

            return configurationBuilder;
        }

        private static string GetEnvironmentVariableOrDefault(string variableName, string defaultValue)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? defaultValue;
        }
    }
}