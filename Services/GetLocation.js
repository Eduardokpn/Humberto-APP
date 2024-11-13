let watchId = null;

function startWaching() {
    if (navigator.geolocation) {
        watchId = navigator.geolocation.watchPosition(updatePosition, handleError, {
            enableHighAccuracy: true,
            maximumAge: 1000,
            timeout: 5000 //5000
        });
        // Define um intervalo para atualizar a localização a cada 10 segundos
        alert("Monitoramento iniciado.");
    } else {
        alert("Geolocalização não é suportada neste navegador.");
    }
}

function stopWatching() {
    if (watchId !== null) {
        navigator.geolocation.clearWatch(watchId);
        watchId = null;
        alert("Monitoramento parado.");
        console.log("Monitoramento Encerrado");
    }
}

function updatePosition(position) {
    const latitude = position.coords.latitude;
    const longitude = position.coords.longitude;

    // Atualiza a latitude e longitude nas tags <p> em tempo real
    document.getElementById("latitudeDisplay").textContent = latitude;
    document.getElementById("longitudeDisplay").textContent = longitude;

    // Envia os dados para o backend (opcional, só se necessário)
    fetch('/api/Location/getCurrentLocation', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ latitude, longitude })
    })
        .then(response => response.json())
        .then(data => {
            console.log("Localização enviada para o servidor: ", data.message);
        })
        .catch(error => console.error('Erro:', error));
}

function handleError(error) {
    console.error("Erro ao obter a localização:", error);
}