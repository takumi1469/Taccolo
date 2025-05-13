//function getAllLearningSets() {
//    fetch(`/api/LsToShow/PassAllLs`)
//  .then(response => response.json())  // Parse JSON response
//  .catch(error => console.error('Error:', error));
//}

function getAllLearningSets(event) {
    fetch(`/api/LsToShow/PassAllLs`)
        .then(response => response.json())  // Parse JSON response
        .then(learningSets => {
            console.log(learningSets);  // Now you have the array of Learning Sets

            // Example of accessing individual properties:
            learningSets.forEach(ls => {
                console.log("TITLE IS " + ls.Title + " USERNAME IS " + ls.Username); // Accessing the title of each Learning Set
            });
        })
        .catch(error => console.error('Error:', error));
}