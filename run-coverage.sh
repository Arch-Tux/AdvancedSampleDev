#!/bin/bash

echo "🧪 Exécution des tests avec couverture de code..."
echo ""

# Couleurs
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

cd "$(dirname "$0")"

# Nettoyer les anciens rapports
rm -rf TestResults/ coveragereport/ 2>/dev/null

# ========================================
# Tests Unitaires
# ========================================
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}  Tests Unitaires (Domain + Application)${NC}"
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

echo -e "${YELLOW}📊 Exécution des tests unitaires...${NC}"
dotnet test --filter "FullyQualifiedName~Domain|FullyQualifiedName~Application.ProductServiceTests|FullyQualifiedName~Application.SupplierServiceTests" \
    --settings coverlet.runsettings \
    --collect:"XPlat Code Coverage" \
    --results-directory:"TestResults/Unit" \
    --verbosity quiet

if [ $? -ne 0 ]; then
    echo "❌ Les tests unitaires ont échoué"
    exit 1
fi

echo -e "${GREEN}✅ Tests unitaires réussis !${NC}"
echo ""

# ========================================
# Tests de Composants (Intégration)
# ========================================
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}  Tests de Composants (Integration)${NC}"
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

echo -e "${YELLOW}📊 Exécution des tests de composants...${NC}"
dotnet test --filter "FullyQualifiedName~Integration" \
    --settings coverlet.runsettings \
    --collect:"XPlat Code Coverage" \
    --results-directory:"TestResults/Integration" \
    --verbosity quiet

if [ $? -ne 0 ]; then
    echo "❌ Les tests de composants ont échoué"
    exit 1
fi

echo -e "${GREEN}✅ Tests de composants réussis !${NC}"
echo ""

# ========================================
# Génération des rapports
# ========================================
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo -e "${BLUE}  Génération des rapports de couverture${NC}"
echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
echo ""

# Vérifier si reportgenerator est installé
if ! command -v reportgenerator &> /dev/null; then
    echo -e "${YELLOW}⚠️  reportgenerator n'est pas installé${NC}"
    echo "Installation de reportgenerator..."
    dotnet tool install --global dotnet-reportgenerator-globaltool
    echo ""
fi

# Générer le rapport HTML combiné
echo -e "${YELLOW}📈 Génération du rapport HTML combiné...${NC}"
reportgenerator \
    -reports:"TestResults/**/coverage.cobertura.xml" \
    -targetdir:"coveragereport" \
    -reporttypes:"Html;TextSummary;Badges" \
    -classfilters:"-*.Tests.*"

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✅ Rapport de couverture généré !${NC}"
    echo ""
    echo "📂 Rapports disponibles :"
    echo "   - HTML     : coveragereport/index.html"
    echo "   - Résumé   : coveragereport/Summary.txt"
    echo "   - Badges   : coveragereport/badge_*.svg"
    echo ""
    
    # Afficher le résumé
    if [ -f "coveragereport/Summary.txt" ]; then
        echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
        echo -e "${BLUE}  📊 Résumé de la couverture${NC}"
        echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
        echo ""
        cat coveragereport/Summary.txt
        echo ""
    fi
    
    # Statistiques par catégorie
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${BLUE}  📈 Statistiques${NC}"
    echo -e "${BLUE}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo ""
    
    UNIT_TESTS=$(find TestResults/Unit -name "*.xml" | wc -l | xargs)
    INTEGRATION_TESTS=$(find TestResults/Integration -name "*.xml" | wc -l | xargs)
    
    echo -e "  ${GREEN}✓${NC} Tests unitaires      : ${UNIT_TESTS} assemblies"
    echo -e "  ${GREEN}✓${NC} Tests de composants  : ${INTEGRATION_TESTS} assemblies"
    echo ""
    
    # Ouvrir le rapport dans le navigateur (macOS)
    if [[ "$OSTYPE" == "darwin"* ]]; then
        echo "🌐 Ouverture du rapport dans le navigateur..."
        open coveragereport/index.html
    else
        echo "Pour voir le rapport, ouvrez : coveragereport/index.html"
    fi
    
    echo ""
    echo -e "${GREEN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
    echo -e "${GREEN}  ✅ Tous les tests sont passés !${NC}"
    echo -e "${GREEN}━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━${NC}"
else
    echo "❌ Échec de la génération du rapport"
    exit 1
fi
