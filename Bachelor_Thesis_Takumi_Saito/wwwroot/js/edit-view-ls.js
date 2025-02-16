let originalTextUnchanged;
let translatedTextUnchanged;

// When the page loads, capture the initial texts
window.onload = function () {
    originalTextUnchanged = document.getElementById("p-original").textContent;
    translatedTextUnchanged = document.getElementById("p-translated").textContent;
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

    switchToEditWords();
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
    pTranslated.id = "p-tramslated";
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

// Define the Meaning class

// Define the WordMeaningPair class
class WordMeaningPair {
    constructor(word, meaning) {
        this.word = word; // String
        this.meaning = meaning; // Instance of the Meaning class
    }
}

class Meaning {
    constructor(translatedText, alternatives) {
        this.translatedText = translatedText; // String
        this.alternatives = alternatives; // Array of strings
    }
}
function switchToEditWords() {
    const translatedTexts = document.getElementsByClassName("p-translated-text"); //returns HTMLCollection

    for (let i = 0; i < translatedTexts.length; i++) {
        var originalTranslatedText = translatedTexts[i].textContent;
        const inputTranslatedText = document.createElement("input");
        inputTranslatedText.value = originalTranslatedText;
        inputTranslatedText.className = "input-translated-text";
        translatedTexts[i].replaceWith(inputTranslatedText);
        
    }

}


















//// Example usage
//const pair1 = new WordMeaningPair(
//    "hello",
//    new Meaning("hola", ["saludo", "hi"])
//);

//const pair2 = new WordMeaningPair(
//    "goodbye",
//    new Meaning("adiós", ["bye", "farewell"])
//);

//// Create an array of WordMeaningPair objects
//const wordMeaningPairs = [pair1, pair2];

//console.log(wordMeaningPairs);