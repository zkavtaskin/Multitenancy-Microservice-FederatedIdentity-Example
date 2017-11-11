
$(document).ready(function () {
   
    function carousel() {

        var windowHeight = $(window).height();

        if (637 > windowHeight)
            return;

        var documentHeight = $('#display').height();

        // - 1 because we start at 0
        var numberOfRows = (documentHeight / windowHeight) - 1; 

        if (numberOfRows == 0)
            return;

        console.log("going to animate");

        var currentOffset = $(window).scrollTop();
        var fuzzyCurrentRow = currentOffset / windowHeight;
        var fuzzyCurrentRowLeftOver = fuzzyCurrentRow % 1;
        var currentRow = fuzzyCurrentRow - fuzzyCurrentRowLeftOver;

        var newOffset = 0;
        if (currentRow != numberOfRows) {
            newOffset = (currentRow + 1) * windowHeight;
        }
       
        $('html, body').animate({
            scrollTop: newOffset
        }, 1000);
    }

    setInterval(carousel, 2000); 
});
