# Construction Material Calculator & Order Manager

## 📌 Overview

A WPF desktop application designed for construction site engineers to calculate material quantities and manage purchase orders efficiently. The system combines engineering calculations with order management in a single, organized workflow.

---

## 🚀 Features

### 🧮 Material Calculators

* Concrete volume calculation (with waste factor)
* Steel weight calculation using standard formula (D²/162)
* Paint quantity estimation (area or dimensions-based)
* Tile quantity calculation with waste percentage

### 📦 Material & Order Management

* Material catalog (CRUD operations)
* Create purchase orders directly from calculated results
* Automatic cost calculation (Quantity × Unit Price)
* Order tracking (Pending / Delivered)

### 📊 Dashboard & Insights

* Summary cards (materials count, orders, total cost)
* Order statistics and filtering (category, date, status)
* Search functionality

### 💾 Data Persistence

* Save and load all data using JSON
* Export order summaries to text files

---

## 🏗️ Application Structure

The application consists of 3 main windows:

1. **MainWindow**

   * Dashboard & Material Catalog
   * Navigation to other modules

2. **CalculatorWindow**

   * 4 calculation modules (Concrete, Steel, Paint, Tiles)
   * Order creation section

3. **OrdersWindow**

   * Orders management
   * Filtering, searching, and statistics

---

## 🛠️ Technologies Used

* C# / .NET
* WPF (Windows Presentation Foundation)
* MVVM (recommended structure)
* Entity-like data handling (Models)
* JSON (System.Text.Json)

---

## 📂 Project Structure

```
/Models
  ├── Material.cs
  ├── Order.cs
  └── AppData.cs

/Windows
  ├── MainWindow.xaml
  ├── CalculatorWindow.xaml
  └── OrdersWindow.xaml

/Helpers
  └── FileHelper.cs
```

---

## ⚙️ How It Works

1. Add materials to the catalog (name, category, unit, price)
2. Use calculators to compute required quantities
3. Create purchase orders from calculated results
4. Track and manage orders
5. Save/load data for persistence

---

## ✅ Key Functional Requirements

* Input validation for all fields
* Prevent duplicate materials
* Prevent invalid calculations
* Ensure smooth data flow between windows
* Maintain consistent UI updates

---

## 📈 Future Improvements

* Apply full MVVM pattern
* Add reporting dashboards (charts/graphs)
* Implement auto-save functionality
* Add authentication system
* Improve UI/UX design

---

## 📷 Use Case

This application simulates a real-world tool used by:

* Site Engineers
* Quantity Surveyors
* Construction Managers

---

## 🧠 Notes

* Focus on clean code and separation of concerns
* Ensure no crashes on invalid input
* Maintain scalable and maintainable structure

---

## 📎 License

This project is for educational purposes.
