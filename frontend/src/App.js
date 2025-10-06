import React, { useState } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  // Локальные состояния формы и результата
  const [number, setNumber] = useState('');
  const [result, setResult] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [fieldError, setFieldError] = useState('');

  // Клиент для запросов к API; URL можно задать переменной окружения REACT_APP_API_URL
  const api = axios.create({
    baseURL: process.env.REACT_APP_API_URL || '',
    timeout: 10000
  });

  // Отправляет число на сервер и показывает результат
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Базовая клиентская валидация
    if (number === '' || isNaN(number)) {
      setFieldError('Введите число');
      setError('');
      return;
    }

    const numeric = parseFloat(number);
    if (!isFinite(numeric)) {
      setFieldError('Число должно быть конечным');
      setError('');
      return;
    }
    if (numeric < -1e9 || numeric > 1e9) {
      setFieldError('Допустимый диапазон: от -1e9 до 1e9');
      setError('');
      return;
    }

    setLoading(true);
    setError('');
    setFieldError('');
    setResult(null);

    try {
      const response = await api.post('/api/number/multiply', {
        number: numeric
      });
      
      setResult(response.data);
    } catch (err) {
      const serverMessage = typeof err.response?.data === 'string' ? err.response.data : err.message;
      setError('Ошибка при отправке запроса: ' + serverMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="App">
      <div className="container">
        <h1>Умножение числа на 2</h1>
        
        <form onSubmit={handleSubmit} className="form">
          <div className="input-group">
            <label htmlFor="number">Введите число:</label>
            <input
              id="number"
              type="number"
              step="any"
              value={number}
              onChange={(e) => setNumber(e.target.value)}
              placeholder="Введите число..."
              disabled={loading}
            min={-1e9}
            max={1e9}
            />
          </div>
        {fieldError && (
          <div className="error">
            {fieldError}
          </div>
        )}
          
          <button type="submit" disabled={loading}>
            {loading ? 'Отправка...' : 'Умножить на 2'}
          </button>
        </form>

        {error && (
          <div className="error">
            {error}
          </div>
        )}

        {result && (
          <div className="result">
            <h2>Результат:</h2>
            <p>Исходное число: <strong>{result.originalNumber}</strong></p>
            <p>Результат умножения на 2: <strong>{result.multipliedNumber}</strong></p>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;