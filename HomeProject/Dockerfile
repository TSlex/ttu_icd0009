FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /app
EXPOSE 80

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

COPY BLL.App/*.csproj ./BLL.App/
COPY BLL.App.DTO/*.csproj ./BLL.App.DTO/

COPY Contracts.BLL.App/*.csproj ./Contracts.BLL.App/
COPY Contracts.DAL.App/*.csproj ./Contracts.DAL.App/

COPY DAL.App.DTO/*.csproj ./DAL.App.DTO/
COPY DAL.App.EF/*.csproj ./DAL.App.EF/

COPY Domain/*.csproj ./Domain/

COPY Extension/*.csproj ./Extension/
COPY PublicApi.DTO.v1/*.csproj ./PublicApi.DTO.v1/
COPY Resourses/*.csproj ./Resourses/

COPY WebApp/*.csproj ./WebApp/

#======================================================================
RUN dotnet restore

# copy everything else and build app
COPY BLL.App/. ./BLL.App/
COPY BLL.App.DTO/. ./BLL.App.DTO/

COPY Contracts.BLL.App/. ./Contracts.BLL.App/
COPY Contracts.DAL.App/. ./Contracts.DAL.App/

COPY DAL.App.DTO/. ./DAL.App.DTO/
COPY DAL.App.EF/. ./DAL.App.EF/

COPY Domain/. ./Domain/

COPY Extension/. ./Extension/
COPY PublicApi.DTO.v1/. ./PublicApi.DTO.v1/
COPY Resourses/. ./Resourses/

COPY WebApp/. ./WebApp/

WORKDIR /app/WebApp

#======================================================================
RUN dotnet publish -c Release -o out

#======================================================================
FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS runtime
WORKDIR /app
EXPOSE 80

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
     
# change timezone
RUN echo "Europe/Tallinn" > /etc/timezone
RUN dpkg-reconfigure -f noninteractive tzdata

#enviroments
ENV ConnectionStrings:MySqlConnection="server=80.66.248.96;port=3306;database=asp_project_3;user=aleksi_asp2;password=AdolfHitler"

ENV Rank:GiftSendExperience="10"
ENV Rank:CommentExperience="2"

COPY --from=build /app/WebApp/out ./

ENTRYPOINT ["dotnet", "WebApp.dll"]
