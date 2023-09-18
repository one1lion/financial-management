export function handleDragStart(event) {
    event.dataTransfer.setDragImage(new Image(), 0, 0);
}
export function capturePointerEvents(elem) {
    console.log("Pointer captures on", elem);
    elem.setPointerCapture(1);
}
export function releasePointerEvents(elem) {
    console.log("Pointer released on", elem);
    elem.releasePointerCapture(1);
}
//# sourceMappingURL=DraggableContainer.js.map