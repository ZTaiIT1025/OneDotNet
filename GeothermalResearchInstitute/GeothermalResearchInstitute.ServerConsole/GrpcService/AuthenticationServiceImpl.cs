// <copyright file="AuthenticationServiceImpl.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Linq;
using System.Threading.Tasks;
using GeothermalResearchInstitute.ServerConsole.Model;
using GeothermalResearchInstitute.v1;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GeothermalResearchInstitute.ServerConsole.GrpcService
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Performance", "CA1812", Justification = "Instantiated with reflection.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Design", "CA1062:验证公共方法的参数", Justification = "由Grpc框架保证.")]
    public class AuthenticationServiceImpl
        : AuthenticationService.AuthenticationServiceBase
    {
        private readonly ILogger<AuthenticationServiceImpl> logger;
        private readonly IServiceProvider serviceProvider;

        public AuthenticationServiceImpl(
            ILogger<AuthenticationServiceImpl> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            if (this.logger.IsEnabled(LogLevel.Debug))
            {
                var authenticationOptions = this.serviceProvider.GetRequiredService<IOptionsSnapshot<AuthenticationOptions>>();
                foreach (var c in authenticationOptions.Value.Credentials)
                {
                    this.logger.LogDebug(c.ToString());
                }
            }
        }

        public override Task<AuthenticateResponse> Authenticate(
            AuthenticateRequest request, ServerCallContext context)
        {
            var authenticationOptions = this.serviceProvider.GetRequiredService<IOptionsSnapshot<AuthenticationOptions>>();
            var credential = authenticationOptions.Value.Credentials.SingleOrDefault(
                c => string.Equals(c.Username, request.Username, StringComparison.Ordinal) &&
                     string.Equals(c.Password, request.Password, StringComparison.Ordinal));
            if (credential == null)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated, "Invalid username or password."));
            }
            else
            {
                return Task.FromResult(new AuthenticateResponse()
                {
                    Nickname = credential.Nickname,
                    Role = credential.Role,
                });
            }
        }
    }
}
