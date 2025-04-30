const lsId = document.getElementById("p-lsid").textContent;
const questions;

document.addEventListener("DOMContentLoaded", getQuestions); // Calls getQuestions after the page is loaded
function getQuestions() {
    fetch(`/api/Question/GiveQuestions/${lsId}`, {method: "GET"})
        .then(response => {
            if (!response.ok) {
                alert("Saving wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Questions received successfully:", result);
            questions = result;
        })
        .catch(error => {
            console.error("Error saving the learning set:", error);
            alert("An error occurred while saving the learning set.");
        });
}
