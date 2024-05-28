from AlgorithmImports import *

class conservative_reblancing(AlphaModel):

    def __init__(self, benchmark, v_lookback, m_lookback):
        self.benchmark = benchmark
        self.v_lookback = v_lookback
        self.m_lookback = m_lookback
        self.symbols = []
        self.month = -1

    def on_securities_changed(self, algorithm, changes):
        for added in changes.added_securities:
            self.symbols.append(added.symbol)
            # algorithm.Log(f"Added {added.symbol} to universe")

        for removed in changes.removed_securities:
            symbol = removed.symbol
            if symbol in self.symbols:
                self.symbols.remove(symbol)
                # algorithm.Log(f"Removed {symbol} from universe")

    def update(self, algorithm, data):
        # algorithm.Debug(f"Update method called for month {algorithm.time.month}, universe size: {len(self.symbols)}")
        if algorithm.time.month == self.month: return []
        self.month = algorithm.time.month

        # Initialize the data
        alphas = dict()

        # Fetch indicator data
        for symbol in self.symbols:

            # Create the indicators
            roc = algorithm.roc(symbol, 1, Resolution.Daily)
            std = algorithm.std(symbol, self.v_lookback, Resolution.DAILY)
            momp = algorithm.momp(symbol, self.m_lookback, Resolution.DAILY)

            # Get historical data for warm-up
            history = algorithm.History(symbol, max(self.v_lookback, self.m_lookback) + 10, Resolution.DAILY)
            # algorithm.Log(f"History size for {symbol}: {len(history)}")
            # Warm up the indicators
            for idx, row in history.loc[symbol].iterrows():
                roc.Update(idx, row["close"])
                std.Update(idx, roc.current.value)
                momp.Update(idx, row["close"])

            # Compute the rank value
            alphas[symbol] = max(momp.Current.Value / std.Current.Value, 0)
            # algorithm.Log(f"Processing symbol {symbol} with alpha value: {alphas[symbol]}")

        # Rank the symbol by the value of mom/vol
        selected = sorted(alphas.items(), key=lambda x: x[1], reverse=True)[:5]
        selected_symbols = [x[0] for x in selected]
        # algorithm.Debug(f"Selected symbols at {algorithm.Time}: {', '.join([str(symbol) for symbol in selected_symbols])}")        
        return [
            Insight.price(symbol, Expiry.END_OF_MONTH, InsightDirection.UP) for symbol in selected_symbols
        ]