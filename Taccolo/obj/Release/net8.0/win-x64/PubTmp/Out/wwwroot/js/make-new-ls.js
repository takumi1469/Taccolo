function embedVideo() {
    // Get the URL entered by the user
    const url = document.getElementById("video-url").value;

    //// Validate the URL and construct the YouTube embed URL (basic validation for YouTube)
    const youtubeRegex = /^https:\/\/www\.youtube\.com\/watch\?v=([a-zA-Z0-9_-]{11})$/;
    const match = url.match(youtubeRegex);

    // Extract the video ID from the URL
    const videoId = match[1];

    // Construct the embed URL
    //const embedUrl = `https://www.youtube.com/embed/${videoId}`;
    const embedUrl = `https://www.youtube.com/embed/${videoId}`;

    // Get the iframe element and set the src attribute to embed the video
    const iframe = document.getElementById("video-frame");
    iframe.src = embedUrl;
    iframe.style.display = "block";
}
