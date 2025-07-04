#!/bin/bash

echo "🛑 Parando Ludus Gestão..."

# Navegar para o diretório docker
cd "$(dirname "$0")/../../docker"

# Parar containers
docker-compose down

echo "✅ Containers parados com sucesso!" 