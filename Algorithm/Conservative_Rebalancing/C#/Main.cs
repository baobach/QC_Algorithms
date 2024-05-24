#region imports
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System.Drawing;
    using QuantConnect;
    using QuantConnect.Algorithm.Framework;
    using QuantConnect.Algorithm.Framework.Selection;
    using QuantConnect.Algorithm.Framework.Alphas;
    using QuantConnect.Algorithm.Framework.Portfolio;
    using QuantConnect.Algorithm.Framework.Execution;
    using QuantConnect.Algorithm.Framework.Risk;
    using QuantConnect.Algorithm.Selection;
    using QuantConnect.Parameters;
    using QuantConnect.Benchmarks;
    using QuantConnect.Brokerages;
    using QuantConnect.Util;
    using QuantConnect.Interfaces;
    using QuantConnect.Algorithm;
    using QuantConnect.Indicators;
    using QuantConnect.Data;
    using QuantConnect.Data.Consolidators;
    using QuantConnect.Data.Custom;
    using QuantConnect.DataSource;
    using QuantConnect.Data.Fundamental;
    using QuantConnect.Data.Market;
    using QuantConnect.Data.UniverseSelection;
    using QuantConnect.Notifications;
    using QuantConnect.Orders;
    using QuantConnect.Orders.Fees;
    using QuantConnect.Orders.Fills;
    using QuantConnect.Orders.Slippage;
    using QuantConnect.Scheduling;
    using QuantConnect.Securities;
    using QuantConnect.Securities.Equity;
    using QuantConnect.Securities.Future;
    using QuantConnect.Securities.Option;
    using QuantConnect.Securities.Forex;
    using QuantConnect.Securities.Crypto;   
    using QuantConnect.Securities.Interfaces;
    using QuantConnect.Storage;
    using QCAlgorithmFramework = QuantConnect.Algorithm.QCAlgorithm;
    using QCAlgorithmFrameworkBridge = QuantConnect.Algorithm.QCAlgorithm;
#endregion

namespace QuantConnect.Algorithm.CSharp
{
    public class ConservativeAlgorithm : QCAlgorithm
    {
        public override void Initialize()
        {                       
            SetStartDate(2018, 1, 1);
            SetEndDate(2024, 1, 1);
            SetCash(100000);

            // Set number days to trace back
            int vLookback = 36; 
            int mLookback = 12; 
            // SetWarmUp(40);

            SetBrokerageModel(BrokerageName.InteractiveBrokersBrokerage, AccountType.Margin);

            // SPY 500 companies
            string spy = "SPY";
            SetBenchmark(AddEquity(spy, Resolution.Daily).Symbol);
            UniverseSettings.Resolution = Resolution.Daily;
            UniverseSettings.Schedule.On(DateRules.MonthStart());
            AddUniverseSelection(new ETFConstituentsUniverseSelectionModel(spy));
            SetAlpha(new ConservativeRebalancingAlphaModel(vLookback, mLookback));
            // Set the portfolio construction to rebalance on the first trading day of each month
            Settings.RebalancePortfolioOnInsightChanges = false;
            Settings.RebalancePortfolioOnSecurityChanges = false;
            SetPortfolioConstruction(new EqualWeightingPortfolioConstructionModel(
                time =>
                {
                    // Rebalance on the first trading day of each month
                    if (time.Day == 1)
                    {
                        return time;
                    }
                    return null;
                }));
            SetRiskManagement(new NullRiskManagementModel());
            SetExecution(new ImmediateExecutionModel());
        }
    }
}