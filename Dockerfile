#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Site/PresentationLayer/OBForumPostAPI/OBForumPostAPI.csproj", "Site/PresentationLayer/OBForumPostAPI/"]
COPY ["InfrastructureLayer/Persistence/OBForumPost.Persistence/OBForumPost.Persistence.csproj", "InfrastructureLayer/Persistence/OBForumPost.Persistence/"]
COPY ["DomainLayer/OBForum.Domain/OBForumPost.Domain.csproj", "DomainLayer/OBForum.Domain/"]
COPY ["Site/ApplicationLayer/OBForm.Application/OBFormPost.Application.csproj", "Site/ApplicationLayer/OBForm.Application/"]
RUN dotnet restore "Site/PresentationLayer/OBForumPostAPI/OBForumPostAPI.csproj"
COPY . .
WORKDIR "/src/Site/PresentationLayer/OBForumPostAPI"
RUN dotnet build "OBForumPostAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OBForumPostAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OBForumPostAPI.dll"]