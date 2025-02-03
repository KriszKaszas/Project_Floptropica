window.playAudio = (fileName) => {
    let audio = new Audio(`sounds/${fileName}`);
    audio.play();
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
