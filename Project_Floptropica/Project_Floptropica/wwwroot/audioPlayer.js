window.currentAudio = null;

window.playAudioWithCallback = (fileName, dotNetHelper) => {
    if (window.currentAudio) {
        window.currentAudio.pause();
    }

    window.currentAudio = new Audio(`sounds/${fileName}`);
    window.currentAudio.play();

    window.currentAudio.onended = () => {
        dotNetHelper.invokeMethodAsync("OnAudioEnded");
        window.currentAudio = null;
    };
};

window.toggleAudio = () => {
    if (window.currentAudio) {
        if (window.currentAudio.paused) {
            window.currentAudio.play();
        } else {
            window.currentAudio.pause();
        }
    }
};

window.getSoundCategories = async () => {
    const response = await fetch("sounds.json");
    const data = await response.json();
    return Object.keys(data.Categories);
};

window.getSoundsInCategory = async (category) => {
    const response = await fetch("sounds.json");
    const data = await response.json();
    return data.Categories[category] || [];
};