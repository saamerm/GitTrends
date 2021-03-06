﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using GitTrends.Shared;
using Xamarin.Essentials.Interfaces;

namespace GitTrends
{
    public class SplashScreenViewModel : BaseViewModel
    {
        readonly static WeakEventManager<InitializationCompleteEventArgs> _initializationCompletedEventManager = new();

        public SplashScreenViewModel(IMainThread mainThread,
                                        IAnalyticsService analyticsService,
                                        SyncfusionService syncfusionService,
                                        MediaElementService mediaElementService,
                                        NotificationService notificationService,
                                        GitTrendsContributorsService gitTrendsContributorsService) : base(analyticsService, mainThread)
        {
            InitializeAppCommand = new AsyncCommand(() => ExecuteInitializeAppCommand(syncfusionService, mediaElementService, notificationService, gitTrendsContributorsService));
        }

        public static event EventHandler<InitializationCompleteEventArgs> InitializationCompleted
        {
            add => _initializationCompletedEventManager.AddEventHandler(value);
            remove => _initializationCompletedEventManager.RemoveEventHandler(value);
        }

        public IAsyncCommand InitializeAppCommand { get; }

        async Task ExecuteInitializeAppCommand(SyncfusionService syncFusionService, MediaElementService mediaElementService, NotificationService notificationService, GitTrendsContributorsService gitTrendsContributorsService)
        {
            bool isInitializationSuccessful = false;

            try
            {
                var initializeSyncfusionTask = syncFusionService.Initialize(CancellationToken.None);
                var initializeNotificationServiceTask = notificationService.Initialize(CancellationToken.None);
                var intializeOnboardingChartValueTask = mediaElementService.InitializeOnboardingChart(CancellationToken.None);
                var initializeGitTrendsContributorsTask = gitTrendsContributorsService.Initialize(CancellationToken.None);
#if DEBUG
                initializeSyncfusionTask.SafeFireAndForget(ex => AnalyticsService.Report(ex));
                initializeNotificationServiceTask.SafeFireAndForget(ex => AnalyticsService.Report(ex));
#else
                await initializeSyncfusionTask.ConfigureAwait(false);
                await initializeNotificationServiceTask.ConfigureAwait(false);
#endif
                await intializeOnboardingChartValueTask.ConfigureAwait(false);
                await initializeGitTrendsContributorsTask.ConfigureAwait(false);

                isInitializationSuccessful = true;
            }
            catch (Exception e)
            {
                AnalyticsService.Report(e);
            }
            finally
            {
                OnInitializationCompleted(isInitializationSuccessful);
            }
        }

        void OnInitializationCompleted(bool isInitializationSuccessful) =>
            _initializationCompletedEventManager.RaiseEvent(this, new InitializationCompleteEventArgs(isInitializationSuccessful), nameof(InitializationCompleted));
    }
}
