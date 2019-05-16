# CookBook Application

This is a simple web application for storing and sharing cooking recipes. The main goal of this app is mostly self learning.

### Features (In Progress)
- Registration, login, public profile management
- Adding recipe, viewing all recipes (with sorting)
- User can view recipe details, add it to favourite, upvote/downvote it, add comments
- Users can follow other users
- Main feed where followed users activity will be displayed
- Basic admin control panel
- Generating PDF from recipe for printing
- Generate custom shopping list for e.g the week
- Possibly add notification if an item from shopping list is on sale in some store nearby
- Suggest recipes based on time, weather etc.
- Maybe adding videos

More details: https://github.com/users/wmanka/projects/2

### Tech
 - ASP.NET Core version 3.0 (Preview 3)
 - Visual Studio 2019
 - Entity Framework Core

### Installation & Development  [![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/dwyl/esta/issues)
1. Fork/clone this repository
2. Open project using Visual Studio
3. Restore database using EF migrations
    Open NuGet package manager console:
    ```sh
    $ Update-Database
    ```
3. Checkout to new branch, add your code, add migration, push and create pull request.
