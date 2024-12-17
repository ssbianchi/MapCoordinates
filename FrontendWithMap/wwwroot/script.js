async function initMap() {
    try {
        // Exibe o carregamento
        const loadingElement = document.getElementById("loading");
        loadingElement.style.display = "flex";

        // Buscar os dados da API
        const response = await fetch("http://localhost:5296/api/Coordinates/from-excel");
        const locations = await response.json();

        // Verificar se existem coordenadas
        if (!locations || locations.length === 0) {
            console.error("Nenhuma coordenada encontrada.");
            return;
        }

        // Configurar o mapa inicial centrado na primeira coordenada
        const map = new google.maps.Map(document.getElementById("map"), {
            zoom: 12,
            center: { lat: locations[0].latitude, lng: locations[0].longitude }
        });

        // Adicionar marcadores ao mapa usando AdvancedMarkerElement
        locations.forEach(location => {
            // Verificar se as coordenadas são válidas
            if (location.latitude && location.longitude) {
                new google.maps.marker.AdvancedMarkerElement({
                    position: { lat: location.latitude, lng: location.longitude },
                    map: map,
                    title: location.endereco
                });
            }
        });

    } catch (error) {
        console.error("Erro ao carregar os dados do mapa: ", error);
    } finally {
        // Oculta o carregamento
        document.getElementById("loading").style.display = "none";
    }

}

// Chama a função initMap assim que o documento estiver carregado
document.addEventListener("DOMContentLoaded", initMap);
