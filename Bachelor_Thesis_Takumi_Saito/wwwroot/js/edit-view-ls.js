let originalTextUnchanged;
let translatedTextUnchanged;
let wmpUnchanged;
let WmpHTMLUnchanged;
let wmpsToDelete = [];
const lsId = document.getElementById("p-id").textContent;

window.onload = attachEventListenerToIcons();

function attachEventListenerToIcons() {
    const addWordButtons = document.querySelectorAll(".icon-plus-word");
    const addMeaningButtons = document.querySelectorAll(".icon-plus-meaning");
    const deleteWordButtons = document.querySelectorAll(".icon-minus-word");
    const deleteMeaningButtons = document.querySelectorAll(".icon-minus-meaning");

    addWordButtons.forEach(el => {
        el.removeEventListener("click", addWord);
        el.addEventListener("click", addWord);
    });

    addMeaningButtons.forEach(el => {
        el.removeEventListener("click", addMeaning);
        el.addEventListener("click", addMeaning);
    });

    deleteWordButtons.forEach(el => {
        el.removeEventListener("click", deleteWord);
        el.addEventListener("click", deleteWord);
    });

    deleteMeaningButtons.forEach(el => {
        el.removeEventListener("click", deleteMeaning);
        el.addEventListener("click", deleteMeaning);
    });

}


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

    // Prepare the data to send
    const data = {
        Id: id,
        OriginalText: originalText,
        TranslatedText: translatedText,
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
            // Move on to save WMPs
            saveWordMeaningPairs();
            // Update the UI to "view mode"
            switchToViewBySave(originalText, translatedText);
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });

}

function saveWordMeaningPairs() {
    // Get the updated WMPs
    const updatedWmps = getAllWmpsAfterEdit();

    // Prepare the data to send
    const data = {
        WordMeaningPairs: updatedWmps,
        WmpsToDelete: wmpsToDelete
    };

    // Make the fetch call
    fetch(`/api/WordMeaningPair/UpdateWmp`, {

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
            console.log("WordMeaningPairs updated successfully TEST3:", result);

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

    document.querySelectorAll(".p-alternative-meaning").forEach(el => {
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
        const alternatives = Array.from(wmp.querySelectorAll(".p-alternative-meaning"))
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
    let newOrder = 1;

    wmpContainers.forEach(wmp => {
        if (wmp.querySelector(".input-word").value.trim() != "") {
            const Word = wmp.querySelector(".input-word").value;
            const TranslatedText = wmp.querySelector(".input-translated-text")?.value || "";
            const LsId = document.getElementById("p-id")?.textContent.trim() || "";
            const Id = wmp.querySelector(".p-wmp-id")?.textContent.trim() || "";
            const Order = newOrder;
            newOrder++;

            // Extract alternatives
            const Alternatives = Array.from(wmp.querySelectorAll(".input-alternative-meaning"))
                .map(input => input.value)
                .filter(value => value.trim() !== ""); // Remove empty values

            wmpArray.push({ Id, LsId, Word, TranslatedText, Alternatives, Order });
        }
    });

    console.log(JSON.stringify(wmpArray));
    return wmpArray;
}

function switchToViewWmpBySave() {
    document.querySelectorAll(".input-word").forEach(el => {

        if (el.value.trim() === "") {
            el.closest(".div-wmp")?.remove();
        }
        else {
            const p = document.createElement("p");
            const strong = document.createElement("strong");
            strong.textContent = el.value;
            p.appendChild(strong);
            p.className = "p-word";
            el.replaceWith(p);
        }
    });

    document.querySelectorAll(".input-translated-text").forEach(el => {
        const p = document.createElement("p");
        p.textContent = el.value;
        p.className = "p-translated-text";
        el.replaceWith(p);
    });

    document.querySelectorAll(".input-alternative-meaning").forEach(el => {

        const closestDivAlt = el.closest(".div-each-alt");
        const closestDivWmp = closestDivAlt.closest(".div-wmp");
        const listDivAlts = closestDivWmp.querySelectorAll(".div-each-alt");

        if (listDivAlts.length > 1 && el.value === "") {
            closestDivAlt.remove();
        }
        else {
            const p = document.createElement("p");
            p.textContent = el.value;
            p.className = "p-alternative-meaning";
            el.replaceWith(p);
        }
    });

    // Make icons disappear
    const icons = document.querySelectorAll(".icon-for-edit").forEach(el => {
        el.style.display = "none";
    });

}

function switchToViewWmpByCancel() {
    document.getElementById("div-glossary").innerHTML = WmpHTMLUnchanged;
    attachEventListenerToIcons();
}

function addWord(event) {
    // Step 1: Find the closest WMP container (assuming it's wrapped in a div)
    const button = event.target;
    const closestDivWmp = button.closest(".div-wmp");
    const currentOrder = parseInt(closestDivWmp.querySelector(".p-order").textContent, 10);
    //const newOrder = currentOrder + 1;

    // Step 2: Create a new div for the new WordMeaningPair inputs
    const newDivWmp = document.createElement("div");
    newDivWmp.classList.add("div-wmp"); // Same class as other WMPs

    //Give placeholder GUID to signify to backend that it's a new WMP
    const newGuid = "00000000-0000-0000-0000-000000000000" 

    newDivWmp.innerHTML = `
                        <img src="/icons/star.svg" class="icon icon-star" title="add word below">
                        <input class="input-word" placeholder="Word"><span> :</span>
                        <input class="input-translated-text" placeholder="Meaning">
                        <img src="/icons/plus3.svg" class="icon icon-plus icon-for-edit icon-plus-word" title="add word below" style="display: inline;">
                        <img src="/icons/minus3.svg" class="icon icon-minus icon-for-edit icon-minus-word" title="delete this word" style="display: inline;">
                        <br>
                        <p class="p-order no-display">dummy order</p>
                        <p class="p-wmp-id no-display">${newGuid}</p>
                            <span class="span-or">or: <img src="/icons/plus3.svg" class="icon icon-plus icon-for-edit icon-plus-meaning" title="add meaning" style="display: inline;"></span>
                                        <div class="div-each-alt">
                                        <input class="input-alternative-meaning" placeholder="other meaning">
                                        <img src="/icons/minus3.svg" class="icon icon-minus icon-for-edit icon-minus-meaning" title="delete this meaning" style="display: inline;">
                                        </div>
    `;

    // Step 3: Insert the new WMP div **right after** the current WMP container
        closestDivWmp.insertAdjacentElement("afterend", newDivWmp);

    attachEventListenerToIcons();
}

function addMeaning(event) {
    const button = event.target;
    const closestDivAlt = button.closest(".div-each-alt");
    const newDivAlt = document.createElement("div");
    newDivAlt.className = "div-each-alt";
    newDivAlt.innerHTML = `
                        <input class="input-alternative-meaning" placeholder="other meaning">
                        <img src="/icons/plus3.svg" class="icon icon-plus icon-for-edit icon-plus-meaning" title="add meaning below" style="display: inline;">
                        <img src="/icons/minus3.svg" class="icon icon-minus icon-for-edit icon-minus-meaning" title="delete this meaning" style="display: inline;">
    `;

    closestDivAlt.insertAdjacentElement("afterend", newDivAlt);

    attachEventListenerToIcons();
}

function deleteWord(event) {
    const button = event.target;
    const closestDivWmp = button.closest(".div-wmp");
    const guidToDelete = closestDivWmp.querySelector(".p-wmp-id").textContent.trim();
    wmpsToDelete.push(guidToDelete);
    
    closestDivWmp.remove();
}

function deleteMeaning(event) {
    const button = event.target;
    const closestDivAlt = button.closest(".div-each-alt");
    const closestDivWmp = closestDivAlt.closest(".div-wmp");
    const listDivAlts = closestDivWmp.querySelectorAll(".div-each-alt");
    if (listDivAlts.length > 1) {
        closestDivAlt.remove();
    }
    else {
        closestDivAlt.innerHTML = `
                        <input class="input-alternative-meaning" placeholder="other meaning">
                        <img src="/icons/plus3.svg" class="icon icon-plus icon-for-edit icon-plus-meaning" title="add meaning below" style="display: inline;">
                        <img src="/icons/minus3.svg" class="icon icon-minus icon-for-edit icon-minus-meaning" title="delete this meaning" style="display: inline;">
    `;
    }
}


// <<From here down is for Comments and Helps>>
function addComment(event) {
    let dateTime = new Date().toLocaleString('en-US', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        hour12: false
    });

    // update the view with added comments
    const button = event.target;
    const username = document.getElementById("p-username").textContent;
    const comment = document.getElementById("textarea-comment").value.trim(); // trim() removes white space at start or end
    const newDivComment = document.createElement("div");
    newDivComment.className = "div-each-comment";
    if (comment == "") {}
    else {
        newDivComment.innerHTML = `
    <p class="p-comment-username">${username} <span class="span-date-time">(${dateTime})</span></p>
    <p class="p-comment">${comment}</p>
    `;
        button.insertAdjacentElement("afterend", newDivComment);
        document.getElementById("p-no-comment")?.remove(); //will not happen if p-no-comment doesn't exist
    }

    // Ajax request to save comments to database
    // Prepare the data to send
    const data = {
        Body: comment,
        LsId: lsId,
        Date: dateTime
    };

    fetch(`/api/Comment/AddComment`, {

        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
        .then(response => {
            if (!response.ok) {
                alert("Saving wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Comment saved successfully TESTComment:", result);
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });
}

function addHelpRequest(event) {
    // update the view with added comments
    const button = event.target;
    //const username = document.getElementById("p-username").textContent;
    const helpRequest = document.getElementById("textarea-help-request").value.trim(); // trim() removes white space at start or end
    const newDivHelpRequest = document.createElement("div");
    newDivHelpRequest.className = "div-each-help-request";
    if (helpRequest == "") { }
    else {
        if (isAuthenticated) {
            newDivHelpRequest.innerHTML = `
            <h4 class="h4-help-request">${helpRequest}</h4>
            <textarea class="textarea-help-reply" id="textarea-help-reply"></textarea>
            <button id="button-add-help-reply" class="button-add-help-reply button-comment-help">Reply</button>
          `;
        }
        else {
            newDivHelpRequest.innerHTML = `
            <h4 class="h4-help-request">${helpRequest}</h4>
            <textarea class="textarea-help-reply" id="textarea-help-reply"></textarea>
            <a class="a-redirect-login" href="/Identity/Account/Login"><button id="button-add-help-reply" class="button-add-help-reply button-comment-help">Reply</button></a>

          `;
        }

        button.insertAdjacentElement("afterend", newDivHelpRequest);
        document.getElementById("button-add-help-reply").addEventListener("click", addHelpReply);
        document.getElementById("p-no-help-request")?.remove(); //will not happen if p-no-help-request doesn't exist
    }

    // Ajax request to save comments to database
    // Prepare the data to send
    const data = {
        Body: helpRequest,
        LsId: lsId
    };

    fetch(`/api/HelpRequest/AddHelpRequest`, {

        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
        .then(response => {
            if (!response.ok) {
                alert("Saving wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Comment saved successfully TESTHelpRequest:", result);
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });
}

function addHelpReply(event) {
    // update the view with added comments
    const button = event.target;
    //const username = document.getElementById("p-username").textContent;
    const helpReply = document.getElementById("textarea-help-request").value.trim(); // trim() removes white space at start or end
    const newDivHelpRequest = document.createElement("div");
    newDivHelpRequest.className = "div-each-help-request";
    if (helpRequest == "") { }
    else {
        if (isAuthenticated) {
            newDivHelpRequest.innerHTML = `
            <h4 class="h4-help-request">${helpRequest}</h4>
            <button id="button-add-help-reply" class="button-add-help-reply button-comment-help">Reply</button>
          `;
        }
        else {
            newDivHelpRequest.innerHTML = `
            <h4 class="h4-help-request">${helpRequest}</h4>
            <a class="a-redirect-login" href="/Identity/Account/Login"><button id="button-add-help-reply" class="button-add-help-reply button-comment-help">Reply</button></a>

          `;
        }

        button.insertAdjacentElement("afterend", newDivHelpRequest);
        //document.getElementById("button-add-help-reply").addEventListener("click", addHelpReply);
        //document.getElementById("p-no-help-request")?.remove(); //will not happen if p-no-help-request doesn't exist
    }

    // Ajax request to save comments to database
    // Prepare the data to send
    const data = {
        Body: helpRequest,
        LsId: lsId
    };

    fetch(`/api/HelpRequest/AddHelpRequest`, {

        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
        .then(response => {
            if (!response.ok) {
                alert("Saving wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Comment saved successfully TESTHelpRequest:", result);
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });
}