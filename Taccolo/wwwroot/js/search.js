

function getSearchResult() {

    const keywords = document.getElementById("input-search").value;
    const sourceLanguage = document.getElementById("select-from-language").value;
    const targetLanguage = document.getElementById("select-to-language").value;
    const matchAndOr = document.getElementById("select-or-and").value; 

    fetch(`/api/Search/AddHelpReply`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
}