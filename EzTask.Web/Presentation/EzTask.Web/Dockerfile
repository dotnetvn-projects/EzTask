#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-1903 AS build
WORKDIR /src
COPY ["Presentation/EzTask.Web/EzTask.Web.csproj", "Presentation/EzTask.Web/"]
COPY ["Presentation/EzTask.Web.Framework/EzTask.Web.Framework.csproj", "Presentation/EzTask.Web.Framework/"]
COPY ["Libraries/EzTask.Framework/EzTask.Framework.csproj", "Libraries/EzTask.Framework/"]
COPY ["Libraries/EzTask.DataAccess/EzTask.DataAccess.csproj", "Libraries/EzTask.DataAccess/"]
COPY ["Libraries/EzTask.Entity/EzTask.Entity.csproj", "Libraries/EzTask.Entity/"]
COPY ["Libraries/EzTask.Repository/EzTask.Repository.csproj", "Libraries/EzTask.Repository/"]
COPY ["Libraries/EzTask.Interface/EzTask.Interface.csproj", "Libraries/EzTask.Interface/"]
COPY ["Business/EzTask.Business/EzTask.Business.csproj", "Business/EzTask.Business/"]
COPY ["Libraries/EzTask.Model/EzTask.Model.csproj", "Libraries/EzTask.Model/"]
COPY ["Plugins/EzTask.Plugin.MessageService/EzTask.Plugin.MessageService.csproj", "Plugins/EzTask.Plugin.MessageService/"]
COPY ["Libraries/EzTask.Log/EzTask.Log.csproj", "Libraries/EzTask.Log/"]
COPY ["Modules/EzTask.Modules.Task/EzTask.Modules.Task.csproj", "Modules/EzTask.Modules.Task/"]
COPY ["Modules/EzTask.Modules.Core/EzTask.Modules.Core.csproj", "Modules/EzTask.Modules.Core/"]
COPY ["Modules/EzTask.Modules.UserProfile/EzTask.Modules.UserProfile.csproj", "Modules/EzTask.Modules.UserProfile/"]
COPY ["Modules/EzTask.Modules.Project/EzTask.Modules.Project.csproj", "Modules/EzTask.Modules.Project/"]
COPY ["Modules/EzTask.Modules.Dashboard/EzTask.Modules.Dashboard.csproj", "Modules/EzTask.Modules.Dashboard/"]
COPY ["Modules/EzTask.Modules.Authentication/EzTask.Modules.Authentication.csproj", "Modules/EzTask.Modules.Authentication/"]
RUN dotnet restore "Presentation/EzTask.Web/EzTask.Web.csproj"
COPY . .
WORKDIR "/src/Presentation/EzTask.Web"
RUN dotnet build "EzTask.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EzTask.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EzTask.Web.dll"]