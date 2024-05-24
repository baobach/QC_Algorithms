# README.md

## Overview

Welcome to the repository for hosting algorithmic trading strategies on the QuantConnect platform. This repository contains implementations of various trading algorithms in both Python and C#. Each algorithm is designed to leverage QuantConnect's powerful backtesting and live trading capabilities. All algorithm codes are organized and saved in the `Algorithms` folder.

## Repository Structure

```bash
├── Algorithms
│   ├── Python
│   │   ├── Algorithm1.py
│   │   └── Algorithm2.py
│   ├── C#
│   │   ├── Algorithm1.cs
│   │   └── Algorithm2.cs
├── README.md
└── LICENSE
```

- **Algorithms**: This folder contains all the algorithm implementations.
  - **Python**: Subfolder for algorithms implemented in Python.
  - **C#**: Subfolder for algorithms implemented in C#.

## Getting Started

### Prerequisites

To run these algorithms, you need to have an account on [QuantConnect](https://www.quantconnect.com/) and set up the QuantConnect Lean engine on your local machine or use QuantConnect's cloud platform.

### Running Algorithms

1. **Clone the Repository**:

   ```sh
   git clone https://github.com/baobach/QC_Algorithms.git
   cd QC_Algorithms
   ```

2. **Upload to QuantConnect**:
   - Navigate to the [QuantConnect](https://www.quantconnect.com/) platform.
   - Create a new project.
   - Upload the desired algorithm files from the `Algorithms` folder into your QuantConnect project.

3. **Configure and Run**:
   - Configure the algorithm settings as needed.
   - Run backtests or deploy live on the QuantConnect platform.

## Adding New Algorithms

1. **Implement the Algorithm**:
   - Write your algorithm in Python or C# and save it in the respective folder under `Algorithms`.

2. **Follow the Naming Conventions**:
   - Use meaningful names for your files and include a brief description of the algorithm at the top of the file.

3. **Commit and Push**:

   ```sh
   git add Algorithms/your-algorithm-file
   git commit -m "Add new algorithm: Your Algorithm Name"
   git push origin main
   ```

## Contributing

We welcome contributions to this repository! To contribute, please follow these steps:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature-name`).
3. Make your changes.
4. Commit your changes (`git commit -m 'Add feature'`).
5. Push to the branch (`git push origin feature/your-feature-name`).
6. Create a pull request.

## License

This repository is licensed under the Apache 2.0 License. See the [LICENSE](LICENSE) file for more information.

## Contact

For any questions or suggestions, please open an issue or contact us at [robert@quantfin.net](mailto:robert@quantfin.net).

---

Thank you for using and contributing to our repository! Happy coding and successful trading!
