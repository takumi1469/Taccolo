let originalTextUnchanged;
let translatedTextUnchanged;
let wmpUnchanged;
let WmpHTMLUnchanged;
const lsId = document.getElementById("p-id").textContent;

window.onload = function () {
    // Attach event listeners here
    const addWordButtons = document.querySelectorAll(".icon-plus-word");
    const addMeaningButtons = document.querySelectorAll(".icon-plus-meaning");
    const deleteWordButtons = document.querySelectorAll(".icon-minus-word");
    const deleteMeaningButtons = document.querySelectorAll(".icon-minus-meaning");

    addWordButtons.forEach(el => {
        el.addEventListener("click", addWord);
    });

    addMeaningButtons.forEach(el => {
        el.addEventListener("click", addMeaning);
    });

    deleteWordButtons.forEach(el => {
        el.addEventListener("click", deleteWord);
    });

    deleteMeaningButtons.forEach(el => {
        el.addEventListener("click", deleteMeaning);
    });



    myElement.addEventListener('click', function () {
        console.log('Element clicked');
    });
};


// When you switch to EDIT mode, capture the initial texts
function getUnchangedItems() {
    originalTextUnchanged = document.getElementById("p-original").textContent;
    translatedTextUnchanged = document.getElementById("p-translated").textContent;
    wmpUnchanged = getAllWmpsBeforeEdit();
    WmpHTMLUnchanged = document.getElementById("div-glossary").innerHTML;
};

function switchToEdit() {
    getUnchangedItems();
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

    // Make icons appear
    const icons = document.querySelectorAll(".icon-for-edit").forEach(el => {
        el.style.display = "inline";
    });

    switchToEditWmps();
}

function saveLearningSet() {
    // Get the updated data from the textareas
    const originalText = document.getElementById("textarea-original").value;
    const translatedText = document.getElementById("textarea-translated").value;

    const id = document.getElementById("p-id").textContent;

    // Get the updated WMPs
    const updatedWmps = getAllWmpsAfterEdit();

    // Prepare the data to send
    const data = {
        Id: id,
        OriginalText: originalText,
        TranslatedText: translatedText,
        WordMeaningPairs: updatedWmps
    };

    // Make the fetch call
    fetch(`/api/LearningSet/UpdateLs`, {

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
            console.log("Learning Set updated successfully TEST3:", result);

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

    switchToViewWmpBySave();
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

    switchToViewWmpByCancel()
}


// <<From here down is for List of Words>>
class WordMeaningPair {
    constructor(word, translatedText, alternatives, lsId, wmpId, order) {
        this.Word = word; // string
        this.TranslatedText = translatedText; //string 
        this.Alternatives = alternatives; //array of strings
        this.LsId = lsId;
        this.Id = wmpId;
        this.Order = order;
    }
}

function switchToEditWmps() {
    document.querySelectorAll(".p-word").forEach(el => {
        const input = document.createElement("input");
        input.value = el.textContent;
        input.className = "input-word";
        el.replaceWith(input);
    });


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
        const lsid = document.getElementById("p-id")?.textContent.trim() || "";
        const wmpId = wmp.querySelector(".p-wmp-id")?.textContent.trim() || "";
        const order = wmp.querySelector(".p-order")?.textContent || "";

        // Extract alternatives
        const alternatives = Array.from(wmp.querySelectorAll(".li-alternative-meaning"))
            .map(p => p.textContent)
            .filter(value => value.trim() !== ""); // Remove empty values

        wmpArray.push({ wmpId, lsid, word, translatedText, alternatives, order });
    });

    //console.log(JSON.stringify(wmpArray));
    return wmpArray;
}
function getAllWmpsAfterEdit() {
    const wmpContainers = document.querySelectorAll(".div-wmp"); // Select all WMP containers
    let wmpArray = [];

    wmpContainers.forEach(wmp => {
        const Word = wmp.querySelector(".input-word")?.value || "";
        const TranslatedText = wmp.querySelector(".input-translated-text")?.value || "";
        const LsId = document.getElementById("p-id")?.textContent.trim() || "";
        const Id = wmp.querySelector(".p-wmp-id")?.textContent.trim() || "";
        const Order = wmp.querySelector(".p-order")?.textContent || "";

        // Extract alternatives
        const Alternatives = Array.from(wmp.querySelectorAll(".input-alternative-meaning"))
            .map(input => input.value)
            .filter(value => value.trim() !== ""); // Remove empty values

        wmpArray.push({ Id, LsId, Word, TranslatedText, Alternatives, Order });
    });

    console.log(JSON.stringify(wmpArray));
    return wmpArray;
}

function switchToViewWmpBySave() {
    document.querySelectorAll(".input-word").forEach(el => {
        const p = document.createElement("p");
        const strong = document.createElement("strong");
        strong.textContent = el.value;
        p.appendChild(strong);
        p.className = "p-word";
        el.replaceWith(p);
    });

    document.querySelectorAll(".input-translated-text").forEach(el => {
        const p = document.createElement("p");
        p.textContent = el.value;
        p.className = "p-translated-text";
        el.replaceWith(p);
    });

    document.querySelectorAll(".input-alternative-meaning").forEach(el => {
        const li = document.createElement("li");
        li.textContent = el.value;
        li.className = "li-alternative-meaning";
        el.replaceWith(li);
    });

    // Make icons disappear
    const icons = document.querySelectorAll(".icon-for-edit").forEach(el => {
        el.style.display = "none";
    });

}

function switchToViewWmpByCancel() {
    document.getElementById("div-glossary").innerHTML = WmpHTMLUnchanged;

    // Attach events to Plus and Minus buttons





}

function addWord() {
    // Step 1: Find the closest WMP container (assuming it's wrapped in a div)
    const closestDivWmp = button.closest(".div-wmp");
    const currentOrder = parseInt(closestDivWmp.querySelector(".p-order").textContent, 10);
    const newOrder = currentOrder + 1;

    // Step 2: Create a new div for the new WordMeaningPair inputs
    const newDivWmp = document.createElement("div");
    newDivWmp.classList.add("div-wmp"); // Same class as other WMPs

    const newGuid = crypto.randomUUID();

    newDivWmp.innerHTML = `
                        <img src="/icons/star.svg" class="icon icon-star" title="add word below">
                        <input class="input-word" placeholder="Word"><span> :</span>
                        <input class="input-translated-text" placeholder="Meaning">
                        <img src="/icons/plus3.svg" class="icon icon-plus icon-for-edit icon-plus-word" title="add word below" style="display: inline;">
                        <img src="/icons/minus3.svg" class="icon icon-minus icon-for-edit icon-minus-word" title="delete this word" style="display: inline;">
                        <br>
                        <p class="p-order no-display">${newOrder}</p>
                        <p class="p-wmp-id no-display">${newGuid}</p>
                            <span class="span-or">or: <img src="/icons/plus3.svg" class="icon icon-plus icon-for-edit icon-plus-meaning" title="add meaning" style="display: inline;"></span>
                            <ul class="ul-alternatives">
                                        <div class="div-each-alt">
                                        <input class="input-alternative-meaning" placeholder="other meaning">
                                        <img src="/icons/minus3.svg" class="icon icon-minus icon-for-edit icon-minus-meaning" title="delete this meaning" style="display: inline;">
                                        </div>
                            </ul>
    `;

    // Step 3: Insert the new WMP div **right after** the current WMP container
    wmpContainer.insertAdjacentElement("afterend", newDivWmp);
}

function addMeaning() {

}

function deleteWord() {

}

function deleteMeaning() {

}