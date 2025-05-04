const lsId = document.getElementById("p-lsid").textContent;
let questions;
let index = 0;
let answered = false;
const result = document.getElementById("p-result");
let correctAnswer = 0;
let wrongAnswer = 0;


// Call getQuestions after the page is loaded
document.addEventListener("DOMContentLoaded", getQuestions); 
function getQuestions() {
    fetch(`/api/Question/GiveQuestions/${lsId}`, { method: "GET" })
        .then(response => {
            if (!response.ok) {
                alert("Getting questions wasn't successful")
            }
            return response.json(); // Parse the JSON response
        })
        .then(result => {
            console.log("Questions received successfully:", result);
            questions = result;
            showQuestions();
        })
        .catch(error => {
            console.error("Error getting questions:", error);
            alert("An error occurred while getting questions.");
        });
}

function showQuestions() {
    if (index > questions.length - 1) {
        endOfQuestions();
        return;
    };

    const divQuestion = document.getElementById("div-question");
    let currentQuestion = questions[index];
    let choices = [
        { text: currentQuestion.correctChoice, isCorrect: true },
        { text: currentQuestion.wrongChoices[0], isCorrect: false },
        { text: currentQuestion.wrongChoices[1], isCorrect: false },
        { text: currentQuestion.wrongChoices[2], isCorrect: false },
    ];

    for (let i = choices.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [choices[i], choices[j]] = [choices[j], choices[i]];
    }

    divQuestion.innerHTML = `
            <div class="div-left-card">
                <span class="span-word">${currentQuestion.word}</span>
            </div >
            <div class="div-right-choices">
                <label class="label-choice">
                    <input class="input-choice" type="radio" name="input-choice" data-correct=${choices[0].isCorrect}> ${choices[0].text}
                </label>
                <label class="label-choice">
                    <input class="input-choice" type="radio" name="input-choice" data-correct=${choices[1].isCorrect}> ${choices[1].text}
                </label>
                <label class="label-choice">
                    <input class="input-choice" type="radio" name="input-choice" data-correct=${choices[2].isCorrect}> ${choices[2].text}
                </label>
                <label class="label-choice">
                    <input class="input-choice" type="radio" name="input-choice" data-correct=${choices[3].isCorrect}> ${choices[3].text}
                </label>
            </div>
        `;

    const radios = document.querySelectorAll(".input-choice");
    radios.forEach(radio => {
        radio.addEventListener("change", checkCorrectOrWrong);
    });
}

function checkCorrectOrWrong(event) {
    const radio = event.target;
    const radios = document.querySelectorAll(".input-choice");
    //const result = document.getElementById("p-result");
    correctOrWrong = radio.getAttribute("data-correct");

    // Clear styling of all labels first
    radios.forEach(r => {
        r.parentElement.style.color = "";
        r.parentElement.style.fontWeight = "";
    });

    if (correctOrWrong == "true" && answered == false) {
        answered = true;
        radios.forEach(r => r.disabled = true);
        correctAnswer++;
        radio.parentElement.style.color = "blue";
        radio.parentElement.style.fontWeight = "bold";
        result.textContent = "Correct!";
        result.style.fontSize = "2vw";
        result.style.color = "blue";
    }
    if (correctOrWrong == "false" && answered == false) {
        answered = true;
        radios.forEach(r => r.disabled = true);
        wrongAnswer++;

        radio.parentElement.style.color = "red";

        // Find and highlight the correct one
        radios.forEach(r => {
            if (r.dataset.correct === "true") {
                r.parentElement.style.color = "blue";
                r.parentElement.style.fontWeight = "bold";
            }
        });
        result.textContent = "Wrong";
        result.style.fontSize = "2vw";
        result.style.color = "red";
    }
}

function goNext(event) {
    answered = false;
    result.textContent = "";
    index++;
    showQuestions();
}

function endOfQuestions() {
    result.textContent = `Result - correct:${correctAnswer}, wrong:${wrongAnswer}, skipped:${questions.length - correctAnswer - wrongAnswer} in ${questions.length} questions`;
    result.style.fontSize = "2vw";
    result.style.color = "blue";

    const divNextButton = document.querySelector(".div-next-button");
    divNextButton.innerHTML = `
    <button class="button-next" id="button-reload">Try again</button>
    <a href="/"><button class="button-finish">Finish and go to top</button></a>
    `
    const buttonReload = document.getElementById("button-reload");
    buttonReload.addEventListener("click", () => { location.reload(); });
}

