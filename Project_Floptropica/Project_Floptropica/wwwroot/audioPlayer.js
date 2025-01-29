window.playAudio = (fileName) => {
    let audio = new Audio(`sounds/${fileName}`);
    audio.play();
};

window.getSoundFiles = async () => {
    const response = await fetch("sounds.json");
    const data = await response.json();
    return data.files;
};