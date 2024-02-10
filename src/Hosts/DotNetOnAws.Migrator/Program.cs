// ------------------------------------------------------------------
// <copyright file="Program.cs" company="Ayush Agarwal.">
// Copyright © 2024 by Ayush Agarwal. Licensed under the MIT License.
// </copyright>
// ------------------------------------------------------------------

namespace DotNetOnAws.Migrator
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// The main class for the DotNetOnAws.Migrator application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point for the DotNetOnAws.Migrator application.
        /// </summary>
        /// <param name="args">The command-line arguments passed to the application.</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
