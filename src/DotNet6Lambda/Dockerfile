FROM public.ecr.aws/h8k8c9u2/lambda-dotnet6:latest AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-amd64 as build

WORKDIR "/src/DotNet6Lambda"
COPY . .
RUN dotnet restore "DotNet6Lambda.csproj"
RUN dotnet build "DotNet6Lambda.csproj" --configuration Release --output /app/build

FROM build AS publish
RUN dotnet publish "DotNet6Lambda.csproj" \
            --configuration Release \ 
            --runtime linux-x64 \
            --self-contained false \ 
            --output /app/publish \
            -p:PublishReadyToRun=true  

FROM base AS final
WORKDIR /var/task
COPY --from=publish /app/publish .

CMD [ "DotNet6Lambda::DotNet6Lambda.Function::FunctionHandler" ]