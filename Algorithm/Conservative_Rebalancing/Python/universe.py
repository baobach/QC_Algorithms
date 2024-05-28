from AlgorithmImports import *

class etf_constituents_universe(ETFConstituentsUniverseSelectionModel):
    def __init__(self, benchmark, universe_settings: UniverseSettings = None) -> None:
        super().__init__(benchmark, universe_settings, self.etf_constituents_filter)

    def etf_constituents_filter(self, constituents):
        return [c.symbol for c in constituents]