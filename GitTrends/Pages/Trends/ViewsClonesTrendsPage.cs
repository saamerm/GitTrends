﻿using GitTrends.Mobile.Common;
using GitTrends.Shared;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace GitTrends
{
    class ViewsClonesTrendsPage : BaseTrendsContentPage
    {
        public ViewsClonesTrendsPage(IMainThread mainThread, IAnalyticsService analyticsService) : base(mainThread, 0, analyticsService)
        {

        }

        protected override Layout CreateHeaderView() => new ViewsClonesStatisticsGrid();
        protected override BaseChartView CreateChartView() => new ViewClonesChart(MainThread);
        protected override EmptyDataView CreateEmptyDataView() => new EmptyDataView(TrendsPageAutomationIds.ViewsClonesEmptyDataView)
                                                                    .Bind(IsVisibleProperty, nameof(TrendsViewModel.IsViewsClonesEmptyDataViewVisible))
                                                                    .Bind(EmptyDataView.TitleProperty, nameof(TrendsViewModel.ViewsClonesEmptyDataViewTitleText))
                                                                    .Bind(EmptyDataView.ImageSourceProperty, nameof(TrendsViewModel.ViewsClonesEmptyDataViewImage));
    }
}
