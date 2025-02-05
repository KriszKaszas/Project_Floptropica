window.currentAudio = null;
window.lastPausedTime = 0; // Store paused time

window.playAudioWithCallback = (fileName, dotNetHelper) => {
    if (window.currentAudio) {
        window.currentAudio.pause();
        window.currentAudio = null;
    }

    window.currentAudio = new Audio(`sounds/${fileName}`);
    window.currentAudio.play();
    window.lastPausedTime = 0; // Reset pause time

    window.currentAudio.onended = () => {
        dotNetHelper.invokeMethodAsync("OnAudioEnded");
        window.currentAudio = null;
        window.lastPausedTime = 0;
    };
};

window.toggleAudio = () => {
    if (window.currentAudio) {
        if (window.currentAudio.paused) {
            window.currentAudio.play();
        } else {
            window.lastPausedTime = window.currentAudio.currentTime;
            window.currentAudio.pause();
        }
    }
};

// Resume audio from where it was paused
window.resumeAudio = () => {
    if (window.currentAudio && window.currentAudio.paused) {
        window.currentAudio.play();
    }
};

// Restart audio from the beginning
window.restartAudio = () => {
    if (window.currentAudio) {
        window.currentAudio.currentTime = 0;
        window.currentAudio.play();
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