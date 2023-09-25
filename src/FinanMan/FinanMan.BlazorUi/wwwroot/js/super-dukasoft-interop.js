window.dukaSoftFuncs = {
    collapseSelectLists: function () {
        const selectList = document.querySelector('select:focus');
        if (selectList) {
            selectList.blur();
            selectList.focus();
        }
    },
    handleDraggableContainerDragStart: function (event) {
        event.dataTransfer.setDragImage(new Image(), 0, 0);
    }
};
