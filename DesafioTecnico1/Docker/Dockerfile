FROM postgres:16

ENV POSTGRES_USER=user
ENV POSTGRES_PASSWORD=password@123
ENV POSTGRES_DB=desafio

EXPOSE 5432
#docker network create minha_rede
#docker run --rm -d -p 5432:5432 --name postgres_container --network minha_rede meu_postgres 
#docker run --rm -d -p 80:80 --name minha_api joaovsmathias/desafio-tecnico-api:latest
#docker run --rm -d -p 8080:8080  -e ASPNETCORE_URLS="http://+:8080" -e ConnectionStrings__DesafioTecnicoContext="Host=postgres_container;Database=desafio;Username=user;Password=password@123" --name minha_api --network minha_rede joaovsmathias/desafio-tecnico-api:latest

