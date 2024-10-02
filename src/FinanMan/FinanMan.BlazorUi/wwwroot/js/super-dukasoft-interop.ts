interface Window {
    dukaSoftFuncs: DukaSoftFuncs,
}

interface DukaSoftFuncs {
    collapseSelectLists(): void,
    clipboardCopy: ClipboardCopy,
    handleDraggableContainerDragStart(event: DragEvent): void,
    getElement(elemOrSelector: string | Element): Element
}

interface ClipboardCopy {
    writeString(textToWrite: string): Promise<boolean>,
    writeElementContent(elemOrSelector: string | Element): Promise<boolean>,
}

window.dukaSoftFuncs = {
    collapseSelectLists: function () {
        const selectList:HTMLElement = document.querySelector('select:focus');
        if (selectList) {
            selectList.blur();
            selectList.focus();
        }
    },
    clipboardCopy: {
        writeString: function (textToWrite: string) {
            return navigator.clipboard.writeText(textToWrite)
                .then(function () {
                    return true;
                })
                .catch(function (error) {
                    return false;
                });
        },
        writeElementContent: function (elemOrSelector: string | Element) {
            const elem = window.dukaSoftFuncs.getElement(elemOrSelector);
            return window.dukaSoftFuncs.clipboardCopy.writeString(elem.textContent);
        }
    },
    getElement: function (elemOrSelector: string | Element) {
        let elem = (typeof elemOrSelector === 'string')
            ? document.getElementById(elemOrSelector)
            : elemOrSelector;

        if (!elem && typeof elemOrSelector === 'string') { elem = document.querySelector(elemOrSelector); }

        if (!elem?.getAttribute && elem?.id) {
            // It is possible the passed in value is an ElementRef from c#
            elem = document.getElementById(elem.id);
        }
        return elem;
    },
    handleDraggableContainerDragStart: function (event:DragEvent) {
        event.dataTransfer.setDragImage(new Image(), 0, 0);
    },
};
