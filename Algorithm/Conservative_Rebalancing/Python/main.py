#region imports
from AlgorithmImports import *
from universe import *
from alpha import *
#endregion

class ConservativeApgorithm(QCAlgorithm):

    def initialize(self):
        self.set_start_date(2018, 1, 1)
        self.set_end_date(2024, 5, 1)
        self.set_cash(100000)            
        # Set number days to trace back
        v_lookback = self.get_parameter("v_lookback", 36)
        m_lookback = self.get_parameter("m_lookback", 12)
        
        self.set_brokerage_model(BrokerageName.INTERACTIVE_BROKERS_BROKERAGE, AccountType.MARGIN)

        # SPY 500 companies
        spy = self.add_equity("SPY",
            resolution = self.universe_settings.resolution,
            data_normalization_mode = self.universe_settings.data_normalization_mode).symbol
        self.set_benchmark(spy)

        # # DOW 30 Companies
        # dia = self.add_equity("DIA",
        #     resolution = self.universe_settings.resolution,
        #     data_normalization_mode = self.universe_settings.data_normalization_mode).symbol
        # self.set_benchmark(dia)
        
        self.set_universe_selection(etf_constituents_universe(spy, self.universe_settings))
        self.add_alpha(conservative_reblancing(spy, v_lookback, m_lookback))
        self.Settings.RebalancePortfolioOnInsightChanges = False
        self.Settings.RebalancePortfolioOnSecurityChanges = False
        self.set_portfolio_construction(EqualWeightingPortfolioConstructionModel())
        self.set_risk_management(NullRiskManagementModel())
        self.set_execution(ImmediateExecutionModel())
