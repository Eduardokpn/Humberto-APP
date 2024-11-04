async function buscar() {
    const localizacao = await startWatching()
    if (!localizacao) {
        console.error("Não foi possivel buscar a localização");
        
    } 
    
    const endereco = document.getElementById("localFinal").value

    const projectBBaseUrl = 'https://localhost:44384'; // Substitua pela URL que você configurou no appsettings.json

    const response = await fetch(`${projectBBaseUrl}/BaseRoutes/iniciarBusca`, {
        method: 'Post',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({endereco})
});
    if (response.ok) 
    {
    const rotas = response.json();
    console.log("rotas recebidas: ", rotas);
    }
    else
    {
        console.error("erro ao iniciar a busca", response.statusText);
    }
     
    
}


let watchId = null;

function startWatching() {
    if (navigator.geolocation) {
        watchId = navigator.geolocation.watchPosition(updatePosition, handleError, {
            enableHighAccuracy: true,
            maximumAge: 1000,
            timeout: 5000 //5000
        });
        // Define um intervalo para atualizar a localização a cada 10 segundos
        alert("Monitoramento iniciado.");
        return null
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
    const projectBBaseUrl = 'https://localhost:44384'; //'http://humbertoapi.somee.com/HumbertoApii'; // Substitua pela URL que você configurou no appsettings.json

    fetch(`${projectBBaseUrl}/api/Location/getCurrentLocation`, {

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
