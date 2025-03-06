# Используем базовый образ Ubuntu с графической средой (Xfce) и VNC
FROM dorowu/ubuntu-desktop-lxde-vnc:latest

EXPOSE 6080 

# Устанавливаем .NET SDK
RUN apt-get update && \
    apt-get install -y wget && \
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0

# Копируем исходный код вашего Avalonia-приложения
WORKDIR /app
COPY . .

# Восстанавливаем зависимости и собираем проект
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Устанавливаем графические зависимости для Avalonia
RUN apt-get install -y libgtk-3-0 libglib2.0-0 libc6 libx11-6 libxext6 libxrandr2 libxcomposite1 libxcursor1 libxi6 libxtst6 libxrender1 libfontconfig1 libasound2

# Запуск VNC-сервера и приложения
CMD ["bash", "-c", "vncserver :1 -geometry 1280x800 -depth 24 && cd /app/publish && dotnet NTFSChecker.Avalonia.dll"]