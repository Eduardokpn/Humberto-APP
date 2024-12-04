let instructionDoc = document.getElementById("instrucao");
let maneuverDoc = document.getElementById("manobra");
let departureTimeDoc = document.getElementById("departureTime");
let desembarqueDoc = document.getElementById("desembarque");
let embarqueLocalDoc = document.getElementById("embarqueLocal");
let linhaDoc = document.getElementById("linha");
let map, userPosition, stepEnd;
let currentDate = new Date();
let lastExecutionTime = new Date();
const userImg = document.createElement("img");
userImg.src = "https://uxwing.com/wp-content/themes/uxwing/download/location-travel-map/location-pin-icon.png";
async function initMap(userLocation = null, stepEndLocation = null) {
    
    if(!map) {
        // livrarias
        const { Map } = await google.maps.importLibrary("maps");
        const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");
        userImg.width = 25;

        // centraliza o mapa
        map = new Map(document.getElementById("map"), {
            zoom: 18,
            center: userLocation,
            mapId: "DEMO_MAP_ID",
        });

         userPosition = new AdvancedMarkerElement({
            map: map,
            position: userLocation,
            title: "User Location",
            content: userImg,
        });

        // The stepEnd, positioned at Uluru
         stepEnd = new AdvancedMarkerElement({
            map: map,
            position: stepEndLocation,
            title: "End Step Location",
        });
    } else {
        map.setCenter(userLocation);
        if (userPosition) {
            userPosition.map = null;
        }
        userPosition = new google.maps.marker.AdvancedMarkerElement({
            map: map,
            position: userLocation,
            title: "User Location",
            content: userImg,
        });

        if (stepEnd) {
            stepEnd.map = null; // Remover o marcador anterior
        }
        stepEnd = new google.maps.marker.AdvancedMarkerElement({
            map: map,
            position: stepEndLocation,
            title: "End Step Location",
        });
    }
    
}

function calcularDistancia(loc1, loc2) {
    const R = 6371e3; // Raio da Terra em metros
    const φ1 = loc1.lat * Math.PI / 180;
    const φ2 = loc2.lat * Math.PI / 180;
    const Δφ = (loc2.lat - loc1.lat) * Math.PI / 180;
    const Δλ = (loc2.lng - loc1.lng) * Math.PI / 180;

    const a = Math.sin(Δφ / 2) * Math.sin(Δφ / 2) +
        Math.cos(φ1) * Math.cos(φ2) *
        Math.sin(Δλ / 2) * Math.sin(Δλ / 2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    return R * c; // Distância em metros
}

function getCrParam() {
    let searchParams = new URLSearchParams(window.location.search)
    let Cr = searchParams.get('Cr')

    return Cr
}

function innerDocLeg(instruction, maneuver) {
    instructionDoc.innerText = instruction;
    maneuverDoc.innerText = maneuver;
} 

function innerDocTransit(departureTime,desembarque,embarqueLocal,linha) {
    departureTimeDoc.innerText = departureTime
    desembarqueDoc.innerText = desembarque
    embarqueLocalDoc.innerText = embarqueLocal
    linhaDoc.innerText = linha
}
async function iniciarNavegacao(steps) {

    
    
    if (navigator.geolocation) {
        navigator.geolocation.watchPosition(await updateLocation,await handleError, {
            timeout: 3000,
            enableHighAccuracy: true,
            maximumAge: 0
        });
        
        /*setInterval(() => {
            navigator.geolocation.getCurrentPosition(updateLocation, handleError, {
                enableHighAccuracy: true,
                maximumAge: 0,
                timeout: 3000,
            });
        }, 2000);*/
        /* 
        * Este atributo comentado é apenas para caso watchPosition não esteja atualizando corretamente
        */
    } else {
        alert("Geolocalização não suportada pelo navegador.");
    }
    let stepIndex = 0; // Inicia no primeiro passo

    async function updateLocation(position) {
        const currentExecutionTime = Date.now();
        const { latitude, longitude } = position.coords;
        const userLocation = { lat: latitude, lng: longitude };
        const stepEndLocation = steps[stepIndex].endLocation.latLng;
        
        console.time("Ultima Atualização")
        console.log("Posição atual atualizada com sucesso: \n" +
                        userLocation.lat + " " + userLocation.lng + "\n" +
                        currentDate.getHours() + ":" + currentDate.getMinutes() + ":" + currentDate.getSeconds()  + "\n"                                   
        )
        if (lastExecutionTime !== null) {
            const interval = currentExecutionTime - lastExecutionTime; // Calcular o intervalo
            console.log(`Intervalo entre atualizações: ${interval} ms`);
        } else {
            console.log("Primeira execução.");
        }
        
        lastExecutionTime = currentExecutionTime;
        await initMap(userLocation, stepEndLocation)
        const distancia = calcularDistancia(userLocation, stepEndLocation);
        console.log(`stepIndex: ${stepIndex}` )
        console.log(`Distancia: ${distancia.toFixed(1)}` )
        
        // TODO: APÓS CADA ETAPA APRESENTAR NA TELA UM ICONE REFERENTE A CADA maneuver, e informações relevantes como distancia,
        //  também será necessario ajustar o problema de só atualizar a posição atual quando a da alt + tab.
        
        if (stepIndex == 0) {
            //alert(`Iniciando a viagem: ${steps[stepIndex].navigationInstruction.instructions}`)
            innerDocLeg(steps[stepIndex].navigationInstruction.instructions, steps[stepIndex].navigationInstruction.maneuver)
            console.log(`Iniciando a viagem: ${steps[stepIndex].navigationInstruction.instructions}\n Manobra: ${steps[stepIndex].navigationInstruction.maneuver} `)
            
        }
        if (distancia < 20.0) { // Limite de 20 metros
            //alert(`Etapa anterior concluída,: ${steps[stepIndex].navigationInstruction.instructions}`);
            console.log(`Etapa anterior concluída,: ${steps[stepIndex].navigationInstruction.instructions} \n Manobra: ${steps[stepIndex].navigationInstruction.maneuver} `);
            console.info(stepEndLocation)   
            
            
            stepIndex++;
            
            if (stepIndex < steps.length) {
                innerDocLeg(
                    steps[stepIndex].navigationInstruction.instructions,
                    steps[stepIndex].navigationInstruction.maneuver
                )
                //alert(`Próximo passo: ${steps[stepIndex].navigationInstruction.instructions} `);
                console.log(`Próximo passo: ${steps[stepIndex].navigationInstruction.instructions} \n Manobra: ${steps[stepIndex].navigationInstruction.maneuver} `);
                if (steps[stepIndex].transitDetails !== null) {
                    innerDocTransit(
                        `${steps[stepIndex].transitDetails.localizedValues.departureTime.time.text}`,
                        `${steps[stepIndex].transitDetails.stopDetails.arrivalStop.name}`,
                        `${steps[stepIndex].transitDetails.stopDetails.departureStop.name}`,
                        `${steps[stepIndex].transitDetails.transitLine.nameShort} - ${steps[stepIndex].transitDetails.transitLine.name}`
                    )
                }
                
            } else {
                document.getElementById("concluido").innerText = "VIAGEM CONCLUIDA" 
                alert("Rota concluída!");
                navigator.geolocation.clearWatch(updateLocation); // Para o monitoramento
            }
        }
    }
}
async function handleError(error) {
    console.error("Erro ao obter a localização:", error);
    alert("Permita o acesso a localização para usar o App")
}



document.addEventListener('DOMContentLoaded', async ( e ) => {
    
    let cr = getCrParam();
    await fetch(`/LoadMap/GetStepsNavigator?Cr=${cr}`, {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    })
        .then(response => response.json())
        .then(data => {
            const steps = data.legs[0].steps;
            iniciarNavegacao(steps);
        });

})


await initMap();
