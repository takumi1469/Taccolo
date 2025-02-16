let originalTextUnchanged;
let translatedTextUnchanged;
let wmpUnchanged;

// When the page loads, capture the initial texts
window.onload = function () {
    originalTextUnchanged = document.getElementById("p-original").textContent;
    translatedTextUnchanged = document.getElementById("p-translated").textContent;
    wmpUnchanged = getAllWmpsBeforeEdit();
};

function switchToEdit() {
    // Get the p element with the original text
    const pOriginal = document.getElementById("p-original");
    const originalText = pOriginal.textContent;

    const pTranslated = document.getElementById("p-translated");
    const translatedText = pTranslated.textContent;

    // Create a new textarea element
    const textareaOriginal = document.createElement("textarea");
    textareaOriginal.id = "textarea-original";
    textareaOriginal.value = originalText;

    const textareaTranslated = document.createElement("textarea");
    textareaTranslated.id = "textarea-translated";
    textareaTranslated.value = translatedText;

    // Replace the <p> element with the <textarea>
    pOriginal.replaceWith(textareaOriginal);
    pTranslated.replaceWith(textareaTranslated);

    // Make and replace buttons
    const divButtons = document.getElementById("div-buttons");
    const buttonSave = document.createElement("button");
    buttonSave.id = "button-save";
    buttonSave.textContent = "SAVE";
    buttonSave.onclick = saveLearningSet;
    const buttonCancel = document.createElement("button");
    buttonCancel.id = "button-cancel";
    buttonCancel.textContent = "CANCEL";
    buttonCancel.onclick = switchToViewByCancel;

    divButtons.innerHTML = "";
    divButtons.appendChild(buttonSave);
    divButtons.appendChild(buttonCancel);

    switchToEditWmps();
}

function saveLearningSet() {
    // Get the updated data from the textareas
    const originalText = document.getElementById("textarea-original").value;
    const translatedText = document.getElementById("textarea-translated").value;

    const id = document.getElementById("p-id").textContent;

    // Prepare the data to send
    const data = {
        Id: id,
        OriginalText: originalText,
        TranslatedText: translatedText
    };

    // Make the fetch call
    fetch(`/api/LearningSet/Update`, {

        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
        .then(response => {
            if (!response.ok) {
                /*throw new Error("Failed to save the learning set.");*/
                alert("Saving wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Learning Set updated successfully:", result);

            // Update the UI to "view mode"
            switchToViewBySave(originalText, translatedText);
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });
}

function switchToViewBySave(originalText, translatedText) {
    const pOriginal = document.createElement("p");
    pOriginal.id = "p-original";
    pOriginal.className = "p-original";
    pOriginal.textContent = originalText;

    const pTranslated = document.createElement("p");
    pTranslated.id = "p-translated";
    pTranslated.className = "p-translated";
    pTranslated.textContent = translatedText;

    const textareaOriginal = document.getElementById("textarea-original");
    const textareaTranslated = document.getElementById("textarea-translated");

    textareaOriginal.replaceWith(pOriginal);
    textareaTranslated.replaceWith(pTranslated);

    // Replace SAVE and CANCEL buttons with the EDIT button
    const divButtons = document.getElementById("div-buttons");
    divButtons.innerHTML = `
        <button class="button-edit" id="button-edit">EDIT</button>
    `;

    // Attach the event listener to the new Edit button
    const editButton = document.getElementById("button-edit");
    editButton.addEventListener("click", switchToEdit);
}

function switchToViewByCancel(originalText, translatedText) {
    const pOriginal = document.createElement("p");
    pOriginal.id = "p-original";
    pOriginal.className = "p-original";
    pOriginal.textContent = originalTextUnchanged;

    const pTranslated = document.createElement("p");
    pTranslated.id = "p-translated";
    pTranslated.className = "p-translated";
    pTranslated.textContent = translatedTextUnchanged;

    const textareaOriginal = document.getElementById("textarea-original");
    const textareaTranslated = document.getElementById("textarea-translated");

    textareaOriginal.replaceWith(pOriginal);
    textareaTranslated.replaceWith(pTranslated);

    // Replace SAVE and CANCEL buttons with the EDIT button
    const divButtons = document.getElementById("div-buttons");
    divButtons.innerHTML = `
        <button class="button-edit" id="button-edit">EDIT</button>
    `; 

    // Attach the event listener to the new Edit button
    const editButton = document.getElementById("button-edit");
    editButton.addEventListener("click", switchToEdit);
}


// <<From here down is for List of Words>>
class WordMeaningPair {
    constructor(word, translatedText, alternatives) {
        this.word = word; // string
        this.translatedText = translatedText; //string 
        this.alternatives = alternatives; //array of strings
    }
}

function switchToEditWmps() {
    document.querySelectorAll(".p-translated-text").forEach(el => {
        const input = document.createElement("input");
        input.value = el.textContent;
        input.className = "input-translated-text";
        el.replaceWith(input);
    });

    document.querySelectorAll(".li-alternative-meaning").forEach(el => {
        const input = document.createElement("input");
        input.value = el.textContent;
        input.className = "input-alternative-meaning";
        el.replaceWith(input);
    });
}

function getAllWmpsBeforeEdit() {
    const wmpContainers = document.querySelectorAll(".div-wmp"); // Select all WMP containers
    let wmpArray = [];

    wmpContainers.forEach(wmp => {
        const word = wmp.querySelector(".p-word")?.textContent || "";
        const translatedText = wmp.querySelector(".p-translated-text")?.textContent || "";

        // Extract alternatives
        const alternatives = Array.from(wmp.querySelectorAll(".li-alternative-meaning"))
            .map(p => p.textContent)
            .filter(value => value.trim() !== ""); // Remove empty values

        wmpArray.push({ word, translatedText, alternatives });
    });

    console.log(JSON.stringify(wmpArray));
    return wmpArray;
}
function getAllWmpsAfterEdit() {
    const wmpContainers = document.querySelectorAll(".div-wmp"); // Select all WMP containers
    let wmpArray = [];

    wmpContainers.forEach(wmp => {
        const word = wmp.querySelector(".input-word")?.value || "";
        const translatedText = wmp.querySelector(".input-translated-text")?.value || "";

        // Extract alternatives (assuming they are in input elements with class 'input-alternative')
        const alternatives = Array.from(wmp.querySelectorAll(".input-alternative-meaning"))
            .map(input => input.value)
            .filter(value => value.trim() !== ""); // Remove empty values

        wmpArray.push({ word, translatedText, alternatives });
    });

    return wmpArray;
}










