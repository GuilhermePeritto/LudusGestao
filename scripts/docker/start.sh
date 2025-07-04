#!/bin/bash

echo "🚀 Iniciando Ludus Gestão com Docker..."

# Verificar se o Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker e tente novamente."
    exit 1
fi

# Navegar para o diretório docker
cd "$(dirname "$0")/../../docker"

# Parar containers existentes
echo "🛑 Parando containers existentes..."
docker-compose down

# Construir e iniciar os containers
echo "🔨 Construindo e iniciando containers..."
docker-compose up --build -d

# Aguardar um pouco para os serviços iniciarem
echo "⏳ Aguardando serviços iniciarem..."
sleep 10

# Verificar status dos containers
echo "📊 Status dos containers:"
docker-compose ps

echo ""
echo "✅ Ludus Gestão iniciado com sucesso!"
echo "🌐 API disponível em: http://localhost:5000"
echo "🗄️  Banco de dados: localhost:5432"
echo ""
echo "Para ver os logs: docker-compose logs -f"
echo "Para parar: docker-compose down" 