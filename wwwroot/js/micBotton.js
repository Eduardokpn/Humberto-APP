 // Verifica se o navegador suporta a Web Speech API
const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
if (!SpeechRecognition) {
    alert('Seu navegador não suporta reconhecimento de voz. Tente usar o Google Chrome.');
} else {
    const recognition = new SpeechRecognition();
    recognition.lang = 'pt-BR'; // Define o idioma para português do Brasil
    recognition.interimResults = false; // Resultados finais apenas

    const inputField = document.getElementById('localFinal');
    const startBtn = document.getElementById('startBtn');

    startBtn.addEventListener('click', () => {
        recognition.start(); // Inicia o reconhecimento de voz
        startBtn.disabled = true;
        
    });

    recognition.onresult = (event) => {
        const transcript = event.results[0][0].transcript; // Transcrição do áudio
        localFinal.value = transcript; // Preenche o input com o texto
        console.log('Texto reconhecido:', transcript);
    };

    recognition.onerror = (event) => {
        console.error('Erro no reconhecimento:', event.error);
        alert('Erro ao reconhecer áudio: ' + event.error);
    };

    recognition.onend = () => {
        startBtn.disabled = false;
        console.log('Reconhecimento de voz finalizado.');
    };
}