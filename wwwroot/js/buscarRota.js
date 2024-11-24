async function buscar() { 
    // Recebe destino
    const endereco = document.getElementById("localFinal").value;

    const response = await fetch(`/Main/Base/iniciarBusca`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 
                    "Access-Control-Allow-Origin": "*",   
        },
        body: JSON.stringify({ endereco })
    });

    if (response.ok) {
        const rotas = await response.json();
        console.log("rotas recebidas: ", rotas.devMessage);
        window.location.href = '/Route/Route/ShowRoutes';
    } else {
        const errorMessage = await response.json(); 
        alert(errorMessage.userMessage);  
        console.error(`Ocorreu um Erro \n ${errorMessage.devMessage}`);
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
        return null
    } else {
        alert("Opa! tivemos um problema com usa localização, Verifique se o GPS está ligado?");
    }
}

function stopWatching() {
    if (watchId !== null) {
        navigator.geolocation.clearWatch(watchId);
        watchId = null;
        alert("Gps desligado!");
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
    alert("Permita o acesso a localização para usar o App")
}

window.addEventListener("load", async ( event) => {
    
    startWatching()
    
    
    const searchParams = new URLSearchParams(window.location.search);
    
    // valida se o usuario foi redirecionado de outra guia para a hone
    if (searchParams.has('redirected') === true) {
        
        const response = await fetch(`/Main/Base/redirected`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
        })
        const retornoResponse = await response.json();
        await alert(retornoResponse.userMessage);
        console.error(`Ocorreu um Erro \n ${retornoResponse.devMessage}`);
        window.location.href = '/home';
    }

    
    
});