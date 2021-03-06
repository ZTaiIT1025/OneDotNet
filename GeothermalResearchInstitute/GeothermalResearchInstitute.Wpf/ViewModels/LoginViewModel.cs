// <copyright file="LoginViewModel.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Windows.Controls;
using GeothermalResearchInstitute.v2;
using GeothermalResearchInstitute.Wpf.Common;
using GeothermalResearchInstitute.Wpf.Options;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Prism.Commands;
using Prism.Regions;
using Prism.Validation;

namespace GeothermalResearchInstitute.Wpf.ViewModels
{
    public class LoginViewModel : ValidatableBindableBase, IRegionMemberLifetime
    {
        private readonly IOptions<CoreOptions> coreOptions;
        private readonly IRegionManager regionManager;
        private readonly DeviceService.DeviceServiceClient client;
        private ViewModelContext viewModelContext;
        private string username = string.Empty;
        private string password = string.Empty;

        public LoginViewModel(
            IOptions<CoreOptions> coreOptions,
            IRegionManager regionManager,
            DeviceService.DeviceServiceClient client)
        {
            this.coreOptions = coreOptions ?? throw new ArgumentNullException(nameof(coreOptions));
            this.regionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            this.client = client ?? throw new ArgumentNullException(nameof(client));

            this.LoginCommand = new DelegateCommand<PasswordBox>(this.ExecuteLoginCommand);
            this.NavigateBackCommand = new DelegateCommand(this.ExecuteNavigateBackCommand);
        }

        public ViewModelContext ViewModelContext
        {
            get => this.viewModelContext;
            set => this.SetProperty(ref this.viewModelContext, value);
        }

        [Required]
        [StringLength(48, MinimumLength = 3)]
        public string Username
        {
            get => this.username;
            set => this.SetProperty(ref this.username, value);
        }

        [Required]
        [StringLength(48, MinimumLength = 3)]
        public string Password
        {
            get => this.password;
            set => this.SetProperty(ref this.password, value);
        }

        public DelegateCommand<PasswordBox> LoginCommand { get; }

        public DelegateCommand NavigateBackCommand { get; }

        public bool KeepAlive => false;

        private async void ExecuteLoginCommand(PasswordBox passwordBox)
        {
            this.Password = passwordBox.Password;  // Use SecuredPassword in the future.

            try
            {
                AuthenticateResponse response = await this.client.AuthenticateAsync(
                    new AuthenticateRequest
                    {
                        Username = this.Username,
                        Password = this.Password,
                    },
                    deadline: DateTime.UtcNow.AddMilliseconds(this.coreOptions.Value.DefaultReadTimeoutMillis));
                this.ViewModelContext.Principal = new GenericPrincipal(
                    new GenericIdentity(response.Nickname, "Remote"),
                    new[] { ProtoUtils.ConvertToString(response.Role) });
                this.ExecuteNavigateBackCommand();
            }
            catch (RpcException e)
            {
                e.ShowMessageBox();
            }
        }

        private void ExecuteNavigateBackCommand()
        {
            this.regionManager.RequestNavigate(Constants.ContentRegion, this.ViewModelContext.NavigateBackTarget);
        }
    }
}
