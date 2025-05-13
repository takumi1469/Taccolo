let slugAvailable = false;

document.getElementById("registerSubmit").addEventListener("click", checkSlug());
document.getElementById("button-check-slug").addEventListener("click", function (e) {
    e.preventDefault(); // <- This stops the form from submitting
    checkSlug(); // Call your function that checks if the slug is unique
});

const form = document.getElementById("registerForm");
form.addEventListener("submit", function (e) {
    if (!slugAvailable) {
        e.preventDefault(); // Stop the form from submitting
        alert("Slug is not available. Please choose a different one.");
    }
});

function checkSlug() {
    const slug = document.getElementById("input-slug").value;
    const data = {
        Slug: slug
    }

    if (slug == "") return;

    fetch(`/api/RegisterSlug/CheckSlug`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json" // Let the server know we're sending JSON
        },
        body: JSON.stringify(data) // Convert the data to JSON format
    })
        .then(response => {
            if (!response.ok) {
                alert("Checking wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Slug was checked successfully:", result);
            slugAvailable = !result;
            showCheckResult(result);
        })
        .catch(error => {
            console.error("Error getting Learning Sets:", error);
            alert("An error occurred while getting Learning Sets.");
        });   
}

function showCheckResult(result) {
    console.log("showCheckResult has been called, result is " + result);
    const messageSpan = document.getElementById("span-slug-check-result");
    messageSpan.textContent = "";

    if (!result) {
        messageSpan.textContent = "Slug is unique, cool!";
        messageSpan.style.color = "blue";
    }
    else {
        messageSpan.textContent = "Slug is taken, please choose something else :/"
        messageSpan.style.color = "red";
    }
}