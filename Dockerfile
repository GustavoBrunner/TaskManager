#download da sdk do dotnet para o ambiente de desenvolvimento
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env 

#comando para que, dentro desse ambiente virtual, os arquivos sejam alocados em alguma pasta. A partir do momento que esse comando é chamado, todos os outros serão executados dentro dessa pasta
WORKDIR /app

#Comando de cópia. O primeiro ponto indica que, na pasta onde o dockerfile se encontra, todo o conteúdo será copiado. O segundo ponto indica a pasta do ambiente virtual para onde esse conteúdo será colado. Seria o mesmo que fazer COPY . /app. Indica a raíz do ambiente virtual
COPY . .

#Faz com que o comando dotnet restore seja executado. Ele é responsável por baixar todas as dependências do projeto, no ambiente virtual
RUN dotnet restore

#comando que serve para publicar o app. Passamos para ele a configuração, que será modo de release, e para qual pasta ele será colocado, após o release, no caso a pasta out.
RUN dotnet publish -c release -o out

#Podemos criar pastas no projeto também, usando o comando RUN mkdir ./nome_pasta

#Agora, com o primeiro ambiente resolvido, passamos para o próximo ambiente, onde não precisamos mais necessariamente do sdk, e sim do aspnet
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /runtime-app

#Aqui fazemos a cópia de todos os arquivos do ambiente de build organizados anteriormente, para o ambiente de runtime, passando junto do nome do ambiente, o diretório de pastas que vai ser copiado, e o local para onde os arquivos serão colados
COPY --from=build-env /app/out . 

#Qual a porta do container que ficará exposta. A portá que será usada para as requisições
EXPOSE 8080

#comando para dar o start na aplicação. Como copiamos os arquivos de Out que foi buildado no primeiro ambiente, o arquivo dll com o nome do projeto foi gerado. Esse é o arquivo que será startado
ENTRYPOINT [ "dotnet", "TaskManager.dll" ]







