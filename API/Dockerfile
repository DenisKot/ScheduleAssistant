#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ScheduleAssistant.Presentation/ScheduleAssistant.Presentation.csproj", "ScheduleAssistant.Presentation/"]
RUN dotnet restore "ScheduleAssistant.Presentation/ScheduleAssistant.Presentation.csproj"
COPY . .
WORKDIR "/src/ScheduleAssistant.Presentation"
RUN dotnet build "ScheduleAssistant.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScheduleAssistant.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScheduleAssistant.Presentation.dll"]