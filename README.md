# 💬 Habit Tracker

Third project as part of the [C# Academy](https://www.thecsharpacademy.com/#). 
Simple console application using SQLite to track a habit and the amount of said habit done per day. 
All requirements and challenges met.

## 📋 Requirements:
- [X] This is an application where you’ll register one habit.
- [X] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day).
- [X] The application should store and retrieve data from a real database.
- [X] When the application starts, it should create a sqlite database, if one isn’t present.
- [X] It should also create a table in the database, where the habit will be logged.
- [X] The app should show the user a menu of options.
- [X] The users should be able to insert, delete, update and view their logged habit.
- [X] You should handle all possible errors so that the application never crashes.
- [X] The application should only be terminated when the user inserts 0.
- [X] You can only interact with the database using raw SQL. You can’t use mappers such as Entity Framework.

## 🔒 Extra Challenges:
- [X] Let the users create their own habits to track. That will require that you let them choose the unit of measurement of each habit.
- [X] Create a report functionality where the users can view specific information (i.e. how many times the user ran in a year? how many kms?) SQL allows you to ask very interesting things from your database.

## ✨ Features:
The user can choose a specific habit and the unit of measurement used for said habit.
- ![habit1](https://github.com/Squing0/HabitTrackerCSharp/assets/119138371/455eabf4-7115-4dc3-9ba5-600767dd642c)

User has multiple menu options to add, delete, update entries.
- ![Menu](https://github.com/Squing0/HabitTrackerCSharp/assets/119138371/fe6b84e3-f67d-4f89-97e8-9332e9f0c2b7)

They can also view all recorded entries.
- ![hh](https://github.com/Squing0/HabitTrackerCSharp/assets/119138371/5b8ecbbe-983a-46a9-9884-5684563563a6)

They can also get a report of entries for a specific year.
- ![aver](https://github.com/Squing0/HabitTrackerCSharp/assets/119138371/e811e602-a8bf-435b-a3e2-7fb75bc1099c)
