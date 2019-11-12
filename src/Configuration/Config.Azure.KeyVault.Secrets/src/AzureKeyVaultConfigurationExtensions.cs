// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Cryptography.X509Certificates;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// Extension methods for registering <see cref="AzureKeyVaultConfigurationProvider"/> with <see cref="IConfigurationBuilder"/>.
    /// </summary>
    public static class AzureKeyVaultConfigurationExtensions
    {
        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="vault">The Azure KeyVault uri.</param>
        /// <param name="credential">The credential to to use for authentication.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddAzureKeyVault(
            this IConfigurationBuilder configurationBuilder,
            Uri vault,
            TokenCredential credential)
        {
            return AddAzureKeyVault(configurationBuilder, vault, credential, new DefaultKeyVaultSecretManager());
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="vault">Azure KeyVault uri.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddAzureKeyVault(
            this IConfigurationBuilder configurationBuilder,
            Uri vault)
        {
            return AddAzureKeyVault(configurationBuilder, vault, new DefaultAzureCredential());
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="vault">Azure KeyVault uri.</param>
        /// <param name="manager">The <see cref="IKeyVaultSecretManager"/> instance used to control secret loading.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddAzureKeyVault(
            this IConfigurationBuilder configurationBuilder,
            Uri vault,
            IKeyVaultSecretManager manager)
        {
            return AddAzureKeyVault(configurationBuilder, vault, manager, new DefaultKeyVaultSecretManager());
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="vault">Azure KeyVault uri.</param>
        /// <param name="credential">The credential to to use for authentication.</param>
        /// <param name="manager">The <see cref="IKeyVaultSecretManager"/> instance used to control secret loading.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddAzureKeyVault(
            this IConfigurationBuilder configurationBuilder,
            Uri vault,
            TokenCredential credential,
            IKeyVaultSecretManager manager)
        {
            return AddAzureKeyVault(configurationBuilder, new AzureKeyVaultConfigurationOptions(vault, credential)
            {
                Manager = manager
            });
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="client">The <see cref="SecretClient"/> to use for retrieving values.</param>
        /// <param name="manager">The <see cref="IKeyVaultSecretManager"/> instance used to control secret loading.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddAzureKeyVault(
            this IConfigurationBuilder configurationBuilder,
            SecretClient client,
            IKeyVaultSecretManager manager)
        {
            return configurationBuilder.Add(new AzureKeyVaultConfigurationSource(new AzureKeyVaultConfigurationOptions()
            {
                Client = client,
                Manager = manager
            }));
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from the Azure KeyVault.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="options">The <see cref="AzureKeyVaultConfigurationOptions"/> to use.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configurationBuilder, AzureKeyVaultConfigurationOptions options)
        {
            if (configurationBuilder == null)
            {
                throw new ArgumentNullException(nameof(configurationBuilder));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            if (options.Client == null)
            {
                throw new ArgumentNullException(nameof(options.Client));
            }
            if (options.Manager == null)
            {
                throw new ArgumentNullException(nameof(options.Manager));
            }

            configurationBuilder.Add(new AzureKeyVaultConfigurationSource(options));

            return configurationBuilder;
        }
    }
}