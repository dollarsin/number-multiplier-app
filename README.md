# NumberMultiplier (C# + React)

Простое клиент–серверное приложение: форма принимает число, отправляет его на бэкенд (.NET 8), бэкенд возвращает число, умноженное на 2.

## Запуск локально
- Бэкенд:
  - `dotnet restore backend`
  - `dotnet run --project backend -c Development`
  - Проверка: `http://localhost:5000/swagger`, `http://localhost:5000/health`
- Фронтенд:
  - `cd frontend`
  - Создайте `.env` с `REACT_APP_API_URL=http://localhost:5000`
  - `npm install`
  - `npm start` → `http://localhost:3000`

## API
- POST `/api/number/multiply`
  - Тело: `{ "number": 12.5 }`
  - Ответ: `{ "originalNumber": 12.5, "multipliedNumber": 25 }`

## Валидация
- Клиент и сервер проверяют: пустое значение, NaN/Infinity, диапазон [-1e9; 1e9].

## Тесты
- `dotnet test number-multiplier-app.sln`

## Публикация на GitHub (быстрый вариант)
1. Инициализация и первый коммит:
   - `git init`
   - `git add .`
   - `git commit -m "Initial commit: NumberMultiplier"`
2. Создайте пустой репозиторий на GitHub (без README). Затем свяжите и запушьте:
   - `git remote add origin https://github.com/<your-username>/<your-repo>.git`
   - `git branch -M main`
   - `git push -u origin main`
