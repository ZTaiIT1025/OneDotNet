// <copyright file="UserBarModule.cs" company="Shuai Zhang">
// Copyright Shuai Zhang. All rights reserved.
// Licensed under the GPLv3 license. See LICENSE file in the project root for full license information.
// </copyright>

using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using PrismLab.Common;
using PrismLab.Views;

namespace PrismLab.Modules
{
    public class UserBarModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            containerProvider.Resolve<IRegionManager>()
                .RegisterViewWithRegion(Constants.UserBarRegion, typeof(UserBarView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
