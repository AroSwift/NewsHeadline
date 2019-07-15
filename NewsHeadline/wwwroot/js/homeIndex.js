'use strict';

// Get a URI
function generateURL(country, category, apiKey) {
    return "https://newsapi.org/v2/top-headlines?country=" + country + "&category=" + category + "&apiKey=" + apiKey + "&pageSize=100";
}

// Get a random number between x and y
function getRandomNumber(max, min) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

// Make an AJAX request to get a random news article 
function getNews(country, categorySources, apiKey) {
    // Get the number of categories
    let numCategories = Object.keys(categorySources).length - 1;
    // Get a random category
    let randomCategory = getRandomNumber(numCategories, 0);
    // Get a random category
    let selectedCategory = Object.keys(categorySources)[randomCategory];
    // Get the sources associated with the random category
    let sources = Object.values(categorySources)[randomCategory];

    let uri = generateURL(country, selectedCategory, apiKey);
    
    // Make an AJAX request to get the most recent headlines NewsAPI.org website, for the current user's news sources
    $.ajax({
        url: uri,
        beforeSend: function () {
            // Show a loading bar
            $(".lds-ellipsis").show();
            // Also, hide the news article
            $("#newsRow").hide();
        },
        complete: function () {
            // After the AJAX request is successfully completed,
            // Hide the loading bar over 2.5 seconds
            $(".lds-ellipsis").hide(2500);
        },
        success: function (response) {

            let allowedArticles = [];

            // Iterate over each article
            response.articles.forEach(function (article) {
                // When the article retrieved is one that belongs to the current user's news source, but only for the current category
                if (article.source["id"] != null && sources.includes(article.source["id"])) {
                    allowedArticles.push(article);
                }
            });

            // Get a random article
            let randomArticle = getRandomNumber(allowedArticles.length - 1, 0);
            let article = allowedArticles[randomArticle];
            
            // When at least one article is found
            if (article != undefined) {

                // Update the category
                $("#newsCategory").html(selectedCategory);

                // Get the author or provide a default (often null author)
                let author = article.author == null ? "Unkown Author" : article.author;

                // Set the new date
                let date = new Date(Date.parse(article.publishedAt));
                $("#newsDate").html("Published " + date.toDateString());

                // Set the title
                $("#newsTitle").html(article.title);
                // Set the author
                $("#newsAuthor").html("By " + author);
                // Set the description
                $("#newsDescription").html(article.description);
                // Set the image link
                $("#newsImgLink").attr("href", article.url);
                // Set the image source
                $("#newsImg").attr("src", article.urlToImage);

                // Set the article link address
                $("#newsArticleLink").attr("href", article.url);
                // Set the article text to the URI
                $("#newsArticleLink").text(article.url);

                // Slowly fade in the news article over 1.5 seconds
                $("#newsRow").fadeIn(1500);

                // Hide the error block in case it has happened in the past
                $("#errorBlock").hide();
            } else {
                // Display an error when something goes wrong
                let content = `
                    <p class="col-12 errorNoArticle">No news sources for the category "` + selectedCategory + `"! Please add more news sources and categories!</p>
                    <p class="col-12"><a class="btn btn-default" href="/Profile">Add News Sources</a></p>`;
                $("#errorBlock").html(content);
            }
            
        },
        error: function () {
            // Display an error when something goes wrong
            let content = `
                    <p class="col-12 errorNoArticle">No news sources for the category "` + selectedCategory + `"! Please add more news sources and categories!</p>
                    <p class="col-12"><a class="btn btn-default" href="/Profile">Add News Sources</a></p>`;
            $("#errorBlock").html(content);
        }
    });
}