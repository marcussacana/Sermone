var userLang = (navigator.language || navigator.userLanguage).split('-')[0].toLowerCase();
switch (userLang) {
    case 'pt':
        LoadTitle.innerText = "Carregando...";
        LoadDesc.innerText = "A primeira vez pode levar um tempo.";
        ErrorMessage.innerText = "Um erro inesperado ocorreu.";
        ReloadLink.innerText = "Recarregar";
        //Dismiss.innerText = "❌";
        break;
    case 'en':
        LoadTitle.innerText = "Loading...";
        LoadDesc.innerText = "The first time may take some time.";
        ErrorMessage.innerText = "An unexpected error has occurred.";
        ReloadLink.innerText = "Reload";
        break;
}