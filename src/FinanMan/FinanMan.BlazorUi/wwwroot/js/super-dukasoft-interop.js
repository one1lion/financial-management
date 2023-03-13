window.dukaSoftFuncs = {
    collapseSelectLists: function () {
        const selectList = document.querySelector('select:focus');
        if (selectList) {
            selectList.blur();
            selectList.focus();
        }
    }
};