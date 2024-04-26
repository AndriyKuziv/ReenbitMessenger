var isSeen = false;

export function checkLastElement(dotNetInstance) {
    var lastItem = document.getElementById("last-item");
    var scrollableContainer = document.getElementById("scroll-paper");

    if (isScrolledIntoView(lastItem, scrollableContainer) && !isSeen) {
        var currentScrollPosition = getScrollPosition();
        dotNetInstance.invokeMethodAsync("GetGroupChatMessages");
        setScrollPosition(currentScrollPosition);
        isSeen = true;
    }
    else if (!isScrolledIntoView(lastItem, scrollableContainer) && isSeen) {
        isSeen = false;
    }
}

function isScrolledIntoView(elem, scrollElem) {
    var docViewTop = $(scrollElem).scrollTop();
    var docViewBottom = docViewTop + $(scrollElem).height();

    var elemTop = $(elem).offset().top;
    var elemBottom = elemTop + $(elem).height();

    return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
}

window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
            alert("Copied to clipboard!");
        })
            .catch(function (error) {
                alert(error);
            });
    }
}

export function getScrollPosition() {
    return document.getElementById('scroll-container').scrollTop;
}

export function setScrollPosition(position) {
    document.getElementById('scroll-container').scrollTop = position;
}
