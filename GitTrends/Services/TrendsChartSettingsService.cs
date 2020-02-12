﻿using System;
using Xamarin.Essentials;

namespace GitTrends
{
    public class TrendsChartSettingsService
    {
        public bool ShouldShowClonesByDefault
        {
            get => Preferences.Get(nameof(ShouldShowClonesByDefault), true);
            set => Preferences.Set(nameof(ShouldShowClonesByDefault), value);
        }

        public bool ShouldShowUniqueClonesByDefault
        {
            get => Preferences.Get(nameof(ShouldShowUniqueClonesByDefault), true);
            set => Preferences.Set(nameof(ShouldShowUniqueClonesByDefault), value);
        }

        public bool ShouldShowViewsByDefault
        {
            get => Preferences.Get(nameof(ShouldShowViewsByDefault), true);
            set => Preferences.Set(nameof(ShouldShowViewsByDefault), value);
        }

        public bool ShouldShowUniqueViewsByDefault
        {
            get => Preferences.Get(nameof(ShouldShowUniqueViewsByDefault), true);
            set => Preferences.Set(nameof(ShouldShowUniqueViewsByDefault), value);
        }

        public TrendsChartOptions CurrentTrendsChartOption
        {
            get => GetCurrentTrendsChartOption();
            set => SetCurrentTrendsChartOption(value);
        }

        TrendsChartOptions GetCurrentTrendsChartOption()
        {
            if (!ShouldShowUniqueClonesByDefault
                && !ShouldShowUniqueViewsByDefault
                && ShouldShowClonesByDefault
                && ShouldShowViewsByDefault)
            {
                return TrendsChartOptions.NoUniques;
            }

            if (ShouldShowUniqueClonesByDefault
                && ShouldShowUniqueViewsByDefault
                && !ShouldShowClonesByDefault
                && !ShouldShowViewsByDefault)
            {
                return TrendsChartOptions.JustUniques;
            }

            //All is the Default Value 
            ShouldShowUniqueClonesByDefault = true;
            ShouldShowUniqueViewsByDefault = true;
            ShouldShowClonesByDefault = true;
            ShouldShowViewsByDefault = true;

            return TrendsChartOptions.All;
        }

        void SetCurrentTrendsChartOption(in TrendsChartOptions currentTrendsChartOption)
        {
            switch (currentTrendsChartOption)
            {
                case TrendsChartOptions.All:
                    ShouldShowUniqueClonesByDefault = true;
                    ShouldShowUniqueViewsByDefault = true;
                    ShouldShowClonesByDefault = true;
                    ShouldShowViewsByDefault = true;
                    break;

                case TrendsChartOptions.JustUniques:
                    ShouldShowUniqueClonesByDefault = true;
                    ShouldShowUniqueViewsByDefault = true;
                    ShouldShowClonesByDefault = false;
                    ShouldShowViewsByDefault = false;
                    break;

                case TrendsChartOptions.NoUniques:
                    ShouldShowUniqueClonesByDefault = false;
                    ShouldShowUniqueViewsByDefault = false;
                    ShouldShowClonesByDefault = true;
                    ShouldShowViewsByDefault = true;
                    break;

                default:
                    throw new NotSupportedException($"{currentTrendsChartOption.ToString()} not supported");
            }
        }
    }

    public enum TrendsChartOptions { All, NoUniques, JustUniques }
}
