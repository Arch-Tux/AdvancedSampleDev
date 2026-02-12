#!/bin/bash

echo "🧪 Exécution des tests avec couverture de code..."
echo ""

# Couleurs
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

cd "$(dirname "$0")"

# Exécuter les tests avec couverture
echo -e "${YELLOW}📊 Génération de la couverture de code...${NC}"
dotnet test --settings coverlet.runsettings --collect:"XPlat Code Coverage" --verbosity quiet

if [ $? -ne 0 ]; then
    echo "❌ Les tests ont échoué"
    exit 1
fi

echo ""
echo -e "${GREEN}✅ Tests réussis !${NC}"
echo ""

# Vérifier si reportgenerator est installé
if ! command -v reportgenerator &> /dev/null; then
    echo -e "${YELLOW}⚠️  reportgenerator n'est pas installé${NC}"
    echo "Installation de reportgenerator..."
    dotnet tool install --global dotnet-reportgenerator-globaltool
    echo ""
fi

# Générer le rapport HTML
echo -e "${YELLOW}📈 Génération du rapport HTML...${NC}"
reportgenerator \
    -reports:"**/coverage.cobertura.xml" \
    -targetdir:"coveragereport" \
    -reporttypes:"Html;TextSummary"

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✅ Rapport de couverture généré !${NC}"
    echo ""
    echo "📂 Rapport disponible dans : coveragereport/index.html"
    echo ""
    
    # Afficher le résumé
    if [ -f "coveragereport/Summary.txt" ]; then
        echo "📊 Résumé de la couverture :"
        echo ""
        cat coveragereport/Summary.txt
        echo ""
    fi
    
    # Ouvrir le rapport dans le navigateur (macOS)
    if [[ "$OSTYPE" == "darwin"* ]]; then
        echo "🌐 Ouverture du rapport dans le navigateur..."
        open coveragereport/index.html
    else
        echo "Pour voir le rapport, ouvrez : coveragereport/index.html"
    fi
else
    echo "❌ Échec de la génération du rapport"
    exit 1
fi
