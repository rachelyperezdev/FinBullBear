# Finbullbear

## Overview
Finbullbear is a web application built using React (TypeScript) for the frontend and a .NET API for the backend. The application allows users to search for stocks, view detailed financial data for each company, and manage their portfolio. The financial data includes company profiles, income statements, balance sheets, and cash flow statements. The app provides authentication via JWT (JSON Web Tokens), allowing users to securely register, log in, and access their data.

## Features
- **Stock Search**: Search for stocks by company name or ticker symbol.
- **Company Profile**: View detailed information about a company, including industry, market cap, and key statistics.
- **Income Statement**: View the company's financial performance over a specific period.
- **Balance Sheet**: View the company's assets, liabilities, and equity.
- **Cash Flow Statement**: View the movement of cash in and out of the company.
- **Login/Registration**: Secure login and registration system using JWT for user authentication.

## Technologies Used

### Frontend:
- **React** with **TypeScript**: JavaScript library for building user interfaces with type safety.
- **Axios**: A promise-based HTTP client for making API requests.
- **Tailwind CSS**: A utility-first CSS framework for building modern, responsive designs.
- **React Router**: For navigating between pages.
- **React Icons**: A library of customizable icons for UI elements.
- **React Spinners**: Loading spinners for smooth user experience during data fetching.
- **React Toastify**: For showing notifications to users.
- **React Hook Form** with **Yup** and **@hookform/resolver**: For handling form validation and management.

### Backend:
- **.NET API**: Backend service that manages stock data and user authentication.
- **JWT (JSON Web Tokens)**: For secure user authentication and maintaining user sessions.
- **Financial Modeling Prep API**: To retrieve real-time financial data such as company profiles, income statements, balance sheets, and cash flow statements.

## Getting Started

### Prerequisites

To get started, make sure you have the following installed:
- [Node.js](https://nodejs.org/)
- [npm](https://www.npmjs.com/)
- [.NET SDK](https://dotnet.microsoft.com/download)

### Installation

1. **Clone the repository**:
   ```bash
   git clone https://github.com/rachelyperezdev/FinBullBear.git

2. **Install dependencies for the frontend (React)**:
   ```bash
   cd frontend
   npm install

3. **Install dependencies for the backend (.NET)**:
   ```bash
   cd api
   dotnet restore

4. **Create a .env file in the root of the frontend project and add the necessary API URLs**:
   ```bash
   REACT_APP_API_URL=http://localhost:5000/api

## Running the Application

1. Frontend
   ```bash
   npm start

2. Backend
   ```bash
   dotnet run
