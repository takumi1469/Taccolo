// Call getSearchResult after the page is loaded
/*document.addEventListener("DOMContentLoaded", getSearchResultTop);*/

if (window.location.pathname == "/") {
    document.addEventListener("DOMContentLoaded", getSearchResultTop);
}
else if (window.location.pathname == "/OwnLS") {
    document.addEventListener("DOMContentLoaded", getSearchResultOwn);
}
else if (window.location.pathname == "/FavoriteLS") {
    document.addEventListener("DOMContentLoaded", getSearchResultFavorite);
}

else if (window.location.pathname == "/UserPage") {
    document.addEventListener("DOMContentLoaded", getSearchResultUser);
}

function getSearchResult(event, endpoint) {

    const keywords = document.getElementById("input-narrow-search").value;
    const sourceLanguage = document.getElementById("select-from-language").value;
    const targetLanguage = document.getElementById("select-to-language").value;
    const matchAndOr = document.getElementById("select-or-and").value; 
    const slugElement = document.getElementById("p-slug");
    const slug = slugElement ? slugElement.textContent : null;

    const data = {
        Keywords : keywords,
        SourceLanguage : sourceLanguage,
        TargetLanguage : targetLanguage,
        MatchAndOr: matchAndOr
    }

    if (slug) {
        data.Slug = slug
    }

    console.log(`Slug is ${slug}`);

    fetch(endpoint, {
        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
        .then(response => {
            if (!response.ok) {
                alert("Searching wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Learning Sets searched successfully:", result);
            showLearningSets(result);
        })
        .catch(error => {
            console.error("Error getting Learning Sets:", error);
            alert("An error occurred while getting Learning Sets.");
        });
}

function getSearchResultTop(event) {
    getSearchResult(event, `/api/Search/SearchLsTop`);
}

function getSearchResultOwn(event) {
    console.log("getSearchResultOwn has been called");
    getSearchResult(event, `/api/Search/SearchLsOwn`);
}
function getSearchResultFavorite(event) {
    console.log("getSearchResultFavorite has been called");
    getSearchResult(event, `/api/Search/SearchLsFavorite`);
}

function getSearchResultUser(event) {
    console.log("getSearchResultUser has been called");
    getSearchResult(event, `/api/Search/SearchLsUser`);
}


function showLearningSets(sets) {
    const divLearningSets = document.getElementById("div-learning-sets");
    divLearningSets.innerHTML = "";

    sets.forEach(ls => {
        const divEachLs = document.createElement("div");
        divEachLs.className = "div-each-ls";

        const firstWords = ls.input.split(/\s+/).slice(0, 20).join(" ");
        const title = ls.title ? ls.title : "(No title)";
        const date = ls.date ? ls.date : "(No date)";
        const username = ls.userName ? ls.userName : "(No user)";
        const source = ls.sourceLanguage;
        const target = ls.targetLanguage;

        divEachLs.innerHTML = `
                    <div class="div-h2-picture">
                        <h2 class="h2-first-words">${firstWords}</h2>
                    </div>
                        <h3 class="h3-title">${title}</h3>
                        <span class="span-date">${date}</span>
                        <span class="span-user">by ${username}</span>
                        </br>
                        <span class="span-source">from ${source}</span>
                        <span class="span-target">to ${target}</span>
                    `;

        const link = document.createElement("a");
        link.className = "a-each-ls";
        link.href = `EditViewLs/${ls.id}`; 
        link.appendChild(divEachLs);

        divLearningSets.appendChild(link);
    });
}

function clearSearchParameters(event) {
    document.getElementById("input-narrow-search").value = "";
    document.getElementById("select-from-language").value = "not-chosen";
    document.getElementById("select-to-language").value = "not-chosen";
    document.getElementById("select-or-and").value = "OR";

    if (window.location.pathname == "/") {
        getSearchResultTop();
    }
    else if (window.location.pathname == "/OwnLS") {
        getSearchResultOwn();
    }
    else if (window.location.pathname == "/FavoriteLS") {
        getSearchResultFavorite();
    }

    else if (window.location.pathname == "/UserPage") {
        getSearchResultUser();
    }
 }