#region imports
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Globalization;
    using System.Drawing;
    using QuantConnect;
    using QuantConnect.Algorithm.Selection;
    using QuantConnect.Algorithm.Framework;
    using QuantConnect.Algorithm.Framework.Selection;
    using QuantConnect.Algorithm.Framework.Alphas;
    using QuantConnect.Algorithm.Framework.Portfolio;
    using QuantConnect.Algorithm.Framework.Execution;
    using QuantConnect.Algorithm.Framework.Risk;
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
    using QuantConnect.Securities.Positions;
    using QuantConnect.Securities.Forex;
    using QuantConnect.Securities.Crypto;
    using QuantConnect.Securities.Interfaces;
    using QuantConnect.Storage;
    using QCAlgorithmFramework = QuantConnect.Algorithm.QCAlgorithm;
    using QCAlgorithmFrameworkBridge = QuantConnect.Algorithm.QCAlgorithm;
#endregion

namespace QuantConnect.Algorithm.CSharp
{
    public class ConservativeRebalancingAlphaModel : AlphaModel
    {
        private int _vLookback;
        private int _mLookback;
        private List<Symbol> _symbols;
        private int _month;

        public ConservativeRebalancingAlphaModel(int vLookback, int mLookback)
        {
            _vLookback = vLookback;
            _mLookback = mLookback;
            _symbols = new List<Symbol>();
            _month = -1;
        }

        public override void OnSecuritiesChanged(QCAlgorithmFramework algorithm, SecurityChanges changes)
        {
            foreach (var added in changes.AddedSecurities)
            {
                _symbols.Add(added.Symbol);
            }

            foreach (var removed in changes.RemovedSecurities)
            {
                _symbols.Remove(removed.Symbol);
                algorithm.Liquidate(removed.Symbol);
            }
        }

        public override IEnumerable<Insight> Update(QCAlgorithmFramework algorithm, Slice data)
        {
            if (algorithm.Time.Day != 1 || algorithm.Time.Month == _month)
            {
                algorithm.Log($"List of constituents: {string.Join(", ", _symbols.Select(s => s.Value))}");
                return Enumerable.Empty<Insight>();
            }

            _month = algorithm.Time.Month;

            var alphas = new Dictionary<Symbol, decimal>();

            foreach (var symbol in _symbols)
            {
                if (!data.ContainsKey(symbol))
                {
                    continue;
                }

                var roc = algorithm.ROC(symbol, 1, Resolution.Daily);
                var std = algorithm.STD(symbol, _vLookback, Resolution.Daily);
                var momp = algorithm.MOMP(symbol, _mLookback, Resolution.Daily);

                var history = algorithm.History(symbol, Math.Max(_vLookback, _mLookback)+1, Resolution.Daily);

                foreach (var row in history)
                {
                    roc.Update(row.EndTime, row.Close);
                    std.Update(row.EndTime, roc.Current.Value);
                    momp.Update(row.EndTime, row.Close);
                }
                // Log the readiness of the indicators
                algorithm.Log($"Indicators for {symbol} at {algorithm.Time}: ROC ready = {roc.IsReady}, STD ready = {std.IsReady}, MOMP ready = {momp.IsReady}");
                alphas[symbol] = momp.Current.Value / std.Current.Value;
                algorithm.Log($"Alpha value for {symbol}: {alphas[symbol]}");
            }

            var selected = alphas.OrderByDescending(x => x.Value).Take(50).Select(x => x.Key);
            // Log the selected symbols
            algorithm.Debug($"Selected symbols at {algorithm.Time}: {string.Join(", ", selected)}");

            // int daysInMonth = DateTime.DaysInMonth(algorithm.Time.Year, algorithm.Time.Month);
            // TimeSpan lifespan = TimeSpan.FromDays(daysInMonth - 1);

            return selected.Select(symbol => new Insight(symbol, TimeSpan.FromDays(2), InsightType.Price, InsightDirection.Up));
        }
    }
}