# Conservative Rebalancing Approach

## Introduction

This repository contains the implementation of the Conservative Formula strategy, adapted for use with the S&P 500 constituents on the QuantConnect platform. The Conservative Formula, as outlined in the paper "The Conservative Formula: Quantitative Investing made Easy" by Pim van Vliet and David Blitz (2018), selects stocks based on three key criteria: low return volatility, high net payout yield, and strong price momentum. This implementation focuses on selecting the top 50 stocks from the S&P 500 based on these criteria.

## Approach

### QuantConnect Platform

To replicate the results of the Conservative Formula using the S&P 500 constituents on the QuantConnect platform, the following steps were taken:

1. **Data Collection**: Gather historical data for the S&P 500 constituents, including past returns and net payout yield.

2. **Stock Screening**: From the S&P 500, screen and identify the top 50 stocks based on their historical return volatility and momentum.

3. **Alpha Generation**: Implement an alpha model using momentum indicators and the standard deviation of past returns to generate alphas for the selected stocks.

4. **Portfolio Construction**: Construct a portfolio with the top 50 selected stocks, equally weighted, and rebalance it on a quarterly basis.

### Code Structure

- **Main.cs**: Contains the main execution logic, setting up the algorithm, scheduling events, and managing the overall strategy execution.
- **Alpha.cs**: Implements the alpha model, using momentum indicators and the standard deviation of past returns to generate stock alphas.

### Detailed Steps

1. **Initialize Algorithm**: Set up the algorithm parameters in `Main.cs`, including data subscriptions and universe selection for the S&P 500 constituents.

2. **Alpha Model**: In `Alpha.cs`, use momentum indicators to evaluate the performance of stocks and calculate their standard deviation of past returns. Generate alphas based on these calculations to rank the stocks.

3. **Selection and Rebalancing**: Select the top 50 stocks based on the generated alphas and rebalance the portfolio quarterly to maintain these positions.

## Results

The strategy implementation aims to replicate the strong returns and reduced risk profile of the Conservative Formula, adapted for the S&P 500 universe. The results of the backtesting and performance metrics can be found in the `results` folder.

## Conclusion

This repository demonstrates an efficient implementation of the Conservative Formula strategy on the QuantConnect platform, tailored to the S&P 500 constituents. The strategy leverages momentum and volatility metrics to identify top-performing stocks, providing a robust and practical investment approach.

## References

- van Vliet, P., & Blitz, D. (2018). The Conservative Formula: Quantitative Investing made Easy. Available at SSRN: <https://ssrn.com/abstract=3145152>.
