set "root=%cd%"
start cmd /K "cd CatalogService/CatalogService/ && dotnet run"
start cmd /K "cd EventService/EventService/ && dotnet run"
start cmd /K "cd MusicService/MusicService/ && dotnet run"
start cmd /K "cd UserService/UserService/ && dotnet run"
start cmd /K "cd RecommenderService/RecommenderService/ && dotnet run"
start cmd /K "cd MusicSocialNetwork-WebApp/WebApi && dotnet run"
start cmd /K "cd musicsocialnetwork-client && yarn && yarn start"