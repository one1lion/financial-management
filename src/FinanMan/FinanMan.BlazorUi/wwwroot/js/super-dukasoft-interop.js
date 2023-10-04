window.dukaSoftFuncs = {
    collapseSelectLists: function () {
        const selectList = document.querySelector('select:focus');
        if (selectList) {
            selectList.blur();
            selectList.focus();
        }
    },
    clipboardCopy: {
        writeString: function (textToWrite) {
            return navigator.clipboard.writeText(textToWrite)
                .then(function () {
                return true;
            })
                .catch(function (error) {
                return false;
            });
        },
        writeElementContent: function (elemOrSelector) {
            const elem = window.dukaSoftFuncs.getElement(elemOrSelector);
            return window.dukaSoftFuncs.clipboardCopy.writeString(elem.textContent);
        }
    },
    getElement: function (elemOrSelector) {
        let elem = (typeof elemOrSelector === 'string')
            ? document.getElementById(elemOrSelector)
            : elemOrSelector;
        if (!elem && typeof elemOrSelector === 'string') {
            elem = document.querySelector(elemOrSelector);
        }
        if (!(elem === null || elem === void 0 ? void 0 : elem.getAttribute) && (elem === null || elem === void 0 ? void 0 : elem.id)) {
            // It is possible the passed in value is an ElementRef from c#
            elem = document.getElementById(elem.id);
        }
        return elem;
    },
    handleDraggableContainerDragStart: function (event) {
        event.dataTransfer.setDragImage(new Image(), 0, 0);
    },
};
//# sourceMappingURL=super-dukasoft-interop.js.map