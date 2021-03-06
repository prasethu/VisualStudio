﻿using GitHub.Authentication;
using GitHub.Models;
using GitHub.Services;
using Microsoft.TeamFoundation.Git.Controls.Extensibility;
using NSubstitute;
using Rothko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    internal static class Substitutes
    {
        public static IGitRepositoriesExt IGitRepositoriesExt
        {
            get
            {
                var ret = Substitute.For<IGitRepositoriesExt>();
                return ret;
            }
        }

        public static IVSServices IVSServices
        {
            get
            {
                var ret = Substitute.For<IVSServices>();
                ret.GetLocalClonePathFromGitProvider().Returns(@"c:\foo\bar");
                return ret;
            }
        }

        public static IOperatingSystem OperatingSystem
        {
            get
            {
                var ret = Substitute.For<IOperatingSystem>();
                // this expansion happens when the GetLocalClonePathFromGitProvider call is setup by default
                // see IVSServices property above
                ret.Environment.ExpandEnvironmentVariables(Args.String).Returns(x => x[0]);
                return ret;
            }
        }

        public static IExportFactoryProvider ExportFactoryProvider { get { return Substitute.For<IExportFactoryProvider>(); } }

        public static IRepositoryCreationService RepositoryCreationService { get { return Substitute.For<IRepositoryCreationService>(); } }
        public static IRepositoryCloneService RepositoryCloneService { get { return Substitute.For<IRepositoryCloneService>(); } }

        public static IRepositoryHosts RepositoryHosts { get { return Substitute.For<IRepositoryHosts>(); } }
        public static IConnection Connection { get { return Substitute.For<IConnection>(); } }
        public static IConnectionManager ConnectionManager { get { return Substitute.For<IConnectionManager>(); } }
        public static ITwoFactorChallengeHandler TwoFactorChallengeHandler { get { return Substitute.For<ITwoFactorChallengeHandler>(); } }

        /// <summary>
        /// This returns a service provider with everything mocked except for 
        /// RepositoryCloneService and RepositoryCreationService, which are real
        /// instances.
        /// </summary>
        public static IServiceProvider ServiceProvider { get { return GetServiceProvider();  } }

        /// <summary>
        /// This returns a service provider with mocked IRepositoryCreationService and
        /// IRepositoryCloneService as well as all other services mocked. The regular
        /// GetServiceProvider method (and ServiceProvider property return a IServiceProvider
        /// with real RepositoryCloneService and RepositoryCreationService instances.
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider GetFullyMockedServiceProvider()
        {
            return GetServiceProvider(RepositoryCloneService, RepositoryCreationService);
        }

        /// <summary>
        /// This returns a service provider with everything mocked except for 
        /// RepositoryCloneService and RepositoryCreationService, which are real
        /// instances.
        /// </summary>
        /// <param name="cloneService"></param>
        /// <param name="creationService"></param>
        /// <returns></returns>
        public static IServiceProvider GetServiceProvider(
            IRepositoryCloneService cloneService = null,
            IRepositoryCreationService creationService = null,
            IAvatarProvider avatarProvider = null)
        {
            var ret = Substitute.For<IServiceProvider, IUIProvider>();
            var os = OperatingSystem;
            var git = IGitRepositoriesExt;
            var vs = IVSServices;
            var clone = cloneService ?? new RepositoryCloneService(os, vs);
            var create = creationService ?? new RepositoryCreationService(clone);
            var hosts = RepositoryHosts;
            var exports = ExportFactoryProvider;
            var connection = Connection;
            var connectionManager = ConnectionManager;
            var twoFactorChallengeHandler = TwoFactorChallengeHandler;
            avatarProvider = avatarProvider ?? Substitute.For<IAvatarProvider>();
            ret.GetService(typeof(IGitRepositoriesExt)).Returns(git);
            ret.GetService(typeof(IVSServices)).Returns(vs);
            ret.GetService(typeof(IOperatingSystem)).Returns(os);
            ret.GetService(typeof(IRepositoryCloneService)).Returns(clone);
            ret.GetService(typeof(IRepositoryCreationService)).Returns(create);
            ret.GetService(typeof(IRepositoryHosts)).Returns(hosts);
            ret.GetService(typeof(IExportFactoryProvider)).Returns(exports);
            ret.GetService(typeof(IConnection)).Returns(connection);
            ret.GetService(typeof(IConnectionManager)).Returns(connectionManager);
            ret.GetService(typeof(IAvatarProvider)).Returns(avatarProvider);
            ret.GetService(typeof(ITwoFactorChallengeHandler)).Returns(twoFactorChallengeHandler);
            return ret;
        }

        public static IGitRepositoriesExt GetGitExt(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IGitRepositoriesExt)) as IGitRepositoriesExt;
        }

        public static IVSServices GetVSServices(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IVSServices)) as IVSServices;
        }

        public static IOperatingSystem GetOperatingSystem(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IOperatingSystem)) as IOperatingSystem;
        }

        public static IRepositoryCloneService GetRepositoryCloneService(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IRepositoryCloneService)) as IRepositoryCloneService;
        }

        public static IRepositoryCreationService GetRepositoryCreationService(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IRepositoryCreationService)) as IRepositoryCreationService;
        }

        public static IRepositoryHosts GetRepositoryHosts(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IRepositoryHosts)) as IRepositoryHosts;
        }

        public static IExportFactoryProvider GetExportFactoryProvider(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IExportFactoryProvider)) as IExportFactoryProvider;
        }

        public static IConnection GetConnection(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IConnection)) as IConnection;
        }

        public static IConnectionManager GetConnectionManager(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IConnectionManager)) as IConnectionManager;
        }

        public static IAvatarProvider GetAvatarProvider(this IServiceProvider provider)
        {
            return provider.GetService(typeof(IAvatarProvider)) as IAvatarProvider;
        }

        public static ITwoFactorChallengeHandler GetTwoFactorChallengeHandler(this IServiceProvider provider)
        {
            return provider.GetService(typeof(ITwoFactorChallengeHandler)) as ITwoFactorChallengeHandler;
        }
    }
}
