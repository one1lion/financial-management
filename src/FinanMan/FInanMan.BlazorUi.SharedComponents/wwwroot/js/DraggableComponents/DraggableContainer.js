export function handleDragStart(event) {
    event.dataTransfer.setDragImage(new Image(), 0, 0);
}
export function capturePointerEvents(elem) {
    elem.setPointerCapture(1);
}
export function releasePointerEvents(elem) {
    elem.releasePointerCapture(1);
}
//# sourceMappingURL=DraggableContainer.js.map