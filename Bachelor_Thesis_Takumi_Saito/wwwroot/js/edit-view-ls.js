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

    divButtons.innerHTML = "";
    divButtons.appendChild(buttonSave);
    divButtons.appendChild(buttonCancel);
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
            switchToView(originalText, translatedText);
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });
}

function switchToView(originalText, translatedText) {
    //// Replace textareas with <p> elements showing the updated text
    //const divOriginal = document.getElementById("div-original");
    //const divTranslation = document.getElementById("div-translation");

    //divOriginal.innerHTML = `<p id="p-original">${originalText}</p>`;
    //divTranslation.innerHTML = `<p id="p-translated">${translatedText}</p>`;

    const pOriginal = document.createElement("p");
    pOriginal.id = "p-original";
    pOriginal.className = "p-original";
    pOriginal.textContent = originalText;

    const pTranslated = document.createElement("p");
    pTranslated.id = "p-original";
    pTranslated.className = "p-original";
    pTranslated.textContent = translatedText;

    const textareaOriginal = document.getElementById("textarea-original");
    const textareaTranslated = document.getElementById("textarea-translated");

    textareaOriginal.replaceWith(pOriginal);
    textareaTranslated.replaceWith(pTranslated);

    // Replace SAVE and CANCEL buttons with the EDIT button
    const divButtons = document.getElementById("div-buttons");
    divButtons.innerHTML = `
        <button class="button-edit" id="button-edit" onclick="switchToEdit()">EDIT</button>
    `;




    //// Get the p element with the original text
    //const pOriginal = document.getElementById("p-original");
    //const originalText = pOriginal.textContent;

    //const pTranslated = document.getElementById("p-translated");
    //const translatedText = pTranslated.textContent;

    //// Create a new textarea element
    //const textareaOriginal = document.createElement("textarea");
    //textareaOriginal.id = "textarea-original";
    //textareaOriginal.value = originalText;

    //const textareaTranslated = document.createElement("textarea");
    //textareaTranslated.id = "textarea-translated";
    //textareaTranslated.value = translatedText;


    //pOriginal.replaceWith(textareaOriginal);
    //pTranslated.replaceWith(textareaTranslated);



}