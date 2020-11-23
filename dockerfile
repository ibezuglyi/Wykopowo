FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
RUN \
    mkdir ./app && \
    cd ./app && \
    git clone https://github.com/ibezuglyi/Wykopowo

WORKDIR /app/Wykopowo/
RUN dotnet restore
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Wykopowo/out .
RUN chmod +x  Wykopowo
ENTRYPOINT ["./Wykopowo"]