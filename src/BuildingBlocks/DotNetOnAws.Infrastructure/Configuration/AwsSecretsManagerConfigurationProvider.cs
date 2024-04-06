// ------------------------------------------------------------------
// <copyright file="AwsSecretsManagerConfigurationProvider.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------

namespace DotNetOnAws.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using Amazon;
    using Amazon.SecretsManager;
    using Amazon.SecretsManager.Model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Provides configuration values from AWS Secrets Manager.
    /// </summary>
    public class AwsSecretsManagerConfigurationProvider : ConfigurationProvider, IDisposable
    {
        private readonly AwsSecretsManagerOptions options;
        private readonly IDisposable changeTokenRegistration;
        private readonly AwsSecretsManagerPeriodicalWatcher watcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsSecretsManagerConfigurationProvider"/> class.
        /// </summary>
        /// <param name="options">Aws Secrets Manager Options.</param>
        public AwsSecretsManagerConfigurationProvider(AwsSecretsManagerOptions options)
        {
            this.options = options;
            this.watcher = new AwsSecretsManagerPeriodicalWatcher(TimeSpan.FromSeconds(options.ReloadTimeInSeconds));
            this.changeTokenRegistration = ChangeToken.OnChange(() => this.watcher.Watch(), this.Load);
        }

        /// <summary>
        /// Loads configuration values from AWS Secrets Manager.
        /// </summary>
        public override void Load()
        {
            var secret = this.GetSecret();

            if (this.options?.IsKeyValuePair ?? false)
            {
                this.Data = JsonSerializer.Deserialize<Dictionary<string, string>>(secret);
            }
            else
            {
                this.Data = DeserializeToDictionary(secret);
            }
        }

        /// <summary>
        /// Disposes of the resources used by this provider.
        /// </summary>
        public void Dispose()
        {
            this.changeTokenRegistration?.Dispose();
            this.watcher?.Dispose();

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Deserializes the JSON string to a dictionary.
        /// </summary>
        /// <param name="jsonString">The JSON string to deserialize.</param>
        /// <returns>A dictionary representing the JSON data.</returns>
        private static Dictionary<string, string> DeserializeToDictionary(string jsonString)
        {
            var finalOutput = new Dictionary<string, string>();

            using (JsonDocument document = JsonDocument.Parse(jsonString))
            {
                foreach (JsonProperty property in document.RootElement.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Object)
                    {
                        var innerObjDict = DeserializeToDictionary(property.Value.GetRawText());

                        foreach (KeyValuePair<string, string> innerObj in innerObjDict)
                        {
                            finalOutput.Add($"{property.Name}:{innerObj.Key}", innerObj.Value);
                        }
                    }
                    else if (property.Value.ValueKind == JsonValueKind.Array)
                    {
                        var list = JsonSerializer.Deserialize<List<string>>(property.Value.GetRawText());

                        int count = 0;
                        foreach (var listItem in list)
                        {
                            finalOutput.Add($"{property.Name}:{count++}", listItem);
                        }
                    }
                    else
                    {
                        finalOutput.Add(property.Name, property.Value.ToString());
                    }
                }
            }

            return finalOutput;
        }

        /// <summary>
        /// Retrieves the secret value from AWS Secrets Manager.
        /// </summary>
        /// <returns>The secret as a string.</returns>
        private string GetSecret()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = this.options.SecretName,
                VersionStage = "AWSCURRENT",
            };

            using (var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(this.options.Region)))
            {
                var response = client.GetSecretValueAsync(request).Result;

                string secretString;
                if (response.SecretString != null)
                {
                    secretString = response.SecretString;
                }
                else
                {
                    var memoryStream = response.SecretBinary;
                    using var reader = new StreamReader(memoryStream);
                    secretString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                }

                return secretString;
            }
        }
    }
}