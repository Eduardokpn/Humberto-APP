async function buscar() {
    //Recebe a localização atual.
    const localizacao = await startWatching()
    if (!localizacao) {
        console.error("Não foi possivel buscar a localização");
        
    } 
    
    // Recebe destino
    const endereco = document.getElementById("localFinal").value

    // projectBBaseUrl = 'http://humbertoapi.somee.com/HumbertoApii' //'https://localhost:44384'; // Substitua pela URL que você configurou no appsettings.json

    const response = await fetch(`/Main/Base/iniciarBusca`, {
        method: 'Post',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify({endereco})
});
    if (response.ok) 
    {
    const rotas = response.json();
    console.log("rotas recebidas: ", rotas);
    window.location.href='/Route/Route/ShowRoutes';
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
            maximumAge: 100000,
            timeout: 500000 //5000
        });
        // Define um intervalo para atualizar a localização a cada 5 segundos
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
    //const projectBBaseUrl = 'http://humbertoapi.somee.com/HumbertoApii'; // Substitua pela URL que você configurou no appsettings.json

    
    fetch(`/Location/geoLocation/getCurrentLocation `, {

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
