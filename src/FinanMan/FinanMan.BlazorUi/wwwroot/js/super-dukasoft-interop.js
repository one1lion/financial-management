window.dukaSoftFuncs = {
    collapseSelectLists: function () {
        const selectList = document.querySelector('select:focus');
        if (selectList) {
            selectList.blur();
            selectList.focus();
        }
    },
    handleDraggableContainerDragStart: function (event) {
        console.log("We're dragon");
        event.dataTransfer.setDragImage(new Image(), 0, 0);
    }
};
